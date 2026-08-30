using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum SalaryType
{
    [EnumMember(Value = "fixed")]
    Fixed = 1,

    [EnumMember(Value = "fixed_plus_target")]
    FixedPlusTarget = 2,

    [EnumMember(Value = "commission")]
    [EnumAlias("incentive")]
    Commission = 3
}

public static class SalaryTypes
{
    public static string ToApiValue(this SalaryType type) => EnumText.ToApiValue(type);

    public static SalaryType ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<SalaryType>(value, invalidMessage);
}
