using System.Text;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.Features.Shared.TagHelpers;

public class ProductTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public ProductMultiSelectionViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.Add("class", "form-group product-multiselect-container");

        var sb = new StringBuilder();
        sb
            .Append("<select id=\"dropdown-product\" ")
            .Append($"data-has-any-product=\"{Model.Products.Any()}\" ")
            .Append($"data-label-select-products=\"{Resources.SelectProducts}\" ")
            .Append($"data-label-selected=\"{Resources.Selected}\" ")
            .Append($"data-label-all-products=\"{Resources.AllProducts}\" ")
            .Append($"data-label-there-is-no-product=\"{Resources.ThereIsNoProduct}\" ")
            .Append($"data-language=\"{Model.Language}\" multiple=\"multiple\" class=\"select-multiple\">");

        var groupProducts = Model.Products.GroupBy(p => p.Group?.Name);
        foreach (var group in groupProducts)
        {
            if (group.Key == null)
            {
                foreach (var product in group)
                {
                    sb
                        .Append($"<option value=\"{product.Value}\"{(product.Selected ? " selected=\"selected\"" : string.Empty)}>")
                        .Append(product.Text)
                        .Append("</option>");
                }
            }
            else
            {
                sb.Append($"<optgroup label=\"{group.Key}\">");
                foreach (var product in group)
                {
                    sb
                        .Append($"<option value=\"{product.Value}\"{(product.Selected ? " selected=\"selected\"" : string.Empty)} data-isnested=\"1\">")
                        .Append(product.Text)
                        .Append("</option>");
                }

                sb.Append("</optgroup>");
            }
        }

        sb
            .Append("</select>")
            .Append($"<input type=\"hidden\" name=\"UserSelectedProductIds\" id=\"product-ids\" value=\"{Model.UserSelectedProductIds}\" />");

        output.Content.SetHtmlContent(sb.ToString());
    }
}