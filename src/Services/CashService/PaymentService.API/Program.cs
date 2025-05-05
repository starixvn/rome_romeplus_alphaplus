using AlphaPlus.PaymentService.API.Deposit;
using AlphaPlus.PaymentService.API.Shared;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services
    .AddFanexLogging(configuration)
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHealthChecks();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app
        .UseDeveloperExceptionPage()
        .UseSwagger()
        .UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.MapHealthChecks("/health").AllowAnonymous();
app.UseMiddleware<ApiKeyMiddleware>();
app.RegisterDepositEndpoints();

await app.RunAsync();
