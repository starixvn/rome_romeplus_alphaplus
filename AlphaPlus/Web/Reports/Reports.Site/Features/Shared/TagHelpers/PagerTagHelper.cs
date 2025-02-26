using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

public class PagerTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public PagerViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model.TotalRecords == 0)
        {
            output.TagName = null;
        }
        else
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", "report-pager");
            output.Attributes.Add("data-labelpage", Resources.Page);
            output.Attributes.Add("data-labelof", Resources.of);
            output.Attributes.Add("data-labeldisplayitems", Resources.Pager_DisplayItems);
            output.Attributes.Add("data-pageindex", Model.PageIndex);
            output.Attributes.Add("data-pagesize", Model.PageSize);
            output.Attributes.Add("data-totalrecords", Model.TotalRecords);
        }
    }
}
