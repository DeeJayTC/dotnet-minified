using Smoower.Minified.Core;
using Xunit;

namespace Smoower.Minified.Tests;

public class GuardsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("x", false)]
    public void Nil_MatchesIsNullOrWhiteSpace(string? input, bool expected)
        => Assert.Equal(expected, input.nil());

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", false)]
    [InlineData("x", false)]
    public void Emp_MatchesIsNullOrEmpty(string? input, bool expected)
        => Assert.Equal(expected, input.emp());

    [Fact]
    public void None_TrueWhenEmpty() => Assert.True(Array.Empty<int>().none());

    [Fact]
    public void None_FalseWhenAny() => Assert.False(new[] { 1 }.none());
}
