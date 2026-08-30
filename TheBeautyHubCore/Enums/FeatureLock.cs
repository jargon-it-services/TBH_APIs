using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TheBeautyHubCore.Enums;

/// <summary>
/// Screens / capabilities that can be locked for a Beauty Hub plan.
/// Returned from GET /api/management/feature-lock.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FeatureLock
{
    /// <summary>Reports screen.</summary>
    [EnumMember(Value = "report")]
    Report = 1,

    /// <summary>Create transaction screen (client spelling: create_transcation).</summary>
    [EnumMember(Value = "create_transcation")]
    CreateTransaction = 2,

    /// <summary>Branch management screen.</summary>
    [EnumMember(Value = "branches")]
    Branches = 3,

    /// <summary>Staff management screen.</summary>
    [EnumMember(Value = "staff")]
    Staff = 4,

    /// <summary>Services catalog screen.</summary>
    [EnumMember(Value = "services")]
    Services = 5,

    /// <summary>Expenses screen.</summary>
    [EnumMember(Value = "expenses")]
    Expenses = 6,

    /// <summary>Salary rules screen.</summary>
    [EnumMember(Value = "salary_rules")]
    SalaryRules = 7
}

/// <summary>
/// Helpers for serializing / parsing <see cref="FeatureLock"/> API codes.
/// </summary>
public static class FeatureLockCodes
{
    /// <summary>Default locks applied to the Free trial plan.</summary>
    public static readonly FeatureLock[] FreeTrialDefaults =
    [
        FeatureLock.Report,
        FeatureLock.CreateTransaction
    ];

    public static string ToApiCode(this FeatureLock feature)
    {
        var member = typeof(FeatureLock).GetMember(feature.ToString()).FirstOrDefault();
        var enumMember = member?.GetCustomAttribute<EnumMemberAttribute>();
        return enumMember?.Value ?? ToSnakeCase(feature.ToString());
    }

    public static IReadOnlyList<string> ToApiCodes(IEnumerable<FeatureLock> features)
        => features.Select(ToApiCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

    public static string ToJsonArray(IEnumerable<FeatureLock> features)
        => System.Text.Json.JsonSerializer.Serialize(ToApiCodes(features));

    public static bool TryParse(string? code, out FeatureLock feature)
    {
        feature = default;
        if (string.IsNullOrWhiteSpace(code))
            return false;

        var normalized = code.Trim();
        foreach (FeatureLock value in Enum.GetValues<FeatureLock>())
        {
            if (string.Equals(value.ToApiCode(), normalized, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value.ToString(), normalized, StringComparison.OrdinalIgnoreCase))
            {
                feature = value;
                return true;
            }
        }

        return false;
    }

    public static List<FeatureLock> ParseMany(IEnumerable<string>? codes)
    {
        var result = new List<FeatureLock>();
        if (codes == null)
            return result;

        foreach (var code in codes)
        {
            if (TryParse(code, out var feature) && !result.Contains(feature))
                result.Add(feature);
        }

        return result;
    }

    public static List<string> NormalizeApiCodes(IEnumerable<string>? codes)
        => ToApiCodes(ParseMany(codes)).ToList();

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var chars = new List<char> { char.ToLowerInvariant(value[0]) };
        for (var i = 1; i < value.Length; i++)
        {
            var c = value[i];
            if (char.IsUpper(c))
            {
                chars.Add('_');
                chars.Add(char.ToLowerInvariant(c));
            }
            else
            {
                chars.Add(c);
            }
        }

        return new string(chars.ToArray());
    }
}
