using System.Text.Encodings.Web;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class PlatformTagHelper : TagHelper
{
    [HtmlAttributeName("device")]
    public int? Device { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("platform-block bl_underdog text-center", HtmlEncoder.Default);

        var platform = Resources.Platform_NA;
        if (Device != null)
        {
            platform = Resources.ResourceManager.GetString(string.Format("Platform_{0}", Device));
            platform = platform ?? Resources.Platform_Uncategorized;
        }

        output.Content.SetContent(platform);
    }
}