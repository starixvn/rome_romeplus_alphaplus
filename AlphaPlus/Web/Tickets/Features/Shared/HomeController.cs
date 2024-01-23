using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public class HomeController : Controller
{
    private readonly Fanex.Logging.ILogger logger;

    public HomeController(Fanex.Logging.ILogger logger)
        => this.logger = logger;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature != null)
        {
            var exception = exceptionHandlerFeature.Error;
            await logger.ErrorAsync(await ExceptionHelper.BuildError(HttpContext.Request, exception, exceptionHandlerFeature.Path));
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
