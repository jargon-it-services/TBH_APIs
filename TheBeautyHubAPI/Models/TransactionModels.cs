using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheBeautyHubAPI.Models
{
    public class TransactionNamedItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class TransactionBootstrapServiceResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("frequent")]
        public bool Frequent { get; set; }
    }

    public class TransactionBootstrapResponse
    {
        [JsonPropertyName("services")]
        public List<TransactionBootstrapServiceResponse> Services { get; set; } = new();

        [JsonPropertyName("expenses")]
        public List<TransactionNamedItemResponse> Expenses { get; set; } = new();

        [JsonPropertyName("staff")]
        public List<TransactionNamedItemResponse> Staff { get; set; } = new();

        [JsonPropertyName("branches")]
        public List<TransactionNamedItemResponse> Branches { get; set; } = new();

        [JsonPropertyName("user_role")]
        public string UserRole { get; set; } = string.Empty;

        [JsonPropertyName("logged_in_user_id")]
        public Guid LoggedInUserId { get; set; }

        [JsonPropertyName("logged_in_branch_id")]
        public Guid? LoggedInBranchId { get; set; }

        [JsonPropertyName("last_payment_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastPaymentMode { get; set; }

        [JsonPropertyName("last_transaction_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastTransactionType { get; set; }
    }

    public class SaveTransactionLineRequest
    {
        [JsonPropertyName("service_id")]
        public Guid? ServiceId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        [JsonPropertyName("staff_id")]
        public Guid? StaffId { get; set; }
    }

    public class SaveTransactionRequest
    {
        [JsonPropertyName("idempotency_key")]
        public string? IdempotencyKey { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("branch_id")]
        public Guid? BranchId { get; set; }

        [Required]
        [JsonPropertyName("payment_mode")]
        public string PaymentMode { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("services")]
        public List<SaveTransactionLineRequest>? Services { get; set; }

        [JsonPropertyName("customer_name")]
        public string? CustomerName { get; set; }

        [JsonPropertyName("customer_mobile")]
        public string? CustomerMobile { get; set; }

        [JsonPropertyName("remark")]
        public string? Remark { get; set; }

        [JsonPropertyName("staff_id")]
        public Guid? StaffId { get; set; }

        [JsonPropertyName("coupon_code")]
        public string? CouponCode { get; set; }
    }

    public class TransactionSavedResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("grand_total")]
        public decimal GrandTotal { get; set; }

        [JsonPropertyName("can_edit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("edit_count")]
        public int EditCount { get; set; }

        [JsonPropertyName("customer_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomerName { get; set; }

        [JsonPropertyName("customer_mobile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CustomerMobile { get; set; }

        [JsonPropertyName("editable_until")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? EditableUntil { get; set; }

        [JsonPropertyName("last_edited_by")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastEditedBy { get; set; }

        [JsonPropertyName("last_edited_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? LastEditedAt { get; set; }

        [JsonPropertyName("paid_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? PaidAt { get; set; }
    }

    public class TransactionMarkPaidResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("paid_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? PaidAt { get; set; }
    }

    public class TransactionListItemResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("branch")]
        public string Branch { get; set; } = string.Empty;

        [JsonPropertyName("branch_id")]
        public Guid? BranchId { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; } = string.Empty;

        [JsonPropertyName("service_id")]
        public Guid? ServiceId { get; set; }

        [JsonPropertyName("staff")]
        public string Staff { get; set; } = string.Empty;

        [JsonPropertyName("staff_id")]
        public Guid? StaffId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("payment_mode")]
        public string PaymentMode { get; set; } = string.Empty;

        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("customer_id")]
        public Guid? CustomerId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class TransactionListFiltersResponse
    {
        [JsonPropertyName("branches")]
        public List<TransactionNamedItemResponse> Branches { get; set; } = new();

        [JsonPropertyName("services")]
        public List<TransactionNamedItemResponse> Services { get; set; } = new();

        [JsonPropertyName("staff")]
        public List<TransactionNamedItemResponse> Staff { get; set; } = new();

        [JsonPropertyName("statuses")]
        public List<string> Statuses { get; set; } = new();

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = new();

        [JsonPropertyName("payment_modes")]
        public List<string> PaymentModes { get; set; } = new();

        [JsonPropertyName("periods")]
        public List<string> Periods { get; set; } = new();

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "INR";
    }

    public class TransactionListMetaResponse
    {
        [JsonPropertyName("feature_lock")]
        public List<string> FeatureLock { get; set; } = new();
    }

    public class TransactionListDataResponse
    {
        [JsonPropertyName("meta")]
        public TransactionListMetaResponse Meta { get; set; } = new();

        [JsonPropertyName("filters")]
        public TransactionListFiltersResponse Filters { get; set; } = new();

        [JsonPropertyName("transactions")]
        public List<TransactionListItemResponse> Transactions { get; set; } = new();
    }

    public class TransactionLineBreakdownResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("base_amount")]
        public decimal BaseAmount { get; set; }

        [JsonPropertyName("tax_percentage")]
        public decimal TaxPercentage { get; set; }

        [JsonPropertyName("tax_amount")]
        public decimal TaxAmount { get; set; }

        [JsonPropertyName("discount_percentage")]
        public decimal DiscountPercentage { get; set; }

        [JsonPropertyName("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [JsonPropertyName("gross_amount")]
        public decimal GrossAmount { get; set; }

        [JsonPropertyName("net_amount")]
        public decimal NetAmount { get; set; }
    }

    public class TransactionCouponResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("discount_amount")]
        public decimal DiscountAmount { get; set; }
    }

    public class TransactionSummaryResponse
    {
        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("tax_percentage")]
        public decimal TaxPercentage { get; set; }

        [JsonPropertyName("tax_amount")]
        public decimal TaxAmount { get; set; }

        [JsonPropertyName("coupon_discount")]
        public decimal CouponDiscount { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "INR";
    }

    public class TransactionPriceBreakdownResponse
    {
        [JsonPropertyName("services")]
        public List<TransactionLineBreakdownResponse> Services { get; set; } = new();

        [JsonPropertyName("coupon")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TransactionCouponResponse? Coupon { get; set; }

        [JsonPropertyName("summary")]
        public TransactionSummaryResponse Summary { get; set; } = new();
    }

    public class TransactionDateTimeResponse
    {
        [JsonPropertyName("iso")]
        public DateTime Iso { get; set; }

        [JsonPropertyName("display")]
        public string Display { get; set; } = string.Empty;
    }

    public class TransactionBranchInfoResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;
    }

    public class TransactionRecordResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("payment_mode")]
        public string PaymentMode { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("price_breakdown")]
        public TransactionPriceBreakdownResponse PriceBreakdown { get; set; } = new();

        [JsonPropertyName("date_time")]
        public TransactionDateTimeResponse DateTime { get; set; } = new();

        [JsonPropertyName("branch")]
        public TransactionBranchInfoResponse? Branch { get; set; }

        [JsonPropertyName("staff")]
        public TransactionNamedItemResponse? Staff { get; set; }

        [JsonPropertyName("remark")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Remark { get; set; }

        [JsonPropertyName("can_edit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("edit_count")]
        public int EditCount { get; set; }

        [JsonPropertyName("feature_lock")]
        public List<string> FeatureLock { get; set; } = new();

        [JsonPropertyName("editable_until")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? EditableUntil { get; set; }

        [JsonPropertyName("last_edited_by")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastEditedBy { get; set; }

        [JsonPropertyName("last_edited_at")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? LastEditedAt { get; set; }
    }
}
