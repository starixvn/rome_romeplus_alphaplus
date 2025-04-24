using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests;

public abstract class TestBase : IDisposable
{
    protected readonly IServiceCollection Services;
    protected readonly IServiceProvider ServiceProvider;

    protected TestBase()
    {
        Services = new ServiceCollection();
        ConfigureServices(Services);
        ServiceProvider = Services.BuildServiceProvider();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // Add common service configurations here
    }

    protected T GetService<T>() where T : class
    {
        return ServiceProvider.GetService<T>();
    }

    protected T GetRequiredService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public virtual void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}