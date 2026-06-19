using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Smoower.Minified.AspNetCore;

[AttributeUsage(AttributeTargets.Class)]
public sealed class APIAttribute : ApiControllerAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class RTAttribute(string template) : RouteAttribute(template);

[AttributeUsage(AttributeTargets.Method)]
public sealed class HGAttribute : HttpGetAttribute
{
    public HGAttribute() { }
    public HGAttribute(string template) : base(template) { }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class HPOAttribute : HttpPostAttribute
{
    public HPOAttribute() { }
    public HPOAttribute(string template) : base(template) { }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class HPUAttribute : HttpPutAttribute
{
    public HPUAttribute() { }
    public HPUAttribute(string template) : base(template) { }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class HPAAttribute : HttpPatchAttribute
{
    public HPAAttribute() { }
    public HPAAttribute(string template) : base(template) { }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class HDAttribute : HttpDeleteAttribute
{
    public HDAttribute() { }
    public HDAttribute(string template) : base(template) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class AUTHAttribute : AuthorizeAttribute
{
    public AUTHAttribute() { }
    public AUTHAttribute(string policy) : base(policy) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class ANONAttribute : AllowAnonymousAttribute;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class FBAttribute : FromBodyAttribute;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class FRAttribute : FromRouteAttribute;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class FQAttribute : FromQueryAttribute;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class FHAttribute : FromHeaderAttribute;
