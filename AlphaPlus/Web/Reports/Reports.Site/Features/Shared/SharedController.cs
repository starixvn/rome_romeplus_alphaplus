using AlphaPlus.Core.Filters;
using AlphaPlus.Core.Helpers;
using AlphaPlus.PartialUM.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared;

[AllowProtectedMode]
public class SharedController(Fanex.Logging.ILogger logger) : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        HttpContext.Request.EnableBuffering();
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature != null)
        {
            var exception = exceptionHandlerFeature.Error;

            if (exception is PartialUMException partialUMException)
            {
                return Redirect(MicroSites.MainUrl($"~/partialum/index?umfrom={partialUMException.StartTime}&umto={partialUMException.EndTime}"));
            }

            await logger.ErrorAsync(await ExceptionHelper.BuildError(HttpContext.Request, exception, exceptionHandlerFeature.Path));
        }

        return Redirect($"{MicroSites.MainUrl("~")}/error/index");
    }

    public IActionResult InvalidReport() => View();
}
