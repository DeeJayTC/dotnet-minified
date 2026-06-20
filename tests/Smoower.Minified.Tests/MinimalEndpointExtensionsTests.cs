using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Smoower.Minified.MinimalApi;

namespace Smoower.Minified.Tests;

public class MinimalEndpointExtensionsTests
{
    [F]
    public void Verbs_MapEndpointsAndReturnBuilders()
    {
        var app = WebApplication.Create();
        app.g("/g", () => "g").notNul();
        app.po("/po", () => "po").notNul();
        app.pu("/pu", () => "pu").notNul();
        app.pa("/pa", () => "pa").notNul();
        app.dl("/dl", () => "dl").notNul();
        ((IEndpointRouteBuilder)app).DataSources.notEmpty();
    }

    [F]
    public void Grp_OpensGroupAndAuthChains()
    {
        var app = WebApplication.Create();
        var users = app.grp("/users").notNul();
        users.g("/{id}", (int id) => id).auth().notNul();
    }

    [F]
    public void Anon_Chains()
    {
        var app = WebApplication.Create();
        app.g("/x", () => "x").anon().notNul();
    }
}
