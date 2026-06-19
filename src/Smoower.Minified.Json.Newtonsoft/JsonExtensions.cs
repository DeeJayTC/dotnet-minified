using Newtonsoft.Json;

namespace Smoower.Minified.Json.Newtonsoft;

// Compact Newtonsoft.Json helpers. Same surface as Smoower.Minified.Json so code
// reads identically whichever serializer a project uses; import only one.
public static class JsonExtensions
{
    public static string toJson<T>(this T value, bool pretty = false)
        => JsonConvert.SerializeObject(value, pretty ? Formatting.Indented : Formatting.None);

    public static T? fromJson<T>(this string json)
        => JsonConvert.DeserializeObject<T>(json);
}
