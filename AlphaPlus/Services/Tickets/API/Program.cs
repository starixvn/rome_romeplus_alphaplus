using System.Text.Json.Serialization;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.Tickets.API.Endpoints;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddFanexDataWithSqlServer(configuration)
    .AddRemotingRef(configuration)
    .AddInfrastructure(configuration)
    // .AddResponseCompression(options =>
    // {
    //     options.EnableForHttps = false;
    //     options.Providers.Add<BrotliCompressionProvider>();
    //     options.Providers.Add<GzipCompressionProvider>();
    //     options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
    // })
    .Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.SerializerOptions.PropertyNamingPolicy = null;
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHealthChecks();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app
    //.UseResponseCompression()
    .UseMiddleware<ExceptionMiddleware>();
app.MapHealthChecks("/health");
typeof(IEndpoint).Assembly
    .GetTypes()
    .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoint)))
    .Select(Activator.CreateInstance)
    .Cast<IEndpoint>()
    .ToList()
    .ForEach(module => module.MapEndpoints(app));

await Fanex.Logging.Logger.Log.InfoAsync($"{typeof(Program).Assembly.GetName().Name} has been started up.");
await app.RunAsync();
