using System.Text.Json;

namespace TheBeautyHubCore.Parsing;

/// <summary>
/// Parses GUID lists from JSON arrays, comma-separated values, or the empty form token [].
/// </summary>
public static class GuidListParser
{
    public static List<Guid> Parse(string? value)
    {
        var ids = new List<Guid>();
        if (string.IsNullOrWhiteSpace(value))
            return ids;

        var trimmed = value.Trim();
        if (trimmed.Equals("[]", StringComparison.Ordinal)
            || trimmed.Equals("null", StringComparison.OrdinalIgnoreCase)
            || trimmed.Equals("undefined", StringComparison.OrdinalIgnoreCase))
        {
            return ids;
        }

        if (trimmed.StartsWith('['))
        {
            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    return ids;

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    if (element.ValueKind == JsonValueKind.String && Guid.TryParse(element.GetString(), out var fromString))
                        TryAdd(ids, fromString);
                }

                return ids;
            }
            catch (JsonException)
            {
                return Parse(trimmed.TrimStart('[').TrimEnd(']'));
            }
        }

        foreach (var part in trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (Guid.TryParse(part, out var id))
                TryAdd(ids, id);
        }

        return ids;
    }

    public static List<Guid> ParseMany(IEnumerable<string?>? values)
    {
        var ids = new List<Guid>();
        if (values == null)
            return ids;

        foreach (var value in values)
            ids.AddRange(Parse(value));

        return ids.Distinct().ToList();
    }

    public static List<Guid>? Merge(params IEnumerable<Guid>?[] sources)
    {
        var ids = new List<Guid>();
        foreach (var source in sources)
        {
            if (source == null)
                continue;
            foreach (var id in source)
                TryAdd(ids, id);
        }

        return ids.Count == 0 ? null : ids;
    }

    private static void TryAdd(List<Guid> ids, Guid id)
    {
        if (id == Guid.Empty || ids.Contains(id))
            return;
        ids.Add(id);
    }
}
