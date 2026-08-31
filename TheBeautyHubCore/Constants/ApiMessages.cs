namespace TheBeautyHubCore.Constants;

/// <summary>
/// Central catalogue of user-facing API messages.
/// Nested types must not reuse the same field names for different values — C# inlines
/// identically named constants and the last definition wins at call sites.
/// </summary>
public static class ApiMessages
{
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    public const string Unauthorized = "You are not authorized to access this resource.";
    public const string Forbidden = "You do not have permission to perform this action.";
    public const string InvalidToken = "The access token is invalid or has expired. Please sign in again.";
    public const string MissingToken = "Authorization is required. Send the AuthCenter access token as a Bearer token.";
    public const string TokenNotValidForApp = "This access token is not valid for The Beauty Hub.";
    public const string AuthenticationFailed = "Authentication failed. Please sign in again.";
    public const string AccountRequired = "Your account could not be determined from the signed-in session. Please sign in again.";
    public const string ValidationOccurred = "One or more fields are invalid. Please review your input and try again.";
    public const string RequestBodyRequired = "A request body is required. Please send the form or JSON payload and try again.";
    public const string InvalidRequestBody = "The request body could not be read. Please check the payload and try again.";
    public const string InvalidBranchIds = "One or more selected branches were not found. Please choose valid branches and try again.";
    public const string InvalidServiceIds = "One or more selected services were not found. Please choose valid services and try again.";
    public const string BranchesRequiredWhenNotAll = "Please select at least one branch when this record is not available for all branches.";
    public const string FileTooLarge = "The uploaded file must be greater than 0 bytes and no larger than 5 MB.";

    public static string InvalidImageType(string label)
        => $"The {label} must be a JPG, JPEG, PNG, WEBP, or GIF file.";

    public static string InvalidImageOrPdfType(string label)
        => $"The {label} must be a JPG, JPEG, PNG, WEBP, GIF, or PDF file.";

    public static string FileTooLargeFor(string label)
        => $"The {label} must be greater than 0 bytes and no larger than 5 MB.";

    public const string AuthTokenValid = "The access token is valid.";
    public const string AuthTokenInvalid = "The access token could not be validated.";

    public const string AccountSummaryFetched = "Account summary fetched successfully";
    public const string AccountSummaryFailed = "We could not load the account summary right now. Please try again later.";
    public const string FeatureLockFetched = "Feature lock fetched successfully";
    public const string FeatureLockFailed = "We could not load feature lock right now. Please try again later.";

    public const string BranchListFetched = "Branches were fetched successfully.";
    public const string BranchDetailsFetched = "Branch details were fetched successfully.";
    public const string BranchCreated = "The branch was created successfully.";
    public const string BranchUpdated = "The branch was updated successfully.";
    public const string BranchListFailed = "We could not load branches right now. Please try again later.";
    public const string BranchDetailsFailed = "We could not load branch details right now. Please try again later.";
    public const string BranchCreateFailed = "We could not create the branch. Please try again later.";
    public const string BranchUpdateFailed = "We could not update the branch. Please try again later.";
    public const string BranchNotFound = "The requested branch was not found.";
    public const string BranchNameRequired = "Please enter a branch name.";
    public const string BranchAddressRequired = "Please enter address line 1.";
    public const string BranchCityRequired = "Please enter a city.";
    public const string BranchStateRequired = "Please enter a state.";
    public const string BranchPincodeRequired = "Please enter a pincode.";
    public const string BranchMobileRequired = "Please enter a mobile number.";
    public const string BranchEmailRequired = "Please enter an email address.";
    public const string BranchTypeRequired = "Please select a branch type.";
        public const string BranchOpeningTimeRequired = "Please enter the opening time.";
        public const string BranchClosingTimeRequired = "Please enter the closing time.";
        public const string BranchTimeTooLong = "Opening and closing times must be 10 characters or fewer (for example 09:00).";
    public const string BranchWeeklyOffRequired = "Please select a weekly off day.";
    public const string BranchStatusRequired = "Please select a branch status.";
    public const string BranchStatusInvalid = "Branch status must be active or inactive.";
    public const string RecordStatusInvalid = "Status must be active or inactive.";

