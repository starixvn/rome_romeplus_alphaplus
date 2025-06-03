using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.UnitTests.Features.Products;

[Trait("Category", "Products")]
public class ProductDetailTest
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var productDetail = new ProductDetail();

        // Assert
        Assert.NotNull(productDetail);
    }

    [Fact]
    public void Properties_ShouldSetAndGetValues()
    {
        // Arrange
        var productDetail = new ProductDetail
        {
            RefId = "REF123",
            ResourceId = 1001,
            ProductId = 2001,
            SportType = 1,
            BetType = 2,
            Order = 5,
            DisplayName = "Test Product",
            LeagueGroupId = 10,
            TicketType = 3,
            ChildResourceId = 1002
        };

        // Assert
        Assert.Equal("REF123", productDetail.RefId);
        Assert.Equal(1001, productDetail.ResourceId);
        Assert.Equal(2001, productDetail.ProductId);
        Assert.Equal(1, productDetail.SportType);
        Assert.Equal(2, productDetail.BetType);
        Assert.Equal(5, productDetail.Order);
        Assert.Equal("Test Product", productDetail.DisplayName);
        Assert.Equal(10, productDetail.LeagueGroupId);
        Assert.Equal(3, productDetail.TicketType);
        Assert.Equal(1002, productDetail.ChildResourceId);
    }

    [Fact]
    public void ToXml_ShouldReturnCorrectXmlFormat()
    {
        // Arrange
        var productDetail = new ProductDetail
        {
            ProductId = 2001,
            SportType = 1,
            BetType = 2,
            LeagueGroupId = 10,
            TicketType = 3
        };

        // Act
        var result = productDetail.ToXml();

        // Assert
        var expected = "<ProductDetail ProductId=\"2001\" Sporttype=\"1\" Bettype=\"2\" LeagueGroupId=\"10\" TicketType=\"3\"/>";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToXml_WithDifferentValues_ShouldReturnCorrectXmlFormat()
    {
        // Arrange
        var productDetail = new ProductDetail
        {
            ProductId = 3000,
            SportType = 5,
            BetType = 7,
            LeagueGroupId = 15,
            TicketType = 1
        };

        // Act
        var result = productDetail.ToXml();

        // Assert
        var expected = "<ProductDetail ProductId=\"3000\" Sporttype=\"5\" Bettype=\"7\" LeagueGroupId=\"15\" TicketType=\"1\"/>";
        Assert.Equal(expected, result);
    }
}