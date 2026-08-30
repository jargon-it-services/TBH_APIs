using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum SubscriptionStatus
{
    [EnumMember(Value = "Pending")]
    Pending = 1,

    [EnumMember(Value = "Active")]
    Active = 2,

    [EnumMember(Value = "Expired")]
    Expired = 3,

    [EnumMember(Value = "Cancelled")]
    Cancelled = 4
}

public static class SubscriptionStatuses
{
    public static string ToApiValue(this SubscriptionStatus status) => EnumText.ToApiValue(status);
}
