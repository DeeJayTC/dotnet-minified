using Microsoft.Extensions.DependencyInjection;
using Smoower.Minified.Hosting;

namespace Smoower.Minified.Tests;

public class ServiceExtensionsTests
{
    private interface IFoo;
    private sealed class Foo : IFoo;
    private sealed class Bar;

    [F]
    public void Scoped_RegistersWithScopedLifetime()
    {
        var services = new ServiceCollection().scoped<IFoo, Foo>();
        var d = services.sole();
        d.Lifetime.eq(ServiceLifetime.Scoped);
        d.ServiceType.eq(typeof(IFoo));
        d.ImplementationType.eq(typeof(Foo));
    }

    [F]
    public void Single_ResolvesSameInstance()
    {
        var provider = new ServiceCollection().single<Bar>().BuildServiceProvider();
        provider.GetRequiredService<Bar>().same(provider.GetRequiredService<Bar>());
    }

    [F]
    public void Trans_ResolvesNewInstances()
    {
        var provider = new ServiceCollection().trans<Bar>().BuildServiceProvider();
        provider.GetRequiredService<Bar>().notSame(provider.GetRequiredService<Bar>());
    }

    [F]
    public void Svc_ResolvesRequiredService()
    {
        IServiceProvider provider = new ServiceCollection().single<Bar>().BuildServiceProvider();
        provider.GetRequiredService<Bar>().same(provider.svc<Bar>());
    }
}
