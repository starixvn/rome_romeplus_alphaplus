var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health");
app.MapGet("/", () => "Hello World!");
app.MapGet("/api", () => "Hello API!");
app.MapGet("/api/accounts", () => "Account list");
app.MapGet("/api/accounts/{id}", (int id) => $"Account details for {id}");

app.Run();
