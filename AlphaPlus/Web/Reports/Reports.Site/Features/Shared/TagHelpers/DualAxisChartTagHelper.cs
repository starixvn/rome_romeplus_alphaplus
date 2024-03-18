using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("dual-axis-chart")]
public class DualAxisChartTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public DualAxisChartViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "canvas";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("id", "winloss-chart");
        output.Attributes.Add("data-title", Model.Title);
        output.Attributes.Add("data-labels", Model.Labels);
        output.Attributes.Add("data-y-axis-1", Model.FirstData);
        output.Attributes.Add("data-y-axis-2", Model.SecondData);
        output.Attributes.Add("data-y-axis-1-label", Model.FirstDataLabel);
        output.Attributes.Add("data-y-axis-2-label", Model.SecondDataLabel);
        output.Attributes.Add("data-currency-name", Model.CurrencyName);
    }
}