    public const string ServiceCatalogFetched = "The services catalog was fetched successfully.";
    public const string ServiceListFetched = "Services were fetched successfully.";
    public const string ServiceDetailsFetched = "Service details were fetched successfully.";
    public const string ServiceCreated = "The service was created successfully.";
    public const string ServiceUpdated = "The service was updated successfully.";
    public const string ServiceDeleted = "The service was deleted successfully.";
    public const string ServiceCatalogFailed = "We could not load the services catalog right now. Please try again later.";
    public const string ServiceListFailed = "We could not load services right now. Please try again later.";
    public const string ServiceDetailsFailed = "We could not load service details right now. Please try again later.";
    public const string ServiceCreateFailed = "We could not create the service. Please try again later.";
    public const string ServiceUpdateFailed = "We could not update the service. Please try again later.";
    public const string ServiceDeleteFailed = "We could not delete the service. Please try again later.";
    public const string ServiceNotFound = "The requested service was not found.";
    public const string ServiceNameRequired = "Please enter a service name.";
    public const string ServiceDescriptionRequired = "Please enter a service description.";
    public const string ServiceCategoryRequired = "Please select a service category.";
    public const string ServiceDurationInvalid = "Duration must be zero or greater.";
    public const string ServiceGenderRequired = "Please select the applicable gender.";
    public const string ServiceGenderInvalid = "Applicable gender must be unisex, male, or female.";
    public const string ServiceTypeInvalid = "Service type must be Service, in_salon, or home.";
    public const string PartnerGenderInvalid = "Gender must be Male, Female, or Other.";
    public const string StaffGenderInvalid = "Gender must be Male, Female, or Other.";
    public const string ServiceTypeRequired = "Please select a service type.";
    public const string ServiceStatusRequired = "Please select a service status.";
    public const string ServiceCustomerPriceInvalid = "Customer price must be zero or greater.";
    public const string ServiceMaterialCostInvalid = "Material cost must be zero or greater.";
    public const string ServiceCommissionTypeRequired = "Please select a commission type.";
    public const string ServiceCommissionTypeInvalid = "Commission type must be Fixed Amount or Percentage.";
    public const string ServiceCommissionPercentageInvalid = "Commission value must be between 0 and 100 when the type is Percentage.";
    public const string ServiceCommissionValueInvalid = "Commission value must be zero or greater.";
    public const string ServiceOtherCostInvalid = "Other cost must be zero or greater.";

    public const string StaffFormConfigFetched = "Staff form configuration was fetched successfully.";
    public const string StaffListFetched = "Staff were fetched successfully.";
    public const string StaffDetailsFetched = "Staff details were fetched successfully.";
    public const string StaffNextCodeFetched = "The next employee code was generated successfully.";
    public const string StaffCreated = "The staff member was created successfully.";
    public const string StaffUpdated = "The staff member was updated successfully.";
    public const string StaffDeleted = "The staff member was deleted successfully.";
    public const string StaffFormConfigFailed = "We could not load staff form configuration right now. Please try again later.";
    public const string StaffListFailed = "We could not load staff right now. Please try again later.";
    public const string StaffDetailsFailed = "We could not load staff details right now. Please try again later.";
    public const string StaffCreateFailed = "We could not create the staff member. Please try again later.";
    public const string StaffUpdateFailed = "We could not update the staff member. Please try again later.";
    public const string StaffDeleteFailed = "We could not delete the staff member. Please try again later.";
    public const string StaffNotFound = "The requested staff member was not found.";
    public const string StaffEmployeeCodeExists = "This employee code is already in use. Please choose a different code.";
    public const string StaffFullNameRequired = "Please enter the staff member's full name.";
    public const string StaffMobileRequired = "Please enter a mobile number.";
    public const string StaffEmailRequired = "Please enter an email address.";
    public const string StaffGenderRequired = "Please select a gender.";
    public const string StaffAadhaarRequired = "Please enter an Aadhaar number.";
    public const string StaffDesignationRequired = "Please enter a designation.";
    public const string StaffSpecialistRequired = "Please enter a specialist role.";
    public const string StaffBranchRequired = "Please select a branch.";
    public const string StaffBranchInvalid = "The selected branch was not found. Please choose a valid branch.";
    public const string StaffSalaryRuleRequired = "Please select a salary rule.";
    public const string StaffSalaryRuleInvalid = "The selected salary rule was not found. Please choose a valid salary rule.";
    public const string StaffStatusRequired = "Please select a staff status.";
    public const string StaffAppRoleRequired = "Please select an app role when app login is enabled.";
    public const string StaffUsernameRequired = "Please enter a username when app login is enabled.";
    public const string StaffAuthCenterUserRequired = "This staff member must already exist in AuthCenter so we can store their user id. Sign them up there first, or use the same email as the signed-in user.";
    public const string StaffJoiningDateInvalid = "Please enter a valid joining date.";

