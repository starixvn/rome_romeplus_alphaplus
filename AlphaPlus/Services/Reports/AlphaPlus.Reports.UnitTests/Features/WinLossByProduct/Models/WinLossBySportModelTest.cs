using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.WinLossByProduct;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossByProduct;

[Trait("Category", "WinLossByProduct")]
public class WinLossBySportModelTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int sportType = 1;
        string sportTypeName = "Football";
        var winLossByBetTypes = new List<WinLossByBetTypeModel>
        {
            new WinLossByBetTypeModel(),
            new WinLossByBetTypeModel()
        };
        var total = new WinLossByBetTypeModel();

        // Act
        var model = new WinLossBySportModel(
            sportType,
            sportTypeName,
            winLossByBetTypes,
            total);

        // Assert
        Assert.Equal(sportType, model.SportType);
        Assert.Equal(sportTypeName, model.SportTypeName);
        Assert.Same(winLossByBetTypes, model.WinLossByBetTypes);
        Assert.Same(total, model.Total);
    }

    [Theory]
    [InlineData(ProductIdConstants.VirtualSports, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(100, true)]
    public void IsShowBetTypeDirectly_Should_Return_Expected_Result(int productId, bool expected)
    {
        // Act
        bool result = WinLossBySportModel.IsShowBetTypeDirectly(productId);

        // Assert
        Assert.Equal(expected, result);
    }
}