namespace TheBeautyHubCore.Constants;

/// <summary>
/// Central catalogue of user-facing API messages.
/// Keep wording here so responses stay consistent and can be updated in one place.
/// </summary>
public static class ApiMessages
{
    public static class Common
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
    }

    public static class Auth
    {
        public const string TokenValid = "The access token is valid.";
        public const string TokenInvalid = "The access token could not be validated.";
    }

    public static class Branch
    {
        public const string ListFetched = "Branches were fetched successfully.";
        public const string DetailsFetched = "Branch details were fetched successfully.";
        public const string Created = "The branch was created successfully.";
        public const string Updated = "The branch was updated successfully.";
        public const string ListFailed = "We could not load branches right now. Please try again later.";
        public const string DetailsFailed = "We could not load branch details right now. Please try again later.";
        public const string CreateFailed = "We could not create the branch. Please try again later.";
        public const string UpdateFailed = "We could not update the branch. Please try again later.";
        public const string NotFound = "The requested branch was not found.";
        public const string NameRequired = "Please enter a branch name.";
        public const string AddressRequired = "Please enter address line 1.";
        public const string CityRequired = "Please enter a city.";
        public const string StateRequired = "Please enter a state.";
        public const string PincodeRequired = "Please enter a pincode.";
        public const string MobileRequired = "Please enter a mobile number.";
        public const string EmailRequired = "Please enter an email address.";
        public const string TypeRequired = "Please select a branch type.";
        public const string OpeningTimeRequired = "Please enter the opening time.";
        public const string ClosingTimeRequired = "Please enter the closing time.";
        public const string WeeklyOffRequired = "Please select a weekly off day.";
        public const string StatusRequired = "Please select a branch status.";
    }

    public static class Service
    {
        public const string CatalogFetched = "The services catalog was fetched successfully.";
        public const string ListFetched = "Services were fetched successfully.";
        public const string DetailsFetched = "Service details were fetched successfully.";
        public const string Created = "The service was created successfully.";
        public const string Updated = "The service was updated successfully.";
        public const string Deleted = "The service was deleted successfully.";
        public const string CatalogFailed = "We could not load the services catalog right now. Please try again later.";
        public const string ListFailed = "We could not load services right now. Please try again later.";
        public const string DetailsFailed = "We could not load service details right now. Please try again later.";
        public const string CreateFailed = "We could not create the service. Please try again later.";
        public const string UpdateFailed = "We could not update the service. Please try again later.";
        public const string DeleteFailed = "We could not delete the service. Please try again later.";
        public const string NotFound = "The requested service was not found.";
        public const string NameRequired = "Please enter a service name.";
        public const string DescriptionRequired = "Please enter a service description.";
        public const string CategoryRequired = "Please select a service category.";
        public const string DurationInvalid = "Duration must be zero or greater.";
        public const string GenderRequired = "Please select the applicable gender.";
        public const string TypeRequired = "Please select a service type.";
        public const string StatusRequired = "Please select a service status.";
        public const string CustomerPriceInvalid = "Customer price must be zero or greater.";
        public const string MaterialCostInvalid = "Material cost must be zero or greater.";
        public const string CommissionTypeRequired = "Please select a commission type.";
        public const string CommissionTypeInvalid = "Commission type must be percentage or flat.";
        public const string CommissionPercentageInvalid = "Commission value must be between 0 and 100 when the type is percentage.";
        public const string CommissionValueInvalid = "Commission value must be zero or greater.";
        public const string OtherCostInvalid = "Other cost must be zero or greater.";
    }

    public static class Staff
    {
        public const string FormConfigFetched = "Staff form configuration was fetched successfully.";
        public const string ListFetched = "Staff were fetched successfully.";
        public const string DetailsFetched = "Staff details were fetched successfully.";
        public const string NextCodeFetched = "The next employee code was generated successfully.";
        public const string Created = "The staff member was created successfully.";
        public const string Updated = "The staff member was updated successfully.";
        public const string Deleted = "The staff member was deleted successfully.";
        public const string FormConfigFailed = "We could not load staff form configuration right now. Please try again later.";
        public const string ListFailed = "We could not load staff right now. Please try again later.";
        public const string DetailsFailed = "We could not load staff details right now. Please try again later.";
        public const string CreateFailed = "We could not create the staff member. Please try again later.";
        public const string UpdateFailed = "We could not update the staff member. Please try again later.";
        public const string DeleteFailed = "We could not delete the staff member. Please try again later.";
        public const string NotFound = "The requested staff member was not found.";
        public const string EmployeeCodeExists = "This employee code is already in use. Please choose a different code.";
        public const string FullNameRequired = "Please enter the staff member's full name.";
        public const string MobileRequired = "Please enter a mobile number.";
        public const string EmailRequired = "Please enter an email address.";
        public const string GenderRequired = "Please select a gender.";
        public const string AadhaarRequired = "Please enter an Aadhaar number.";
        public const string DesignationRequired = "Please enter a designation.";
        public const string SpecialistRequired = "Please enter a specialist role.";
        public const string BranchRequired = "Please select a branch.";
        public const string BranchInvalid = "The selected branch was not found. Please choose a valid branch.";
        public const string SalaryRuleRequired = "Please select a salary rule.";
        public const string SalaryRuleInvalid = "The selected salary rule was not found. Please choose a valid salary rule.";
        public const string StatusRequired = "Please select a staff status.";
        public const string AppRoleRequired = "Please select an app role when app login is enabled.";
        public const string UsernameRequired = "Please enter a username when app login is enabled.";
        public const string JoiningDateInvalid = "Please enter a valid joining date.";
    }

    public static class SalaryRule
    {
        public const string CatalogFetched = "The salary rules catalog was fetched successfully.";
        public const string ListFetched = "Salary rules were fetched successfully.";
        public const string DetailsFetched = "Salary rule details were fetched successfully.";
        public const string Created = "The salary rule was created successfully.";
        public const string Updated = "The salary rule was updated successfully.";
        public const string Deleted = "The salary rule was deleted successfully.";
        public const string CatalogFailed = "We could not load salary rules right now. Please try again later.";
        public const string ListFailed = "We could not load salary rules right now. Please try again later.";
        public const string DetailsFailed = "We could not load salary rule details right now. Please try again later.";
        public const string CreateFailed = "We could not create the salary rule. Please try again later.";
        public const string UpdateFailed = "We could not update the salary rule. Please try again later.";
        public const string DeleteFailed = "We could not delete the salary rule. Please try again later.";
        public const string NotFound = "The requested salary rule was not found.";
        public const string NameRequired = "Please enter a salary rule name.";
        public const string DescriptionRequired = "Please enter a salary rule description.";
        public const string TypeRequired = "Please select a salary type.";
        public const string StatusRequired = "Please select a salary rule status.";
    }

    public static class Expense
    {
        public const string ListFetched = "Expenses were fetched successfully.";
        public const string DetailsFetched = "Expense details were fetched successfully.";
        public const string Created = "The expense was created successfully.";
        public const string Updated = "The expense was updated successfully.";
        public const string Deleted = "The expense was deleted successfully.";
        public const string ListFailed = "We could not load expenses right now. Please try again later.";
        public const string DetailsFailed = "We could not load expense details right now. Please try again later.";
        public const string CreateFailed = "We could not create the expense. Please try again later.";
        public const string UpdateFailed = "We could not update the expense. Please try again later.";
        public const string DeleteFailed = "We could not delete the expense. Please try again later.";
        public const string NotFound = "The requested expense was not found.";
        public const string NameRequired = "Please enter an expense name.";
        public const string DescriptionRequired = "Please enter an expense description.";
        public const string StatusRequired = "Please select an expense status.";
    }

    public static class Transaction
    {
        public const string BootstrapFetched = "Transaction entry data was fetched successfully.";
        public const string ListFetched = "Transactions were fetched successfully.";
        public const string DetailsFetched = "Transaction details were fetched successfully.";
        public const string Created = "The transaction was created successfully.";
        public const string Updated = "The transaction was updated successfully.";
        public const string MarkedPaid = "The transaction was marked as paid successfully.";
        public const string BootstrapFailed = "We could not load transaction entry data right now. Please try again later.";
        public const string ListFailed = "We could not load transactions right now. Please try again later.";
        public const string DetailsFailed = "We could not load transaction details right now. Please try again later.";
        public const string CreateFailed = "We could not create the transaction. Please try again later.";
        public const string UpdateFailed = "We could not update the transaction. Please try again later.";
        public const string MarkPaidFailed = "We could not mark the transaction as paid. Please try again later.";
        public const string NotFound = "The requested transaction was not found.";
        public const string EditWindowClosed = "This transaction can no longer be edited because the allowed edit window has closed.";
        public const string IdempotencyRequired = "An idempotency key is required so a retry does not create a duplicate transaction.";
        public const string TypeRequired = "Please select a transaction type.";
        public const string TypeInvalid = "Transaction type must be sale or expense.";
        public const string BranchRequired = "Please select a branch.";
        public const string BranchInvalid = "The selected branch was not found. Please choose a valid branch.";
        public const string PaymentModeRequired = "Please select a payment mode.";
        public const string ServicesRequired = "Please add at least one service or expense line item.";
        public const string LineServiceRequired = "Each line item must include a valid service or expense.";
        public const string QuantityInvalid = "Quantity must be greater than zero.";
        public const string InvalidExpenseIds = "One or more selected expenses were not found. Please choose valid expenses and try again.";
        public const string InvalidServiceIds = "One or more selected services were not found. Please choose valid services and try again.";
    }
}
