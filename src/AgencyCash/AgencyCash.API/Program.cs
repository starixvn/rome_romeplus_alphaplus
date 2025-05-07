using AgencyCash.API.RegisterCashMember;
using AgencyCash.API.Shared;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddFanexSql(configuration)
    .AddSingleton<IDynamicRepository, DynamicRepository>()
    .AddSingleton<IRegisterCashMemberService, RegisterCashMemberService>()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddHealthChecks();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}

app
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agency Cash API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
app.MapHealthChecks("/health");
app.MapRegisterCashMemberEndpoint();

await app.RunAsync();
