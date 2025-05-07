using AgencyCash.API.RegisterCashMember;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddFanexSql(builder.Configuration).AddSingleton<IDynamicRepository, DynamicRepository>();
builder.Services.AddSingleton<IRegisterCashMemberService, RegisterCashMemberService>();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.MapHealthChecks("/health");
app.MapGet("/", () => "Hello World!");
app.MapGet("/api", () => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
app.MapGet("/api/accounts", () => "Account list");
app.MapGet("/api/accounts/{id}", (int id) => $"Account details for {id}");
app.MapRegisterCashMemberEndpoint();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agency Cash API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.Run();
