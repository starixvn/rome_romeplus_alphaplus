using AlphaPlus.CashService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container
builder.Services
    .AddCashServiceApi()
    .AddCashServiceCors(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["*"])
    .AddCashServiceLogging()
    .AddDependencies(configuration);

var app = builder.Build();
var p = Fanex.Data.Cryptography.EncryptPassword("1F9aTn0BydQik5ne");
// Configure the HTTP request pipeline
app.UseCashServiceSwagger()
    .UseCashServiceExceptionHandler()
    .UseCashServiceCors()
    .MapApiEndpoints();

await app.RunAsync();