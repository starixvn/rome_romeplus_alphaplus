using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Microsoft.AspNetCore.Diagnostics;
using AlphaPlus.Core.Helpers;
using AlphaPlus.PartialUM.Exceptions;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public class SharedController(Fanex.Logging.ILogger logger) : Controller
{
    private readonly Fanex.Logging.ILogger logger = logger;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        ErrorViewModel viewModel = null;
        HttpContext.Request.EnableBuffering();
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature != null)
        {
            var exception = exceptionHandlerFeature.Error;

            if (exception is PartialUMException partialUMException)
            {
                viewModel = new(true, "Partial UM");
            }
            else
            {
                viewModel = new(false, exception.Message);
                await logger.ErrorAsync(await ExceptionHelper.BuildError(HttpContext.Request, exception, exceptionHandlerFeature.Path));
            }
        }

        return View("~/Features/Shared/Views/Error.cshtml", viewModel);
    }

    public IActionResult Invalid() => View("~/Features/Shared/Views/Invalid.cshtml");
}
