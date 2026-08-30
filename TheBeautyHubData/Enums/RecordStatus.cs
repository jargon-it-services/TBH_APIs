using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum RecordStatus
{
    [EnumMember(Value = "active")]
    [EnumAlias("Active")]
    Active = 1,

    [EnumMember(Value = "inactive")]
    [EnumAlias("Inactive")]
    Inactive = 2
}

public static class RecordStatuses
{
    public static string ToApiValue(this RecordStatus status) => EnumText.ToApiValue(status);

    public static bool TryParse(string? value, out RecordStatus status) => EnumText.TryParse(value, out status);

    public static RecordStatus ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<RecordStatus>(value, invalidMessage);

    public static bool IsActive(string? status)
        => TryParse(status, out var parsed) && parsed == RecordStatus.Active;
}
