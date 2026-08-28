using AutoMapper;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Entities;

namespace TheBeautyHubAPI
{
    /// <summary>
    /// AutoMapper profile for mapping between API models and DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Account mappings
            CreateMap<CreateAccountRequest, CreateAccountDto>();
            CreateMap<UpdateAccountRequest, UpdateAccountDto>();
            CreateMap<AccountDto, AccountResponse>();
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountDto, Account>();
            CreateMap<UpdateAccountDto, Account>();

            // User mappings
            CreateMap<CreateUserRequest, CreateUserDto>();
            CreateMap<UpdateUserRequest, UpdateUserDto>();
            CreateMap<UpdateUserPasswordRequest, UpdateUserPasswordDto>();
            CreateMap<UserDto, UserResponse>();
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Firm mappings
            CreateMap<CreateFirmRequest, CreateFirmDto>();
            CreateMap<UpdateFirmRequest, UpdateFirmDto>();
            CreateMap<FirmDto, FirmResponse>();
            CreateMap<Firm, FirmDto>();
            CreateMap<CreateFirmDto, Firm>();
            CreateMap<UpdateFirmDto, Firm>();

            // FirmDetails mappings
            CreateMap<CreateFirmDetailsRequest, CreateFirmDetailsDto>();
            CreateMap<UpdateFirmDetailsRequest, UpdateFirmDetailsDto>();
            CreateMap<FirmDetailsDto, FirmDetailsResponse>();
            CreateMap<FirmDetails, FirmDetailsDto>();
            CreateMap<CreateFirmDetailsDto, FirmDetails>();
            CreateMap<UpdateFirmDetailsDto, FirmDetails>();

            // Plans mappings
            CreateMap<CreatePlanRequest, CreatePlanDto>();
            CreateMap<UpdatePlanRequest, UpdatePlanDto>();
            CreateMap<PlansDto, PlanResponse>();
            CreateMap<Plans, PlansDto>();
            CreateMap<CreatePlanDto, Plans>();
            CreateMap<UpdatePlanDto, Plans>();

            // Subscription mappings
            CreateMap<CreateSubscriptionRequest, CreateSubscriptionDto>();
            CreateMap<UpdateSubscriptionRequest, UpdateSubscriptionDto>();
            CreateMap<SubscriptionDto, SubscriptionResponse>();
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<CreateSubscriptionDto, Subscription>();
            CreateMap<UpdateSubscriptionDto, Subscription>();

            // Wallet mappings
            CreateMap<CreateWalletRequest, CreateWalletDto>();
            CreateMap<UpdateWalletRequest, UpdateWalletDto>();
            CreateMap<WalletDto, WalletResponse>();
            CreateMap<Wallet, WalletDto>();
            CreateMap<CreateWalletDto, Wallet>();
            CreateMap<UpdateWalletDto, Wallet>();

            // ExpensesType mappings
            CreateMap<CreateExpensesTypeRequest, CreateExpensesTypeDto>();
            CreateMap<UpdateExpensesTypeRequest, UpdateExpensesTypeDto>();
            CreateMap<ExpensesTypeDto, ExpensesTypeResponse>();
            CreateMap<ExpensesType, ExpensesTypeDto>();
            CreateMap<CreateExpensesTypeDto, ExpensesType>();
            CreateMap<UpdateExpensesTypeDto, ExpensesType>();

            // Services mappings
            CreateMap<CreateServicesRequest, CreateServicesDto>();
            CreateMap<UpdateServicesRequest, UpdateServicesDto>();
            CreateMap<ServicesDto, ServicesResponse>();
            CreateMap<Services, ServicesDto>();
            CreateMap<CreateServicesDto, Services>();
            CreateMap<UpdateServicesDto, Services>();

            // TransactionType mappings
            CreateMap<CreateTransactionTypeRequest, CreateTransactionTypeDto>();
            CreateMap<UpdateTransactionTypeRequest, UpdateTransactionTypeDto>();
            CreateMap<TransactionTypeDto, TransactionTypeResponse>();
            CreateMap<TransactionType, TransactionTypeDto>();
            CreateMap<CreateTransactionTypeDto, TransactionType>();
            CreateMap<UpdateTransactionTypeDto, TransactionType>();

