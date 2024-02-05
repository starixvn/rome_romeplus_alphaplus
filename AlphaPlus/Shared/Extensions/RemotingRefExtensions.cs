using System.Reflection;
using Fanex.RemotingRef;

internal static class RemotingRefExtensions
{
    internal static IServiceCollection AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication(Assembly.GetExecutingAssembly().FullName);

        return services.AddSingleton<IRemotingRefService, RemotingRefService>();
    }
}