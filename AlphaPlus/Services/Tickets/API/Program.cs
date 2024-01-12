using System.Text.Json.Serialization;
using AlphaPlus.Tickets.API.Extensions;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddFanexDataWithSqlServer(configuration)
    .AddRemotingRef(configuration)
    .AddInfrastructure()
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
app
    .UseMiddlewares()
    .MapEndpoints();
//app.UseHttpsRedirection();

await Fanex.Logging.Logger.Log.InfoAsync($"{typeof(Program).Assembly.GetName().Name} has been started up.");
await app.RunAsync();
