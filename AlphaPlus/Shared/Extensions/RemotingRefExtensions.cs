using System.Reflection;
using Fanex.RemotingRef;

internal static class RemotingRefExtensions
{
    internal static IServiceCollection AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration["RemotingRefUrl"]))
            .FromApplication(Assembly.GetExecutingAssembly().FullName);
#pragma warning restore CS8604 // Possible null reference argument.

        return services.AddSingleton<IRemotingRefService, RemotingRefService>();
    }
}