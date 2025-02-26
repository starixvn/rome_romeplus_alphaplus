using System.Globalization;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Middlewares
{
    public class RequestCultureMiddleware(RequestDelegate next)
    {
        public const string English = "en-US";
        public const string Vietnamese = "vi-VN";
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(English);
        private readonly RequestDelegate next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureName = UserSession.Current.Language;

            if (string.IsNullOrEmpty(cultureName) || (AppSettings.DisabledVietnamese && cultureName == Vietnamese))
            {
                cultureName = English;
                UserSession.Current.Language = cultureName;
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            await next(context);
        }
    }
}