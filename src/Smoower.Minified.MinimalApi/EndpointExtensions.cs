using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Smoower.Minified.MinimalApi;

// Minimal API route-mapping shorteners. The verb mappers wrap MapGet/MapPost/...
// one-for-one and forward the RouteHandlerBuilder so you can keep chaining
// (.auth(), .anon(), .WithName(), ...). `grp` opens a route group. These are the
// minimal-API counterpart to the [HG]/[HPO]/... attributes on MVC controllers.
public static class EndpointExtensions
{
    public static RouteGroupBuilder grp(this IEndpointRouteBuilder e, string prefix) => e.MapGroup(prefix);

    public static RouteHandlerBuilder g(this IEndpointRouteBuilder e, string pattern, Delegate handler) => e.MapGet(pattern, handler);
    public static RouteHandlerBuilder po(this IEndpointRouteBuilder e, string pattern, Delegate handler) => e.MapPost(pattern, handler);
    public static RouteHandlerBuilder pu(this IEndpointRouteBuilder e, string pattern, Delegate handler) => e.MapPut(pattern, handler);
    public static RouteHandlerBuilder pa(this IEndpointRouteBuilder e, string pattern, Delegate handler) => e.MapPatch(pattern, handler);
    public static RouteHandlerBuilder dl(this IEndpointRouteBuilder e, string pattern, Delegate handler) => e.MapDelete(pattern, handler);

    // Convention shorteners — work on a single endpoint (RouteHandlerBuilder) or a
    // whole group (RouteGroupBuilder), since both are IEndpointConventionBuilder.
    public static TBuilder auth<TBuilder>(this TBuilder b) where TBuilder : IEndpointConventionBuilder => b.RequireAuthorization();
    public static TBuilder auth<TBuilder>(this TBuilder b, params string[] policies) where TBuilder : IEndpointConventionBuilder => b.RequireAuthorization(policies);
    public static TBuilder anon<TBuilder>(this TBuilder b) where TBuilder : IEndpointConventionBuilder => b.AllowAnonymous();
}
