using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum TransactionListPeriod
{
    [EnumMember(Value = "today")]
    Today = 1,

    [EnumMember(Value = "week")]
    Week = 2,

    [EnumMember(Value = "month")]
    Month = 3
}

public static class TransactionListPeriods
{
    public static IReadOnlyList<string> AllApiValues => EnumText.AllApiValues<TransactionListPeriod>();
}