            // TransactionRules mappings
            CreateMap<CreateTransactionRulesRequest, CreateTransactionRulesDto>();
            CreateMap<UpdateTransactionRulesRequest, UpdateTransactionRulesDto>();
            CreateMap<TransactionRulesDto, TransactionRulesResponse>();
            CreateMap<TransactionRules, TransactionRulesDto>();
            CreateMap<CreateTransactionRulesDto, TransactionRules>();
            CreateMap<UpdateTransactionRulesDto, TransactionRules>();

            // Transaction mappings
            CreateMap<CreateTransactionRequest, CreateTransactionDto>();
            CreateMap<UpdateTransactionRequest, UpdateTransactionDto>();
            CreateMap<TransactionDto, TransactionResponse>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>();
            CreateMap<UpdateTransactionDto, Transaction>();

            // TransactionDetail mappings
            CreateMap<CreateTransactionDetailRequest, CreateTransactionDetailDto>();
            CreateMap<UpdateTransactionDetailRequest, UpdateTransactionDetailDto>();
            CreateMap<TransactionDetailDto, TransactionDetailResponse>();
            CreateMap<TransactionDetail, TransactionDetailDto>();
            CreateMap<CreateTransactionDetailDto, TransactionDetail>();
            CreateMap<UpdateTransactionDetailDto, TransactionDetail>();

            // Report mappings
            CreateMap<CreateReportRequest, CreateReportDto>();
            CreateMap<UpdateReportRequest, UpdateReportDto>();
            CreateMap<ReportDto, ReportResponse>();
            CreateMap<Report, ReportDto>();
            CreateMap<CreateReportDto, Report>();
            CreateMap<UpdateReportDto, Report>();

            // ReportForAccount mappings
            CreateMap<CreateReportForAccountRequest, CreateReportForAccountDto>();
            CreateMap<UpdateReportForAccountRequest, UpdateReportForAccountDto>();
            CreateMap<ReportForAccountDto, ReportForAccountResponse>();
            CreateMap<ReportForAccount, ReportForAccountDto>();
            CreateMap<CreateReportForAccountDto, ReportForAccount>();
            CreateMap<UpdateReportForAccountDto, ReportForAccount>();

            // Partner mappings
            CreateMap<CreatePartnerRequest, CreatePartnerDto>();
            CreateMap<UpdatePartnerRequest, UpdatePartnerDto>();
            CreateMap<PartnerDto, PartnerResponse>();
            CreateMap<Partner, PartnerDto>();
            CreateMap<CreatePartnerDto, Partner>();
            CreateMap<UpdatePartnerDto, Partner>();

            // UserSession mappings
            CreateMap<CreateUserSessionRequest, CreateUserSessionDto>();
            CreateMap<UpdateUserSessionRequest, UpdateUserSessionDto>();
            CreateMap<UserSessionDto, UserSessionResponse>();
            CreateMap<UserSession, UserSessionDto>();
            CreateMap<CreateUserSessionDto, UserSession>();
            CreateMap<UpdateUserSessionDto, UserSession>();

            // ExceptionLog mappings
            CreateMap<CreateExceptionLogRequest, CreateExceptionLogDto>();
            CreateMap<ExceptionLogDto, ExceptionLogResponse>();
            CreateMap<ExceptionLog, ExceptionLogDto>();
            CreateMap<CreateExceptionLogDto, ExceptionLog>();

            // Branch mappings
            CreateMap<BranchListItemDto, BranchListItemResponse>();
            CreateMap<BranchDetailDto, BranchDetailResponse>();
            CreateMap<BranchServiceItemDto, BranchServiceItemResponse>();
            CreateMap<BranchEmployeeItemDto, BranchEmployeeItemResponse>();

            CreateMap<StaffFormConfigDto, StaffFormConfigDataResponse>();
            CreateMap<StaffFormBranchDto, StaffFormBranchResponse>();
            CreateMap<StaffFormSalaryRuleDto, StaffFormSalaryRuleResponse>();
            CreateMap<StaffListItemDto, StaffListItemResponse>();
            CreateMap<StaffDetailDto, StaffDetailResponse>();
        }
    }
}
