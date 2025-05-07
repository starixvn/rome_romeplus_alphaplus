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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Service API");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
app.MapHealthChecks("/health");
app.RegisterDepositEndpoints();

await app.RunAsync();
