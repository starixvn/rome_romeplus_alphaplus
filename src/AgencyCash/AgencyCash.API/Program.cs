using AgencyCash.API.Payments;
using AgencyCash.API.RegisterCashMember;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddMemoryCache()
    .AddFanexSql(configuration)
    .AddSingleton<IDynamicRepository, DynamicRepository>()
    .AddSingleton<ISiteDBService, SiteDBService>()
    .AddSingleton<IRegisterCashMemberService, RegisterCashMemberService>()
    .RegisterPaymentsModule(configuration)
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Agency Cash API V1", Version = "v1" });
        c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "API Key needed to access the endpoints. ApiKey: X-API-KEY: {key}",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "X-API-KEY",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "ApiKey"
        });
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    Scheme = "ApiKey",
                    Name = "X-API-KEY",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    })
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
        c.SwaggerEndpoint("v1/swagger.json", "Agency Cash API V1"); // use relative path, no leading slash
        c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger and /swagger/index.html
    });
app.MapHealthChecks("/health");
app.MapRegisterCashMemberEndpoints();
app.MapPaymentsEndpoints();

await app.RunAsync();
