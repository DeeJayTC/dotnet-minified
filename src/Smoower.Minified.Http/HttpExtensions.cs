using System.Net.Http.Json;

namespace Smoower.Minified.Http;

// Compact HttpClient JSON helpers. Thin wrappers over System.Net.Http.Json.
public static class HttpExtensions
{
    public static Task<T?> getJson<T>(this HttpClient c, string url, CancellationToken ct = default)
        => c.GetFromJsonAsync<T>(url, ct);

    public static Task<HttpResponseMessage> postJson<T>(this HttpClient c, string url, T body, CancellationToken ct = default)
        => c.PostAsJsonAsync(url, body, ct);

    public static Task<HttpResponseMessage> putJson<T>(this HttpClient c, string url, T body, CancellationToken ct = default)
        => c.PutAsJsonAsync(url, body, ct);

    public static Task<HttpResponseMessage> patchJson<T>(this HttpClient c, string url, T body, CancellationToken ct = default)
        => c.PatchAsJsonAsync(url, body, ct);

    public static Task<HttpResponseMessage> del(this HttpClient c, string url, CancellationToken ct = default)
        => c.DeleteAsync(url, ct);
}
