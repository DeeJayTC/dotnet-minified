using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;

namespace Smoower.Minified.Tests;

public class ProducesAttributesTests
{
    [F]
    public void PlainAttributes_CarryExpectedStatusCodes()
    {
        new P200Attribute().StatusCode.eq(200);
        new P201Attribute().StatusCode.eq(201);
        new P204Attribute().StatusCode.eq(204);
        new P400Attribute().StatusCode.eq(400);
        new P404Attribute().StatusCode.eq(404);
        new P409Attribute().StatusCode.eq(409);
        new P500Attribute().StatusCode.eq(500);
    }

    [F]
    public void GenericAttributes_CarryStatusCodeAndType()
    {
        var a = new P200Attribute<string>();
        a.StatusCode.eq(200);
        a.Type.eq(typeof(string));

        var c = new P201Attribute<int>();
        c.StatusCode.eq(201);
        c.Type.eq(typeof(int));
    }

    [F]
    public void Attributes_AreProducesResponseType()
        => new P404Attribute().isAssignable<ProducesResponseTypeAttribute>();
}
