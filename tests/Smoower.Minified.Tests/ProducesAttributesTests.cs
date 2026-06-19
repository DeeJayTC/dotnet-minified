using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;
using Xunit;

namespace Smoower.Minified.Tests;

public class ProducesAttributesTests
{
    [Fact]
    public void PlainAttributes_CarryExpectedStatusCodes()
    {
        Assert.Equal(200, new P200Attribute().StatusCode);
        Assert.Equal(201, new P201Attribute().StatusCode);
        Assert.Equal(204, new P204Attribute().StatusCode);
        Assert.Equal(400, new P400Attribute().StatusCode);
        Assert.Equal(404, new P404Attribute().StatusCode);
        Assert.Equal(409, new P409Attribute().StatusCode);
        Assert.Equal(500, new P500Attribute().StatusCode);
    }

    [Fact]
    public void GenericAttributes_CarryStatusCodeAndType()
    {
        var a = new P200Attribute<string>();
        Assert.Equal(200, a.StatusCode);
        Assert.Equal(typeof(string), a.Type);

        var c = new P201Attribute<int>();
        Assert.Equal(201, c.StatusCode);
        Assert.Equal(typeof(int), c.Type);
    }

    [Fact]
    public void Attributes_AreProducesResponseType()
        => Assert.IsAssignableFrom<ProducesResponseTypeAttribute>(new P404Attribute());
}
