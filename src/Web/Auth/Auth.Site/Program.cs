using AlphaPlus.Core.Authorization;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Logging;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
