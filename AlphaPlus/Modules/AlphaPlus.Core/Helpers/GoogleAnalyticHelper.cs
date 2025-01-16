using AlphaPlus.Core.Configurations;
using Microsoft.AspNetCore.Html;

namespace AlphaPlus.Core.Helpers;

public static class GoogleAnalyticHelper
{
    private static readonly HtmlString EmptyHtml = new(string.Empty);

    public static HtmlString Render()
    {
        return Render(AppSettings.GoogleAnalyticId);
    }

    public static HtmlString Render(string googleAnalyticId)
    {
        if (string.IsNullOrEmpty(googleAnalyticId))
        {
            return EmptyHtml;
        }

        return new HtmlString(@"<script>
                                      setTimeout(function(){
                                          (function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
                                          (i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
                                          m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
                                          }})(window,document,'script','" + AppSettings.GoogleAnalyticUrl + @"','ga');

                                          ga('create', '" + googleAnalyticId + @"', 'auto');
                                          ga('send', 'pageview');
                                      },200);
                                    </script>");
    }

    public static HtmlString RenderGtagScript(string gtagId)
    {
        if (string.IsNullOrEmpty(gtagId))
        {
            return EmptyHtml;
        }

        return new HtmlString(@"<script async src='https://www.googletagmanager.com/gtag/js?id=" + gtagId + "'></script>" +
                                    @"<script>
                                    window.dataLayer = window.dataLayer || [];
                                    function gtag(){dataLayer.push(arguments);}
                                    gtag('js', new Date());
                                    gtag('config', '" + gtagId + "');" +
                                    @"</script>");
    }

    public static HtmlString RenderGtagNoScript(string gtagId)
    {
        if (string.IsNullOrEmpty(gtagId))
        {
            return EmptyHtml;
        }

        return new HtmlString(@"<noscript><iframe src='https://www.googletagmanager.com/ns.html?id=" + gtagId +
                                "' height='0' width='0' style='display:none;visibility:hidden'></iframe></noscript>");
    }

    public static HtmlString GtagScript()
    {
        return RenderGtagScript(AppSettings.GoogleAnalyticId);
    }

    public static HtmlString GtagNoScript()
    {
        return RenderGtagNoScript(AppSettings.GoogleAnalyticId);
    }
}
