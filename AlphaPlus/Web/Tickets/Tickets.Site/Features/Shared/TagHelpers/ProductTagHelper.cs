using System.Text;
using System.Text.Encodings.Web;
using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public class ProductTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public ProductMultiSelectionViewModel Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("form-group", HtmlEncoder.Default);

        output.Content.SetHtmlContent(RenderProductDropdown());
    }

    private string RenderProductDropdown()
    {
        var sb = new StringBuilder();
        sb
            .Append("<select id=\"dropdown-product\" ")
            .Append($"data-hasdata=\"{Model.Products.Any().ToString().ToLowerInvariant()}\" ")
            .Append($"data-label-selectproducts=\"{Resources.SelectProducts}\" ")
            .Append($"data-label-selected=\"{Resources.Selected}\" ")
            .Append($"data-label-allproducts=\"{Resources.AllProducts}\" ")
            .Append($"data-label-selectall=\"{Resources.SelectAll}\" ")
            .Append($"data-label-nodata=\"{Resources.ThereIsNoProduct}\" ")
            .Append($"data-language=\"{Model.Language}\" multiple=\"multiple\" class=\"select-multiple\">");
        foreach (var product in Model.Products)
        {
            if (product.Products == null)
            {
                sb
                   .Append($"<option value=\"{product.Id}\"{(product.Checked ? " selected=\"selected\"" : string.Empty)}>")
                   .Append(product.Name)
                   .Append("</option>");
            }
            else
            {
                sb.Append($"<optgroup label=\"{product.Name}\">");
                foreach (var subProduct in product.Products)
                {
                    sb
                        .Append($"<option value=\"{subProduct.Id}\"{(subProduct.Checked ? " selected=\"selected\"" : string.Empty)} data-isnested=\"1\">")
                        .Append(subProduct.Name)
                        .Append("</option>");
                }

                sb.Append("</optgroup>");
            }
        }

        sb
            .Append("</select>")
            .Append($"<input type=\"hidden\" name=\"ProductIds\" id=\"product-ids\" value=\"{Model.UserSelectedProductIds}\" />");

        return sb.ToString();
    }
}