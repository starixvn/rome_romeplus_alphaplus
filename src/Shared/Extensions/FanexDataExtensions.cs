using System.Diagnostics.CodeAnalysis;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

[SuppressMessage("SonarQube", "S3903:Types should be defined in named namespaces", Justification = "This is a static class that contains extension methods for IEnumerable<T>.")]
internal static class FanexDataExtensions
{
    internal static IServiceCollection AddFanexDataWithSqlServer(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddFanexSql(configuration)
        .AddSingleton<IDynamicRepository, DynamicRepository>();
}