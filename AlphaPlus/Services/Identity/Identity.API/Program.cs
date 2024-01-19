using AlphaPlus.Identity.API.Features.Auth;
using AlphaPlus.Identity.API.Utils;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddFanexDataWithSqlServer(configuration);

builder.Services
    .AddSingleton<IEncryption, Encryption>()
    .AddSingleton<IAuthenticateService, AuthenticateService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapAuthenticateEndpoints();

app.Run();
