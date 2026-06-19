using Xunit;
using Stj = Smoower.Minified.Json;
using Nsj = Smoower.Minified.Json.Newtonsoft;

namespace Smoower.Minified.Tests;

public class JsonExtensionsTests
{
    public record Dto(int Id, string Name);

    [Fact]
    public void Stj_RoundTrips()
    {
        var json = Stj.JsonExtensions.toJson(new Dto(7, "ada"));
        var back = Stj.JsonExtensions.fromJson<Dto>(json);
        Assert.Equal(new Dto(7, "ada"), back);
    }

    [Fact]
    public void Stj_PrettyWritesIndented()
        => Assert.Contains("\n", Stj.JsonExtensions.toJson(new Dto(1, "x"), pretty: true));

    [Fact]
    public void Newtonsoft_RoundTrips()
    {
        var json = Nsj.JsonExtensions.toJson(new Dto(7, "ada"));
        var back = Nsj.JsonExtensions.fromJson<Dto>(json);
        Assert.Equal(new Dto(7, "ada"), back);
    }

    [Fact]
    public void Newtonsoft_PrettyWritesIndented()
        => Assert.Contains("\n", Nsj.JsonExtensions.toJson(new Dto(1, "x"), pretty: true));
}
