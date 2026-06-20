using Microsoft.AspNetCore.Mvc;
using Smoower.Minified.AspNetCore;

namespace Smoower.Minified.Tests;

public class ShortResultTests
{
    // Ctl's helpers are protected (so MVC never routes them as actions); expose them
    // through a concrete controller to assert the status/shape.
    private sealed class C : Ctl
    {
        public IActionResult CallNf() => nf();
        public IActionResult CallNc() => nc();
        public IActionResult CallUn() => un();
        public IActionResult CallForb() => forb();
        public IActionResult CallBad() => bad();
        public IActionResult CallBadObj() => bad(new { x = 1 });
        public IActionResult CallUnp() => unp("nope");
    }

    [F] public void Nf_Is404() => new C().CallNf().isType<NotFoundResult>();
    [F] public void Nc_Is204() => new C().CallNc().isType<NoContentResult>();
    [F] public void Un_Is401() => new C().CallUn().isType<UnauthorizedResult>();
    [F] public void Forb_IsForbid() => new C().CallForb().isType<ForbidResult>();
    [F] public void Bad_Is400() => new C().CallBad().isType<BadRequestResult>();
    [F] public void BadObj_Is400WithBody() => new C().CallBadObj().isType<BadRequestObjectResult>();

    [F]
    public void Unp_Is422WithErrorBody()
    {
        var r = new C().CallUnp().isType<UnprocessableEntityObjectResult>();
        r.StatusCode.eq(422);
        r.Value!.GetType().GetProperty("error")!.GetValue(r.Value).eq("nope");
    }
}
