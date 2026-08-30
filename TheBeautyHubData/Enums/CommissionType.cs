using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum CommissionType
{
    [EnumMember(Value = "Fixed Amount")]
    [EnumAlias("flat")]
    [EnumAlias("fixed")]
    FixedAmount = 1,

    [EnumMember(Value = "Percentage")]
    [EnumAlias("percent")]
    [EnumAlias("percentage")]
    Percentage = 2
}

public static class CommissionTypes
{
    public static string ToApiValue(this CommissionType type) => EnumText.ToApiValue(type);

    public static bool TryParse(string? value, out CommissionType type) => EnumText.TryParse(value, out type);

    public static CommissionType ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<CommissionType>(value, invalidMessage);
}
