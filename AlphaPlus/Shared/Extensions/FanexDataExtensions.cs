using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

internal static class FanexDataExtensions
{
    internal static IServiceCollection AddFanexDataWithSqlServer(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddFanexSql(configuration)
        .AddSingleton<IDynamicRepository, DynamicRepository>();
}