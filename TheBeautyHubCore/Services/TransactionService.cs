using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TheBeautyHubCore.Constants;
using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Enums;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class TransactionService : ITransactionService
    {
        public const string EditWindowClosedCode = "TRANSACTION_EDIT_CLOSED";
        private static readonly TimeSpan EditWindow = TimeSpan.FromHours(2);

        private readonly ITransactionRepository _transactionRepository;
        private readonly IServicesRepository _servicesRepository;
        private readonly IExpensesTypeRepository _expensesTypeRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IBranchRepository _branchRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IServicesRepository servicesRepository,
            IExpensesTypeRepository expensesTypeRepository,
            IStaffRepository staffRepository,
            IBranchRepository branchRepository)
        {
            _transactionRepository = transactionRepository;
            _servicesRepository = servicesRepository;
            _expensesTypeRepository = expensesTypeRepository;
            _staffRepository = staffRepository;
            _branchRepository = branchRepository;
        }

        public async Task<TransactionBootstrapDto> GetBootstrapAsync(
            Guid accountId,
            Guid userId,
            IReadOnlyList<string> roles)
        {
            var services = await _servicesRepository.GetByAccountIdAsync(accountId);
            var expenses = await _expensesTypeRepository.GetByAccountIdAsync(accountId);
            var staff = await _staffRepository.GetAllAsync(accountId);
            var branches = (await _branchRepository.GetAllAsync(accountId)).ToList();
            var usage = await _transactionRepository.GetServiceUsageCountsAsync(accountId);
            var frequentIds = usage
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .Select(kv => kv.Key)
                .ToHashSet();

            var last = await _transactionRepository.GetLatestByUserAsync(accountId, userId);
            var staffForUser = await _staffRepository.GetByUserIdAsync(userId, accountId);

            Guid? branchId = staffForUser?.BranchId ?? last?.BranchId ?? branches.FirstOrDefault()?.BranchId;

            return new TransactionBootstrapDto
            {
                Services = services.Select(s => new TransactionBootstrapServiceDto
                {
                    Id = s.ServiceId,
                    Name = s.ServiceName,
                    Price = s.ServicePrice,
                    Frequent = frequentIds.Contains(s.ServiceId)
                }).ToList(),
                Expenses = expenses.Select(e => new TransactionNamedItemDto
                {
                    Id = e.ExpensesTypeId,
                    Name = e.ExpensesTypeName
                }).ToList(),
                Staff = staff.Select(s => new TransactionNamedItemDto
                {
                    Id = s.StaffId,
                    Name = s.FullName
                }).ToList(),
                Branches = branches.Select(b => new TransactionNamedItemDto
                {
                    Id = b.BranchId,
                    Name = b.Name
                }).ToList(),
                UserRole = MapRole(roles),
                LoggedInUserId = userId,
                LoggedInBranchId = branchId,
                LastPaymentMode = last?.PaymentMode,
                LastTransactionType = last?.Type
            };
        }

        public async Task<TransactionSavedDto> CreateAsync(SaveTransactionDto dto)
        {
            ValidateWrite(dto, requireIdempotency: true);

            if (!string.IsNullOrWhiteSpace(dto.IdempotencyKey))
            {
                var existing = await _transactionRepository.GetByIdempotencyKeyAsync(dto.IdempotencyKey.Trim(), dto.AccountId);
                if (existing != null)
                    return MapSaved(existing);
            }

            await EnsureBranchAsync(dto.BranchId, dto.AccountId);
            var lines = await BuildLinesAsync(dto);
            var totals = ComputeTotals(lines, dto.CouponCode);

            var now = DateTime.UtcNow;
            var transaction = new Transaction
            {
                AccountId = dto.AccountId,
                CreatedBy = dto.UserId,
                CreatedAt = now,
                IsDeleted = false,
                Code = await NextCodeAsync(dto.AccountId),
                Type = TransactionKinds.ParseOrThrow(dto.Type, ApiMessages.TransactionTypeInvalid).ToApiValue(),
                BranchId = dto.BranchId,
                PaymentMode = PaymentModes.ParseOrThrow(dto.PaymentMode, ApiMessages.TransactionPaymentModeInvalid).ToApiValue(),
                CustomerName = NullIfEmpty(dto.CustomerName),
                CustomerMobile = NullIfEmpty(dto.CustomerMobile),
                Remark = NullIfEmpty(dto.Remark),
                StaffId = dto.StaffId,
                CouponCode = NullIfEmpty(dto.CouponCode),
                CouponDiscount = totals.CouponDiscount,
                IdempotencyKey = dto.IdempotencyKey?.Trim(),
                Status = TransactionStatus.Paid.ToApiValue(),
                PaidAt = now,
                TotalAmount = totals.Total,
                TaxAmount = totals.TaxAmount,
                TaxPercentage = totals.TaxPercentage,
                EditCount = 0,
                EditableUntil = now.Add(EditWindow)
            };

            foreach (var line in lines)
            {
                line.AccountId = dto.AccountId;
                line.CreatedBy = dto.UserId;
                line.CreatedAt = now;
                transaction.TransactionDetails.Add(line);
            }

            var inserted = await _transactionRepository.InsertAsync(transaction);
            var loaded = await _transactionRepository.GetDetailsAsync(inserted.TransactionId, dto.AccountId)
                ?? inserted;
            return MapSaved(loaded);
        }

        public async Task<TransactionSavedDto> UpdateAsync(string id, SaveTransactionDto dto)
        {
            ValidateWrite(dto, requireIdempotency: false);
            var existing = await FindAsync(id, dto.AccountId)
                ?? throw new KeyNotFoundException(ApiMessages.TransactionNotFound);

            if (!CanEdit(existing))
                throw new InvalidOperationException(EditWindowClosedCode);

            await EnsureBranchAsync(dto.BranchId, dto.AccountId);
            var lines = await BuildLinesAsync(dto);
            var totals = ComputeTotals(lines, dto.CouponCode);
            var now = DateTime.UtcNow;

            existing.Type = TransactionKinds.ParseOrThrow(dto.Type, ApiMessages.TransactionTypeInvalid).ToApiValue();
            existing.BranchId = dto.BranchId;
            existing.PaymentMode = PaymentModes.ParseOrThrow(dto.PaymentMode, ApiMessages.TransactionPaymentModeInvalid).ToApiValue();
            existing.CustomerName = NullIfEmpty(dto.CustomerName);
            existing.CustomerMobile = NullIfEmpty(dto.CustomerMobile);
            existing.Remark = NullIfEmpty(dto.Remark);
            existing.StaffId = dto.StaffId;
            existing.CouponCode = NullIfEmpty(dto.CouponCode);
            existing.CouponDiscount = totals.CouponDiscount;
            existing.TotalAmount = totals.Total;
            existing.TaxAmount = totals.TaxAmount;
            existing.TaxPercentage = totals.TaxPercentage;
            existing.EditCount += 1;
            existing.LastEditedBy = dto.EditorName;
            existing.LastEditedAt = now;

            await _transactionRepository.ReplaceDetailsAsync(existing.TransactionId, lines.Select(line =>
            {
                line.AccountId = dto.AccountId;
                line.CreatedBy = dto.UserId;
                line.CreatedAt = now;
                return line;
            }));
            await _transactionRepository.UpdateAsync(existing);

            var loaded = await _transactionRepository.GetDetailsAsync(existing.TransactionId, dto.AccountId)
                ?? existing;
            return MapSaved(loaded);
        }

        public async Task<TransactionSavedDto> MarkPaidAsync(string id, Guid accountId)
        {
            var existing = await FindAsync(id, accountId)
                ?? throw new KeyNotFoundException(ApiMessages.TransactionNotFound);

            existing.Status = TransactionStatus.Paid.ToApiValue();
            existing.PaidAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(existing);
            return MapSaved(existing);
        }

        public async Task<TransactionListDto> GetListAsync(Guid accountId)
        {
            var transactions = await _transactionRepository.GetListByAccountAsync(accountId);
            var services = await _servicesRepository.GetByAccountIdAsync(accountId);
            var staff = await _staffRepository.GetAllAsync(accountId);
            var branches = (await _branchRepository.GetAllAsync(accountId)).ToList();

            return new TransactionListDto
            {
                FeatureLock = new List<string>(),
                Filters = new TransactionListFiltersDto
                {
                    Branches = branches.Select(b => new TransactionNamedItemDto { Id = b.BranchId, Name = b.Name }).ToList(),
                    Services = services.Select(s => new TransactionNamedItemDto { Id = s.ServiceId, Name = s.ServiceName }).ToList(),
                    Staff = staff.Select(s => new TransactionNamedItemDto { Id = s.StaffId, Name = s.FullName }).ToList(),
                    Statuses = TransactionStatuses.ListFilterApiValues.ToList(),
                    Types = TransactionKinds.AllApiValues.ToList(),
                    PaymentModes = EnumText.AllApiValues<PaymentMode>().ToList(),
                    Periods = TransactionListPeriods.AllApiValues.ToList(),
                    Currency = CurrencyCode.Inr.ToApiValue()
                },
                Transactions = transactions.Select(MapListItem).ToList()
            };
        }

        public async Task<TransactionRecordDto?> GetDetailsAsync(string id, Guid accountId)
        {
            var transaction = await FindAsync(id, accountId);
            return transaction == null ? null : MapDetail(transaction);
        }

        private async Task<Transaction?> FindAsync(string id, Guid accountId)
        {
            if (Guid.TryParse(id, out var guid))
                return await _transactionRepository.GetDetailsAsync(guid, accountId);
            return await _transactionRepository.GetByCodeAsync(id, accountId);
        }

        private async Task EnsureBranchAsync(Guid branchId, Guid accountId)
        {
            var branch = await _branchRepository.GetByIdAsync(branchId);
            if (branch == null || branch.AccountId != accountId)
                throw new ArgumentException(ApiMessages.TransactionBranchInvalid);
        }

        private async Task<List<TransactionDetail>> BuildLinesAsync(SaveTransactionDto dto)
        {
            var isExpense = TransactionKinds.ParseOrThrow(dto.Type, ApiMessages.TransactionTypeInvalid) == TransactionKind.Expense;
            var lines = new List<TransactionDetail>();

            foreach (var item in dto.Services)
            {
                if (!item.ServiceId.HasValue || item.ServiceId == Guid.Empty)
                    throw new ArgumentException(ApiMessages.TransactionLineServiceRequired);
                if (item.Quantity <= 0)
                    throw new ArgumentException(ApiMessages.TransactionQuantityInvalid);

                if (isExpense)
                {
                    var expense = await _expensesTypeRepository.GetByIdAsync(item.ServiceId.Value, dto.AccountId);
                    if (expense == null)
                        throw new ArgumentException(ApiMessages.TransactionInvalidExpenseIds);

                    lines.Add(new TransactionDetail
                    {
                        ExpensesTypeId = expense.ExpensesTypeId,
                        Title = expense.ExpensesTypeName,
                        Quantity = item.Quantity,
                        StaffId = item.StaffId ?? dto.StaffId,
                        Amount = 0,
                        BaseAmount = 0,
                        GrossAmount = 0,
                        NetAmount = 0
                    });
                    continue;
                }

                var service = await _servicesRepository.GetByIdAsync(item.ServiceId.Value, dto.AccountId);
                if (service == null)
                    throw new ArgumentException(ApiMessages.TransactionInvalidServiceIds);

                var lineTotal = service.ServicePrice * item.Quantity;
                lines.Add(new TransactionDetail
                {
                    ServiceId = service.ServiceId,
                    Title = service.ServiceName,
                    Quantity = item.Quantity,
                    StaffId = item.StaffId ?? dto.StaffId,
                    Amount = lineTotal,
                    BaseAmount = service.ServicePrice,
                    GrossAmount = lineTotal,
                    NetAmount = lineTotal
                });
            }

            return lines;
        }

        private static (decimal Total, decimal TaxAmount, decimal TaxPercentage, decimal CouponDiscount) ComputeTotals(
            List<TransactionDetail> lines,
            string? couponCode)
        {
            var subtotal = lines.Sum(l => l.GrossAmount);
            return (subtotal, 0, 0, 0);
        }

        private async Task<string> NextCodeAsync(Guid accountId)
        {
            var count = await _transactionRepository.CountByAccountAsync(accountId);
            return $"TXN{count + 1}";
        }

        private static void ValidateWrite(SaveTransactionDto dto, bool requireIdempotency)
        {
            if (requireIdempotency && string.IsNullOrWhiteSpace(dto.IdempotencyKey))
                throw new ArgumentException(ApiMessages.TransactionIdempotencyRequired);
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException(ApiMessages.TransactionTypeRequired);
            _ = TransactionKinds.ParseOrThrow(dto.Type, ApiMessages.TransactionTypeInvalid);
            if (dto.BranchId == Guid.Empty)
                throw new ArgumentException(ApiMessages.TransactionBranchRequired);
            if (string.IsNullOrWhiteSpace(dto.PaymentMode))
                throw new ArgumentException(ApiMessages.TransactionPaymentModeRequired);
            _ = PaymentModes.ParseOrThrow(dto.PaymentMode, ApiMessages.TransactionPaymentModeInvalid);
            if (dto.Services == null || dto.Services.Count == 0)
                throw new ArgumentException(ApiMessages.TransactionServicesRequired);
        }

        private static bool CanEdit(Transaction transaction)
        {
            if (!transaction.EditableUntil.HasValue)
                return false;
            return DateTime.UtcNow <= transaction.EditableUntil.Value;
        }

        private static TransactionSavedDto MapSaved(Transaction transaction)
        {
            return new TransactionSavedDto
            {
                Id = transaction.Code ?? transaction.TransactionId.ToString(),
                Status = transaction.Status,
                GrandTotal = transaction.TotalAmount,
                CanEdit = CanEdit(transaction),
                EditCount = transaction.EditCount,
                CustomerName = transaction.CustomerName,
                CustomerMobile = transaction.CustomerMobile,
                EditableUntil = transaction.EditableUntil,
                LastEditedBy = transaction.LastEditedBy,
                LastEditedAt = transaction.LastEditedAt,
                PaidAt = transaction.PaidAt
            };
        }

        private static TransactionListItemDto MapListItem(Transaction transaction)
        {
            var first = transaction.TransactionDetails.FirstOrDefault();
            var serviceName = first?.Title
                ?? first?.Service?.ServiceName
                ?? first?.ExpensesType?.ExpensesTypeName
                ?? string.Empty;
            var serviceId = first?.ServiceId ?? first?.ExpensesTypeId;
            var customer = transaction.CustomerName ?? string.Empty;
            var title = string.IsNullOrWhiteSpace(customer)
                ? serviceName
                : $"{serviceName} - {customer}";

            return new TransactionListItemDto
            {
                Id = transaction.Code ?? transaction.TransactionId.ToString(),
                Title = title,
                Branch = transaction.Branch?.Name ?? string.Empty,
                BranchId = transaction.BranchId,
                Service = serviceName,
                ServiceId = serviceId,
                Staff = transaction.Staff?.FullName ?? first?.Staff?.FullName ?? string.Empty,
                StaffId = transaction.StaffId ?? first?.StaffId,
                Status = transaction.Status,
                Type = transaction.Type ?? string.Empty,
                Amount = transaction.TotalAmount,
                PaymentMode = transaction.PaymentMode ?? string.Empty,
                CustomerName = customer,
                CustomerId = null,
                CreatedAt = transaction.CreatedAt
            };
        }

        private static TransactionRecordDto MapDetail(Transaction transaction)
        {
            var lines = transaction.TransactionDetails.Select(d => new TransactionLineBreakdownDto
            {
                Id = d.ServiceId ?? d.ExpensesTypeId ?? d.TransactionDetailsId,
                Title = d.Title ?? d.Service?.ServiceName ?? d.ExpensesType?.ExpensesTypeName ?? string.Empty,
                Quantity = d.Quantity,
                BaseAmount = d.BaseAmount,
                TaxPercentage = d.TaxPercentage,
                TaxAmount = d.TaxAmount,
                DiscountPercentage = d.DiscountPercentage,
                DiscountAmount = d.DiscountAmount,
                GrossAmount = d.GrossAmount,
                NetAmount = d.NetAmount
            }).ToList();

            var subtotal = lines.Sum(l => l.GrossAmount);
            TransactionCouponDto? coupon = null;
            if (!string.IsNullOrWhiteSpace(transaction.CouponCode))
            {
                coupon = new TransactionCouponDto
                {
                    Code = transaction.CouponCode,
                    Type = transaction.CouponType ?? CommissionType.Percentage.ToApiValue(),
                    Value = transaction.CouponValue ?? 0,
                    DiscountAmount = transaction.CouponDiscount
                };
            }

            return new TransactionRecordDto
            {
                Id = transaction.Code ?? transaction.TransactionId.ToString(),
                Status = transaction.Status,
                PaymentMode = transaction.PaymentMode ?? string.Empty,
                Type = transaction.Type ?? string.Empty,
                Category = transaction.Type ?? string.Empty,
                PriceBreakdown = new TransactionPriceBreakdownDto
                {
                    Services = lines,
                    Coupon = coupon,
                    Summary = new TransactionSummaryDto
                    {
                        Subtotal = subtotal,
                        TaxPercentage = transaction.TaxPercentage,
                        TaxAmount = transaction.TaxAmount,
                        CouponDiscount = transaction.CouponDiscount,
                        Total = transaction.TotalAmount,
                        Currency = "INR"
                    }
                },
                DateTime = new TransactionDateTimeDto
                {
                    Iso = ToIst(transaction.CreatedAt),
                    Display = FormatDisplay(transaction.CreatedAt)
                },
                Branch = transaction.Branch == null
                    ? null
                    : new TransactionBranchInfoDto
                    {
                        Id = transaction.Branch.BranchId,
                        Name = transaction.Branch.Name,
                        Location = transaction.Branch.City
                    },
                Staff = transaction.Staff == null && !transaction.StaffId.HasValue
                    ? null
                    : new TransactionNamedItemDto
                    {
                        Id = transaction.Staff?.StaffId ?? transaction.StaffId ?? Guid.Empty,
                        Name = transaction.Staff?.FullName ?? string.Empty
                    },
                Remark = transaction.Remark,
                CanEdit = CanEdit(transaction),
                EditCount = transaction.EditCount,
                FeatureLock = new List<string>(),
                EditableUntil = transaction.EditableUntil,
                LastEditedBy = transaction.LastEditedBy,
                LastEditedAt = transaction.LastEditedAt
            };
        }

        private static string MapRole(IReadOnlyList<string> roles)
        {
            var raw = roles.FirstOrDefault(r => !string.IsNullOrWhiteSpace(r)) ?? string.Empty;
            return raw.Trim().ToLowerInvariant() switch
            {
                "admin" => "account_admin",
                "manager" => "manager",
                "employee" => "employee",
                _ => string.IsNullOrWhiteSpace(raw) ? "employee" : raw.Trim().ToLowerInvariant()
            };
        }

        private static string? NullIfEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static TimeZoneInfo IstZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
        }

        private static DateTime ToIst(DateTime utc)
        {
            var value = utc.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(utc, DateTimeKind.Utc)
                : utc.ToUniversalTime();
            return TimeZoneInfo.ConvertTimeFromUtc(value, IstZone());
        }

        private static string FormatDisplay(DateTime utc)
        {
            return ToIst(utc).ToString("d MMM yyyy, h:mm tt", CultureInfo.InvariantCulture);
        }
    }
}
