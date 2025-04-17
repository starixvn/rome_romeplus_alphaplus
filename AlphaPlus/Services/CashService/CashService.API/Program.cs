using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using AlphaPlus.CashService.API.Extensions;
using AlphaPlus.CashService.API.Services;
using AlphaPlus.CashService.API.Models.ManagePayments;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container
builder.Services
    .AddCashServiceApi()
    .AddCashServiceCors(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" })
    .AddCashServiceLogging()
    .AddDependencies(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCashServiceSwagger()
   .UseCashServiceExceptionHandler()
   .UseCashServiceCors()
   .MapAPIEndpoints();

app.Run();
