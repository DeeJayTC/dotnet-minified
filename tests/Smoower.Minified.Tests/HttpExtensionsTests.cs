using System.Net;
using System.Text;
using Smoower.Minified.Http;
using Xunit;

namespace Smoower.Minified.Tests;

public class HttpExtensionsTests
{
    private sealed class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public HttpRequestMessage? Last { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            Last = request;
            return Task.FromResult(respond(request));
        }
    }

    private record Dto(int Id, string Name);

    [Fact]
    public async Task GetJson_DeserializesBody()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":7,\"name\":\"ada\"}", Encoding.UTF8, "application/json")
        });
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://x/") };
        var dto = await client.getJson<Dto>("things/7");
        Assert.Equal(7, dto!.Id);
        Assert.Equal("ada", dto.Name);
    }

    [Fact]
    public async Task PostJson_SendsBodyAndPost()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.Created));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://x/") };
        var resp = await client.postJson("things", new Dto(1, "x"));
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        Assert.Equal(HttpMethod.Post, handler.Last!.Method);
    }

    [Fact]
    public async Task Del_IssuesDelete()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://x/") };
        var resp = await client.del("things/1");
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
        Assert.Equal(HttpMethod.Delete, handler.Last!.Method);
    }
}
