using System.Text.Json;

namespace Smoower.Minified.Json;

// Compact System.Text.Json helpers.
public static class JsonExtensions
{
    private static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };

    public static string toJson<T>(this T value, bool pretty = false)
        => JsonSerializer.Serialize(value, pretty ? Indented : null);

    public static T? fromJson<T>(this string json)
        => JsonSerializer.Deserialize<T>(json);
}
