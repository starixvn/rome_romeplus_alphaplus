using Alpha.Handbook.Demo.Helpers;
using Alpha.Handbook.Demo.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

ThreadPool.SetMinThreads(100, 100);

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services
    .RegisterApplicationServices(configuration)
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
Alpha.Handbook.Demo.Configurations.ConfigurationManager.Init(configuration);

app.UseForwardedHeaders();
app.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
app.UseStaticFiles(new StaticFileOptions());
app.UseRouting();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionMiddleware>();
}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();

    if (context.Request.Cookies.ContainsKey(".AspNetCore.Session"))
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    }

    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HandbookDefault}/{action=Index}/{id?}");

app.Run();
