using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum TransactionStatus
{
    [EnumMember(Value = "pending")]
    Pending = 1,

    [EnumMember(Value = "paid")]
    Paid = 2,

    [EnumMember(Value = "Draft")]
    Draft = 3,

    [EnumMember(Value = "Posted")]
    Posted = 4,

    [EnumMember(Value = "Cancelled")]
    Cancelled = 5
}

public static class TransactionStatuses
{
    public static readonly TransactionStatus[] ListFilters =
    [
        TransactionStatus.Paid,
        TransactionStatus.Pending
    ];

    public static string ToApiValue(this TransactionStatus status) => EnumText.ToApiValue(status);

    public static IReadOnlyList<string> ListFilterApiValues => EnumText.ToApiValues(ListFilters);
}
