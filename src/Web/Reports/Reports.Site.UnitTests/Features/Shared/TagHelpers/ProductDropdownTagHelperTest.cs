using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "ProductDropdownTagHelper")]
public class ProductDropdownTagHelperTest
{
    private readonly ProductDropdownTagHelper tagHelper;
    private readonly TagHelperContext context;
    private readonly TagHelperOutput output;

    public ProductDropdownTagHelperTest()
    {
        tagHelper = new ProductDropdownTagHelper();
        context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString());

        output = new TagHelperOutput(
            "product-dropdown",
            new TagHelperAttributeList(),
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
    }

    [Fact]
    public void Process_Should_Set_TagName_To_Null()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Null(output.TagName);
    }

    [Fact]
    public void Process_Should_Set_TagMode_To_StartTagAndEndTag()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Render_Product_Dropdown_Without_ProductDetails_When_ShowProductDetails_Is_False()
    {
        // Arrange
        tagHelper.ShowProductDetails = false;

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains(Resources.Product, content);
        Assert.Contains("dropdown-product", content);
        Assert.Contains(Resources.SelectAll, content);
        Assert.Contains(Resources.nSelected, content);
        Assert.Contains(Resources.NoneSelected, content);
        Assert.Contains(Resources.All, content);
        Assert.Contains(Resources.NoProductFound, content);
        Assert.Contains(Resources.SearchProduct, content);
        Assert.DoesNotContain("dropdown-product-detail", content);
        Assert.DoesNotContain(Resources.ProductDetails, content);
    }

    [Fact]
    public void Process_Should_Render_Product_And_ProductDetails_Dropdowns_When_ShowProductDetails_Is_True()
    {
        // Arrange
        tagHelper.ShowProductDetails = true;

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains(Resources.Product, content);
        Assert.Contains("dropdown-product", content);
        Assert.Contains(Resources.ProductDetails, content);
        Assert.Contains("dropdown-product-detail", content);
        Assert.Contains(Resources.NoProductDetailsFound, content);
        Assert.Contains(Resources.SearchProductDetails, content);
    }

    [Fact]
    public void Process_Should_Include_Correct_Labels_And_Data_Attributes()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains($"data-label-selectall=\"{Resources.SelectAll}\"", content);
        Assert.Contains($"data-label-selected=\"{Resources.nSelected}\"", content);
        Assert.Contains($"data-label-noneselected=\"{Resources.NoneSelected}\"", content);
        Assert.Contains($"data-label-all=\"{Resources.All}\"", content);
        Assert.Contains($"data-label-noitemfound=\"{Resources.NoProductFound}\"", content);
        Assert.Contains($"data-label-filterplaceholder=\"{Resources.SearchProduct}\"", content);
    }
}