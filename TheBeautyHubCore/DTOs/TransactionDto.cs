using System;
using System.Collections.Generic;

namespace TheBeautyHubCore.DTOs
{
    public class TransactionNamedItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class TransactionBootstrapServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Frequent { get; set; }
    }

    public class TransactionBootstrapDto
    {
        public List<TransactionBootstrapServiceDto> Services { get; set; } = new();
        public List<TransactionNamedItemDto> Expenses { get; set; } = new();
        public List<TransactionNamedItemDto> Staff { get; set; } = new();
        public List<TransactionNamedItemDto> Branches { get; set; } = new();
        public string UserRole { get; set; } = string.Empty;
        public Guid LoggedInUserId { get; set; }
        public Guid? LoggedInBranchId { get; set; }
        public string? LastPaymentMode { get; set; }
        public string? LastTransactionType { get; set; }
    }

    public class SaveTransactionLineDto
    {
        public Guid? ServiceId { get; set; }
        public int Quantity { get; set; } = 1;
        public Guid? StaffId { get; set; }
    }

    public class SaveTransactionDto
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public string? EditorName { get; set; }
        public string? IdempotencyKey { get; set; }
        public string Type { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public List<SaveTransactionLineDto> Services { get; set; } = new();
        public string? CustomerName { get; set; }
        public string? CustomerMobile { get; set; }
        public string? Remark { get; set; }
        public Guid? StaffId { get; set; }
        public string? CouponCode { get; set; }
    }

    public class TransactionSavedDto
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal GrandTotal { get; set; }
        public bool CanEdit { get; set; }
        public int EditCount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerMobile { get; set; }
        public DateTime? EditableUntil { get; set; }
        public string? LastEditedBy { get; set; }
        public DateTime? LastEditedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class TransactionListItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public Guid? BranchId { get; set; }
        public string Service { get; set; } = string.Empty;
        public Guid? ServiceId { get; set; }
        public string Staff { get; set; } = string.Empty;
        public Guid? StaffId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Guid? CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TransactionListFiltersDto
    {
        public List<TransactionNamedItemDto> Branches { get; set; } = new();
        public List<TransactionNamedItemDto> Services { get; set; } = new();
        public List<TransactionNamedItemDto> Staff { get; set; } = new();
        public List<string> Statuses { get; set; } = new();
        public List<string> Types { get; set; } = new();
        public List<string> PaymentModes { get; set; } = new();
        public List<string> Periods { get; set; } = new();
        public string Currency { get; set; } = "INR";
    }

    public class TransactionListDto
    {
        public List<string> FeatureLock { get; set; } = new();
        public TransactionListFiltersDto Filters { get; set; } = new();
        public List<TransactionListItemDto> Transactions { get; set; } = new();
    }

    public class TransactionLineBreakdownDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class TransactionCouponDto
    {
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class TransactionSummaryDto
    {
        public decimal Subtotal { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal CouponDiscount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; } = "INR";
    }

    public class TransactionPriceBreakdownDto
    {
        public List<TransactionLineBreakdownDto> Services { get; set; } = new();
        public TransactionCouponDto? Coupon { get; set; }
        public TransactionSummaryDto Summary { get; set; } = new();
    }

    public class TransactionDateTimeDto
    {
        public DateTime Iso { get; set; }
        public string Display { get; set; } = string.Empty;
    }

    public class TransactionBranchInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class TransactionRecordDto
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public TransactionPriceBreakdownDto PriceBreakdown { get; set; } = new();
        public TransactionDateTimeDto DateTime { get; set; } = new();
        public TransactionBranchInfoDto? Branch { get; set; }
        public TransactionNamedItemDto? Staff { get; set; }
        public string? Remark { get; set; }
        public bool CanEdit { get; set; }
        public int EditCount { get; set; }
        public List<string> FeatureLock { get; set; } = new();
        public DateTime? EditableUntil { get; set; }
        public string? LastEditedBy { get; set; }
        public DateTime? LastEditedAt { get; set; }
    }
}
