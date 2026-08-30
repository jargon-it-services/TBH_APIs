using System;

namespace TheBeautyHubData.Enums;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class EnumAliasAttribute : Attribute
{
    public EnumAliasAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
