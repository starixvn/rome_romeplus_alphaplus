using Alpha.Main.Site;
using Microsoft.AspNetCore;

var app = BuildWebHost(args);

app.Run();

static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
