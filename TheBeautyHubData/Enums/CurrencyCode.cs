using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum CurrencyCode
{
    [EnumMember(Value = "INR")]
    Inr = 1
}

public static class CurrencyCodes
{
    public static string ToApiValue(this CurrencyCode code) => EnumText.ToApiValue(code);
}
