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
        output.Attributes.Add("class", "platform-block bl_underdog text-center");
        output.Content.SetContent(PlatformHelper.GetPlatform(Device));
    }
}