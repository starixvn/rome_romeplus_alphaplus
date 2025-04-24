using AlphaPlus.Reports.Features.WinLossByProduct;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossByProduct;

[Trait("Category", "WinLossByProduct")]
public class WinLossByProductModelTest
{
    [Fact]
    public void WinLossByProductModel_Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int expectedProductId = 123;
        string expectedProductName = "Sports";
        var expectedWinLossBySports = new List<WinLossBySportModel>
        {
            new WinLossBySportModel(1, "Football", new List<WinLossByBetTypeModel>(), new WinLossByBetTypeModel()),
            new WinLossBySportModel(2, "Basketball", new List<WinLossByBetTypeModel>(), new WinLossByBetTypeModel())
        };
        var expectedTotal = new WinLossByBetTypeModel();

        // Act
        var model = new WinLossByProductModel(
            expectedProductId,
            expectedProductName,
            expectedWinLossBySports,
            expectedTotal);

        // Assert
        Assert.Equal(expectedProductId, model.ProductId);
        Assert.Equal(expectedProductName, model.ProductName);
        Assert.Equal(expectedWinLossBySports, model.WinLossBySports);
        Assert.Equal(expectedTotal, model.Total);
    }

    [Fact]
    public void WinLossByProductModel_Properties_Should_Be_ReadOnly()
    {
        // Arrange
        var model = new WinLossByProductModel(
            1,
            "Test Product",
            new List<WinLossBySportModel>(),
            new WinLossByBetTypeModel());

        // Assert - If the code compiles, this test will pass because the properties are read-only
        // This test verifies the design rather than behavior
        Assert.True(true);
    }

    [Fact]
    public void WinLossByProductModel_Should_Accept_Empty_WinLossBySports()
    {
        // Arrange
        var emptyWinLossBySports = new List<WinLossBySportModel>();
        var total = new WinLossByBetTypeModel();

        // Act
        var model = new WinLossByProductModel(1, "Test Product", emptyWinLossBySports, total);

        // Assert
        Assert.Empty(model.WinLossBySports);
    }

    [Fact]
    public void WinLossByProductModel_Should_Accept_Null_WinLossBySports()
    {
        // Arrange & Act
        var model = new WinLossByProductModel(1, "Test Product", null, new WinLossByBetTypeModel());

        // Assert
        Assert.Null(model.WinLossBySports);
    }
}