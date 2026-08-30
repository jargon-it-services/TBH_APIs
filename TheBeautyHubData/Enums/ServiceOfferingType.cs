using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum ServiceOfferingType
{
    [EnumMember(Value = "in_salon")]
    [EnumAlias("in salon")]
    InSalon = 1,

    [EnumMember(Value = "home")]
    [EnumAlias("home_service")]
    Home = 2
}

public static class ServiceOfferingTypes
{
    public static string ToApiValue(this ServiceOfferingType type) => EnumText.ToApiValue(type);

    public static ServiceOfferingType ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<ServiceOfferingType>(value, invalidMessage);
}
