using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

public enum TransactionKind
{
    [EnumMember(Value = "sale")]
    Sale = 1,

    [EnumMember(Value = "expense")]
    Expense = 2
}

public static class TransactionKinds
{
    public static string ToApiValue(this TransactionKind kind) => EnumText.ToApiValue(kind);

    public static TransactionKind ParseOrThrow(string? value, string invalidMessage)
        => EnumText.ParseOrThrow<TransactionKind>(value, invalidMessage);

    public static IReadOnlyList<string> AllApiValues => EnumText.AllApiValues<TransactionKind>();
}
