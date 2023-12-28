using Alpha.Tickets.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();
app
    .RegisterMiddlewares()
    .RegisterEndpoints(builder.Services);
//app.UseHttpsRedirection();

await Fanex.Logging.Logger.Log.InfoAsync($"{typeof(Program).Assembly.GetName().Name} has been started up.");
await app.RunAsync();