    public const string SalaryRuleCatalogFetched = "The salary rules catalog was fetched successfully.";
    public const string SalaryRuleListFetched = "Salary rules were fetched successfully.";
    public const string SalaryRuleDetailsFetched = "Salary rule details were fetched successfully.";
    public const string SalaryRuleCreated = "The salary rule was created successfully.";
    public const string SalaryRuleUpdated = "The salary rule was updated successfully.";
    public const string SalaryRuleDeleted = "The salary rule was deleted successfully.";
    public const string SalaryRuleCatalogFailed = "We could not load salary rules right now. Please try again later.";
    public const string SalaryRuleListFailed = "We could not load salary rules right now. Please try again later.";
    public const string SalaryRuleDetailsFailed = "We could not load salary rule details right now. Please try again later.";
    public const string SalaryRuleCreateFailed = "We could not create the salary rule. Please try again later.";
    public const string SalaryRuleUpdateFailed = "We could not update the salary rule. Please try again later.";
    public const string SalaryRuleDeleteFailed = "We could not delete the salary rule. Please try again later.";
    public const string SalaryRuleNotFound = "The requested salary rule was not found.";
    public const string SalaryRuleNameRequired = "Please enter a salary rule name.";
    public const string SalaryRuleDescriptionRequired = "Please enter a salary rule description.";
    public const string SalaryRuleTypeRequired = "Please select a salary type.";
    public const string SalaryRuleTypeInvalid = "Salary type must be Fixed Salary, Service Commission, or Hybrid.";
    public const string SalaryRuleStatusRequired = "Please select a salary rule status.";

    public const string ExpenseListFetched = "Expenses were fetched successfully.";
    public const string ExpenseDetailsFetched = "Expense details were fetched successfully.";
    public const string ExpenseCreated = "The expense was created successfully.";
    public const string ExpenseUpdated = "The expense was updated successfully.";
    public const string ExpenseDeleted = "The expense was deleted successfully.";
    public const string ExpenseListFailed = "We could not load expenses right now. Please try again later.";
    public const string ExpenseDetailsFailed = "We could not load expense details right now. Please try again later.";
    public const string ExpenseCreateFailed = "We could not create the expense. Please try again later.";
    public const string ExpenseUpdateFailed = "We could not update the expense. Please try again later.";
    public const string ExpenseDeleteFailed = "We could not delete the expense. Please try again later.";
    public const string ExpenseNotFound = "The requested expense was not found.";
    public const string ExpenseNameRequired = "Please enter an expense name.";
    public const string ExpenseDescriptionRequired = "Please enter an expense description.";
    public const string ExpenseStatusRequired = "Please select an expense status.";

    public const string TransactionBootstrapFetched = "Transaction entry data was fetched successfully.";
    public const string TransactionListFetched = "Transactions were fetched successfully.";
    public const string TransactionDetailsFetched = "Transaction details were fetched successfully.";
    public const string TransactionCreated = "The transaction was created successfully.";
    public const string TransactionUpdated = "The transaction was updated successfully.";
    public const string TransactionMarkedPaid = "The transaction was marked as paid successfully.";
    public const string TransactionBootstrapFailed = "We could not load transaction entry data right now. Please try again later.";
    public const string TransactionListFailed = "We could not load transactions right now. Please try again later.";
    public const string TransactionDetailsFailed = "We could not load transaction details right now. Please try again later.";
    public const string TransactionCreateFailed = "We could not create the transaction. Please try again later.";
    public const string TransactionUpdateFailed = "We could not update the transaction. Please try again later.";
    public const string TransactionMarkPaidFailed = "We could not mark the transaction as paid. Please try again later.";
    public const string TransactionNotFound = "The requested transaction was not found.";
    public const string TransactionEditWindowClosed = "This transaction can no longer be edited because the allowed edit window has closed.";
    public const string TransactionIdempotencyRequired = "An idempotency key is required so a retry does not create a duplicate transaction.";
    public const string TransactionTypeRequired = "Please select a transaction type.";
    public const string TransactionTypeInvalid = "Transaction type must be sale or expense.";
    public const string TransactionBranchRequired = "Please select a branch.";
    public const string TransactionBranchInvalid = "The selected branch was not found. Please choose a valid branch.";
    public const string TransactionPaymentModeRequired = "Please select a payment mode.";
    public const string TransactionPaymentModeInvalid = "Payment mode must be cash, upi, or card.";
    public const string TransactionServicesRequired = "Please add at least one service or expense line item.";
    public const string TransactionLineServiceRequired = "Each line item must include a valid service or expense.";
    public const string TransactionQuantityInvalid = "Quantity must be greater than zero.";
    public const string TransactionInvalidExpenseIds = "One or more selected expenses were not found. Please choose valid expenses and try again.";
    public const string TransactionInvalidServiceIds = "One or more selected services were not found. Please choose valid services and try again.";
}
