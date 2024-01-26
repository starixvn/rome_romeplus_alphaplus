using AlphaPlus.Identity.API;
using AlphaPlus.Identity.API.Extensions;
using AlphaPlus.Identity.API.Features.Auth;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services
    .AddFanexDataWithSqlServer(configuration)
    .AddFanexLogging(configuration)
    .Configure<AppSettings>(configuration)
    .AddOptions()
    .AddApplicationServices();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapAuthEndpoints();

app.Run();
