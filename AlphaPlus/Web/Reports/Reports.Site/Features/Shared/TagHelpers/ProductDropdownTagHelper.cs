using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

[HtmlTargetElement("product-dropdown")]
public class ProductDropdownTagHelper : TagHelper
{
    [HtmlAttributeName("show-product-details")]
    public bool ShowProductDetails { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();
        sb
            .Append("<div class=\"form-group\">")
            .Append($"<label class=\"label-form-control\">{Resources.Product}</label>")
            .Append("<div id=\"dropdown-product\"")
            .Append($"data-label-selectall=\"{Resources.SelectAll}\" ")
            .Append($"data-label-selected=\"{Resources.nSelected}\" ")
            .Append($"data-label-noneselected=\"{Resources.NoneSelected}\" ")
            .Append($"data-label-all=\"{Resources.All}\" ")
            .Append($"data-label-noitemfound=\"{Resources.NoProductFound}\" ")
            .Append($"data-label-filterplaceholder=\"{Resources.SearchProduct}\"></div></div>");

        if (ShowProductDetails)
        {
            sb
                .Append("<div class=\"form-group\">")
                .Append($"<label class=\"label-form-control\">{Resources.ProductDetails}</label>")
                .Append("<div id=\"dropdown-product-detail\"")
                .Append($"data-label-selectall=\"{Resources.SelectAll}\" ")
                .Append($"data-label-selected=\"{Resources.nSelected}\" ")
                .Append($"data-label-noneselected=\"{Resources.NoneSelected}\" ")
                .Append($"data-label-all=\"{Resources.All}\" ")
                .Append($"data-label-noitemfound=\"{Resources.NoProductDetailsFound}\" ")
                .Append($"data-label-filterplaceholder=\"{Resources.SearchProductDetails}\"></div></div>");
        }

        output.Content.SetHtmlContent(sb.ToString());
    }
}