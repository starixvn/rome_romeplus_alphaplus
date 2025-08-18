using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Fanex.RemotingRef;

[SuppressMessage("SonarQube", "S3903:Types should be defined in named namespaces", Justification = "This is a static class that contains extension methods for IEnumerable<T>.")]
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