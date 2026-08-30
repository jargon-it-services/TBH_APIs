using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum PaymentMode
{
    [EnumMember(Value = "cash")]
    Cash = 1,

    [EnumMember(Value = "upi")]
    Upi = 2,

    [EnumMember(Value = "card")]
    Card = 3
}

public static class PaymentModes
{
    public static string ToApiValue(this PaymentMode mode) => EnumText.ToApiValue(mode);

    public static PaymentMode ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<PaymentMode>(value, invalidMessage);

    public static IReadOnlyList<string> AllApiValues => EnumText.AllApiValues<PaymentMode>();
}
