using TheBeautyHubCore.Parsing;
using Xunit;

namespace TheBeautyHub.Tests;

public class GuidListParserTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("[]")]
    [InlineData("null")]
    [InlineData("undefined")]
    public void Empty_tokens_return_no_ids(string? value)
    {
        Assert.Empty(GuidListParser.Parse(value));
    }

    [Fact]
    public void Json_array_parses_guids()
    {
        var a = Guid.Parse("244ffded-2edc-4584-9716-955894c58da9");
        var parsed = GuidListParser.Parse($"[\"{a}\"]");
        Assert.Equal(new[] { a }, parsed);
    }

    [Fact]
    public void Bracketed_unquoted_guid_parses()
    {
        var a = Guid.Parse("244ffded-2edc-4584-9716-955894c58da9");
        var parsed = GuidListParser.Parse($"[{a}]");
        Assert.Equal(new[] { a }, parsed);
    }

    [Fact]
    public void Comma_separated_parses_guids()
    {
        var a = Guid.Parse("244ffded-2edc-4584-9716-955894c58da9");
        var b = Guid.Parse("dec70dc9-e396-49e5-a359-5ef69864ef43");
        var parsed = GuidListParser.Parse($"{a},{b}");
        Assert.Equal(new[] { a, b }, parsed);
    }

    [Fact]
    public void Empty_guid_is_ignored()
    {
        Assert.Empty(GuidListParser.Parse(Guid.Empty.ToString()));
    }

    [Fact]
    public void Merge_combines_branch_and_branch_ids()
    {
        var a = Guid.Parse("244ffded-2edc-4584-9716-955894c58da9");
        var merged = GuidListParser.Merge(null, new[] { a });
        Assert.Equal(new[] { a }, merged);
    }

    [Fact]
    public void Merge_of_empty_sources_is_null()
    {
        Assert.Null(GuidListParser.Merge(null, Array.Empty<Guid>()));
    }
}
