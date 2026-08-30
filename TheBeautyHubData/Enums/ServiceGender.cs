using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum ServiceGender
{
    [EnumMember(Value = "unisex")]
    Unisex = 1,

    [EnumMember(Value = "male")]
    Male = 2,

    [EnumMember(Value = "female")]
    Female = 3
}

public static class ServiceGenders
{
    public static string ToApiValue(this ServiceGender gender) => EnumText.ToApiValue(gender);

    public static ServiceGender ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<ServiceGender>(value, invalidMessage);
}
