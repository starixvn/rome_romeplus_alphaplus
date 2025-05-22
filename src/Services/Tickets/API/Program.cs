using System.Text.Json.Serialization;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.Tickets.API.Extensions;
using AlphaPlus.Tickets.API.Middlewares;
using Microsoft.AspNetCore.Http.Json;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddFanexDataWithSqlServer(configuration)
    .AddRemotingRef(configuration)
    .AddInfrastructure(configuration)
    .AddMemoryCache()
    // .AddResponseCompression(options =>
    // {
    //     options.EnableForHttps = false;
    //     options.Providers.Add<BrotliCompressionProvider>();
    //     options.Providers.Add<GzipCompressionProvider>();
    //     options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
    // })
    .AddEndpoints(assembly)
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
app.MapEndpoints();
app.UseDeveloperExceptionPage();
// await Fanex.Logging.Logger.Log.InfoAsync($"{assembly.GetName().Name} has been started up.");
await app.RunAsync();
