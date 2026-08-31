using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum SalaryType
{
    [EnumMember(Value = "Fixed Salary")]
    [EnumAlias("fixed")]
    [EnumAlias("Fixed Pay")]
    Fixed = 1,

    [EnumMember(Value = "Hybrid")]
    [EnumAlias("fixed_plus_target")]
    [EnumAlias("Fixed + Target Bonus")]
    [EnumAlias("fixed plus target")]
    Hybrid = 2,

    [EnumMember(Value = "Service Commission")]
    [EnumAlias("commission")]
    [EnumAlias("incentive")]
    [EnumAlias("Incentive")]
    Commission = 3
}

public static class SalaryTypes
{
    public static string ToApiValue(this SalaryType type) => EnumText.ToApiValue(type);

    public static bool TryParse(string? value, out SalaryType type) => EnumText.TryParse(value, out type);

    public static SalaryType ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<SalaryType>(value, invalidMessage);
}
