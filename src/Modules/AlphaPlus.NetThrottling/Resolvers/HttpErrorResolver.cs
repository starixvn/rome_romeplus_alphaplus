using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public class HttpErrorResolver : BaseResolver
    {
        public override async Task BlockNonAjaxRequest(HttpContext context)
        {
            var darkModeName = UserSession.Current.DarkModeName;
            context.Response.StatusCode = 429;
            context.Response.ContentType = "text/html";            
            await context.Response.WriteAsync(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""utf-8"">
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>Index</title>
                <link href=""/auth/assets/bundles/error" + @darkModeName + @".css"" rel=""stylesheet"" type=""text/css"" />
            </head>
            <body>
                <div class=""container"">
                    <img src=""/auth/assets/error/images/error-bg" + @darkModeName + @".png"" title="""" alt="""" class=""img"" />
                    <p class=""caption"">
                        Too many requests
                    </p>
                </div>
            </body>
            </html>");
        }
    }
}