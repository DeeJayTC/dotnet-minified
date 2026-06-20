using Stj = Smoower.Minified.Json;
using Nsj = Smoower.Minified.Json.Newtonsoft;

namespace Smoower.Minified.Tests;

public class JsonExtensionsTests
{
    public record Dto(int Id, string Name);

    [F]
    public void Stj_RoundTrips()
    {
        var json = Stj.JsonExtensions.toJson(new Dto(7, "ada"));
        Stj.JsonExtensions.fromJson<Dto>(json).eq(new Dto(7, "ada"));
    }

    [F]
    public void Stj_PrettyWritesIndented()
        => Stj.JsonExtensions.toJson(new Dto(1, "x"), pretty: true).hasText("\n");

    [F]
    public void Newtonsoft_RoundTrips()
    {
        var json = Nsj.JsonExtensions.toJson(new Dto(7, "ada"));
        Nsj.JsonExtensions.fromJson<Dto>(json).eq(new Dto(7, "ada"));
    }

    [F]
    public void Newtonsoft_PrettyWritesIndented()
        => Nsj.JsonExtensions.toJson(new Dto(1, "x"), pretty: true).hasText("\n");
}
