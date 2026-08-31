using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum ServiceOfferingType
{
    [EnumMember(Value = "in_salon")]
    [EnumAlias("in salon")]
    [EnumAlias("Service")]
    InSalon = 1,

    [EnumMember(Value = "home")]
    [EnumAlias("home_service")]
    Home = 2
}

public static class ServiceOfferingTypes
{
    public static string ToApiValue(this ServiceOfferingType type) => EnumText.ToApiValue(type);

    public static bool TryParse(string? value, out ServiceOfferingType type) => EnumText.TryParse(value, out type);

    public static ServiceOfferingType ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<ServiceOfferingType>(value, invalidMessage);
}
