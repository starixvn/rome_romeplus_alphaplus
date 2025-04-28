using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "ProductMultiSelectionTagHelper")]
public class ProductMultiSelectionTagHelperTest
{
    private readonly ProductMultiSelectionTagHelper tagHelper;
    private readonly TagHelperContext context;
    private readonly TagHelperOutput output;
    private readonly ProductMultiSelectionViewModel model;

    public ProductMultiSelectionTagHelperTest()
    {
        model = new ProductMultiSelectionViewModel(
            Products: new List<ProductGroupDTO>
            {
                new ProductGroupDTO(Id: 1, Name: "Product 1", Checked: true, Products: null),
                new ProductGroupDTO(
                    Id: 2, Name: "Product Group", Checked: false, Products: new List<ProductGroupDTO>
                    {
                        new ProductGroupDTO(Id: 3, Name: "Sub Product", Checked: false, Products: new List<ProductGroupDTO>())
                    }
                )
            },
            UserSelectedProductIds: "1",
            Language: "en",
            ProductDetails: new List<ProductDetailDTO>
            {
                new ProductDetailDTO(ProductId: 1, Id: 101, Name: "Detail 1", Checked: true),
                new ProductDetailDTO(ProductId: 3, Id: 102, Name: "Detail 2", Checked: false)
            },
            UserSelectedResourceIds: "101"
        );

        tagHelper = new ProductMultiSelectionTagHelper
        {
            Model = model
        };

        context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString());

        output = new TagHelperOutput(
            "product-multiselection",
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
    public void Process_Should_Render_Product_Dropdown()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains(Resources.Product, content);
        Assert.Contains("dropdown-product", content);
        Assert.Contains("data-label-selectall", content);
        Assert.Contains("data-label-selected", content);
        Assert.Contains("data-label-allproducts", content);
        Assert.Contains("data-label-nodata", content);
        Assert.Contains("Product 1", content);
        Assert.Contains("Product Group", content);
        Assert.Contains("Sub Product", content);
        Assert.Contains("selected=\"selected\"", content);
        Assert.Contains("data-isnested=\"1\"", content);
        Assert.Contains("<input type=\"hidden\" name=\"UserSelectedProductIds\" id=\"product-ids\" value=\"1\" />", content);
    }

    [Fact]
    public void Process_Should_Render_ProductDetail_Dropdown_When_ProductDetails_Exist()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains(Resources.ProductDetails, content);
        Assert.Contains("dropdown-product-detail", content);
        Assert.Contains("Detail 1", content);
        Assert.Contains("Detail 2", content);
        Assert.Contains("data-productid=\"1\"", content);
        Assert.Contains("<input type=\"hidden\" name=\"UserSelectedResourceIds\" id=\"resource-ids\" value=\"101\" />", content);
    }

    [Fact]
    public void Process_Should_Not_Render_ProductDetail_Dropdown_When_ProductDetails_Do_Not_Exist()
    {
        // Arrange
        tagHelper.Model = new ProductMultiSelectionViewModel(
            Products: new List<ProductGroupDTO>
            {
                new ProductGroupDTO(Id: 1, Name: "Product 1", Checked: true, Products: new List<ProductGroupDTO>()),
                new ProductGroupDTO(
                    Id: 2, Name: "Product Group", Checked: false, Products: new List<ProductGroupDTO>
                    {
                        new ProductGroupDTO(Id: 3, Name: "Sub Product", Checked: false, Products: new List<ProductGroupDTO>())
                    }
                )
            },
            UserSelectedProductIds: "1",
            Language: "en"
        );

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain("dropdown-product-detail", content);
        Assert.DoesNotContain(Resources.ProductDetails, content);
    }

    [Fact]
    public void Process_Should_Render_ProductDetail_Dropdown_As_Disabled_When_No_UserSelectedResourceIds()
    {
        // Arrange
        tagHelper.Model = new ProductMultiSelectionViewModel(
            Products: new List<ProductGroupDTO>
            {
                new ProductGroupDTO(Id: 1, Name: "Product 1", Checked: true, Products: new List<ProductGroupDTO>()),
                new ProductGroupDTO(
                    Id: 2, Name: "Product Group", Checked: false, Products: new List<ProductGroupDTO>
                    {
                        new ProductGroupDTO(Id: 3, Name: "Sub Product", Checked: false, Products: new List<ProductGroupDTO>())
                    }
                )
            },
            UserSelectedProductIds: "1",
            Language: "en",
            ProductDetails: new List<ProductDetailDTO>
            {
                new ProductDetailDTO(ProductId: 1, Id: 101, Name: "Detail 1", Checked: true),
                new ProductDetailDTO(ProductId: 3, Id: 102, Name: "Detail 2", Checked: false)
            },
            UserSelectedResourceIds: ""
        );

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("disabled=disabled", content);
    }

    [Fact]
    public void Process_Should_Render_Correct_Product_Options()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();

        Assert.Contains("<option value=\"1\" selected=\"selected\">Product 1</option>", content);
        Assert.Contains("<option value=\"3\" data-isnested=\"1\">Sub Product</option>", content);
        Assert.Contains("<optgroup label=\"Product Group\">", content);
    }

    [Fact]
    public void Process_Should_Render_Correct_ProductDetail_Options()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<option value=\"101\" selected=selected data-productid=\"1\">Detail 1</option>", content);
        Assert.Contains("<option value=\"102\" disabled=disabled data-productid=\"3\">Detail 2</option>", content);
    }
}