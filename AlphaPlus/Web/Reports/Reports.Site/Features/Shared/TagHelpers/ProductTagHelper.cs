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
        output.TagName = null;
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();
        AddProductDropdown(sb);
        AddProductDetailDropdown(sb);

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AddProductDropdown(StringBuilder sb)
    {
        sb
            .Append("<div class=\"form-group product-multiselect-container\">")
            .Append("<select id=\"dropdown-product\" ")
            .Append($"data-hasdata=\"{Model.Products.Any()}\" ")
            .Append($"data-label-selectproducts=\"{Resources.SelectProducts}\" ")
            .Append($"data-label-selected=\"{Resources.Selected}\" ")
            .Append($"data-label-allproducts=\"{Resources.AllProducts}\" ")
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
            .Append($"<input type=\"hidden\" name=\"UserSelectedProductIds\" id=\"product-ids\" value=\"{Model.UserSelectedProductIds}\" />")
            .Append("</div>");
    }

    private void AddProductDetailDropdown(StringBuilder sb)
    {
        if (Model.ProductDetails?.Any() == true)
        {
            var isDisabled = string.IsNullOrWhiteSpace(Model.UserSelectedResourceIds);
            sb
                .Append("<div class=\"form-group product-detail-multiselect-container\">")
                .Append("<select id=\"dropdown-product-detail\" ")
                .Append($"data-hasdata=\"{Model.ProductDetails.Any()}\" ")
                .Append($"data-label-selectnone=\"{Resources.SelectNone}\" ")
                .Append($"data-label-selected=\"{Resources.Selected}\" ")
                .Append($"data-label-selectall=\"{Resources.SelectAll}\" ")
                .Append($"data-language=\"{Model.Language}\" multiple=\"multiple\" class=\"select-multiple\" {(isDisabled ? "disabled=disabled" : string.Empty)}>");
            foreach (var product in Model.ProductDetails)
            {
                sb
                    .Append($"<option value=\"{product.Id}\" {(product.Checked ? "selected=selected" : "disabled=disabled")} ")
                    .Append($"data-productid=\"{product.ProductId}\">")
                    .Append(product.Name)
                    .Append("</option>");
            }

            sb
                .Append("</select>")
                .Append($"<input type=\"hidden\" name=\"UserSelectedResourceIds\" id=\"resource-ids\" value=\"{Model.UserSelectedResourceIds}\" />")
                .Append("</div>");
        }
    }
}