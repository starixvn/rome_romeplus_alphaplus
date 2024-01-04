using System.Globalization;
using AlphaPlus.Tickets.Site.Features.Shared.Helpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.Middlewares;

public sealed class RequestCultureMiddleware
{
    private readonly RequestDelegate next;

    public RequestCultureMiddleware(RequestDelegate next)
        => this.next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var language = context.Request.Cookies["language"]; //?? AppSession.Language;
        language = LanguageHelper.IsValidLanguage(language) ? language : LanguageHelper.DefaultLanguage;

        if (!language.Equals(Thread.CurrentThread.CurrentCulture.Name))
        {
            //AppSession.Language = language;
            var cultureInfo = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }
}