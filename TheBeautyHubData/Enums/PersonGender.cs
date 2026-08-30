using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum PersonGender
{
    [EnumMember(Value = "Male")]
    Male = 1,

    [EnumMember(Value = "Female")]
    Female = 2,

    [EnumMember(Value = "Other")]
    Other = 3
}

public static class PersonGenders
{
    public static string ToApiValue(this PersonGender gender) => EnumText.ToApiValue(gender);

    public static PersonGender ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<PersonGender>(value, invalidMessage);
}
