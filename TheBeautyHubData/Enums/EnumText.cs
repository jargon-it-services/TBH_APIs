using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace TheBeautyHubData.Enums;

/// <summary>
/// Reads API strings from enum metadata. Do not hard-code type/status labels outside the enum.
/// </summary>
public static class EnumText
{
    private static readonly ConcurrentDictionary<Type, object> Cache = new();

    public static string ToApiValue<T>(this T value) where T : struct, Enum
    {
        var map = GetMap<T>();
        if (map.ApiValues.TryGetValue(value, out var text))
            return text;
        throw new ArgumentOutOfRangeException(nameof(value), value, null);
    }

    /// <summary>
    /// First <see cref="EnumAliasAttribute"/> if present, otherwise the canonical API value.
    /// Use for existing database column defaults that still store a legacy alias.
    /// </summary>
    public static string ToStoredDefault<T>(this T value) where T : struct, Enum
    {
        var field = typeof(T).GetField(value.ToString());
        var alias = field?.GetCustomAttributes<EnumAliasAttribute>().Select(a => a.Value).FirstOrDefault();
        return string.IsNullOrWhiteSpace(alias) ? value.ToApiValue() : alias;
    }

    public static IReadOnlyList<string> AllApiValues<T>() where T : struct, Enum
    {
        var map = GetMap<T>();
        return Enum.GetValues<T>().Select(v => map.ApiValues[v]).ToList();
    }

    public static IReadOnlyList<string> ToApiValues<T>(IEnumerable<T> values) where T : struct, Enum
        => values.Select(v => v.ToApiValue()).ToList();

    public static bool TryParse<T>(string? text, out T value) where T : struct, Enum
    {
        value = default;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return GetMap<T>().Parse.TryGetValue(Normalize(text), out value);
    }

    public static T ParseOrThrow<T>(string? text, string invalidMessage) where T : struct, Enum
    {
        if (!TryParse<T>(text, out var value))
            throw new ArgumentException(invalidMessage);
        return value;
    }

    private static EnumMap<T> GetMap<T>() where T : struct, Enum
    {
        return (EnumMap<T>)Cache.GetOrAdd(typeof(T), _ => BuildMap<T>());
    }

    private static EnumMap<T> BuildMap<T>() where T : struct, Enum
    {
        var apiValues = new Dictionary<T, string>();
        var parse = new Dictionary<string, T>(StringComparer.Ordinal);

        foreach (T member in Enum.GetValues<T>())
        {
            var field = typeof(T).GetField(member.ToString());
            if (field == null)
                continue;

            var api = field.GetCustomAttribute<EnumMemberAttribute>()?.Value
                ?? member.ToString();

            apiValues[member] = api;
            AddParse(parse, api, member);
            AddParse(parse, member.ToString(), member);

            foreach (var alias in field.GetCustomAttributes<EnumAliasAttribute>())
                AddParse(parse, alias.Value, member);
        }

        return new EnumMap<T>(apiValues, parse);
    }

    private static void AddParse<T>(Dictionary<string, T> parse, string? raw, T member) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(raw))
            return;

        parse[Normalize(raw)] = member;
    }

    private static string Normalize(string value)
    {
        return value.Trim().Replace("_", " ", StringComparison.Ordinal).ToLowerInvariant();
    }

    private sealed class EnumMap<T> where T : struct, Enum
    {
        public EnumMap(Dictionary<T, string> apiValues, Dictionary<string, T> parse)
        {
            ApiValues = apiValues;
            Parse = parse;
        }

        public Dictionary<T, string> ApiValues { get; }

        public Dictionary<string, T> Parse { get; }
    }
}
