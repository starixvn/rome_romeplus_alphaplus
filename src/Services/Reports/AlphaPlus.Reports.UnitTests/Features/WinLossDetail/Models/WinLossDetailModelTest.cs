using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

[Trait("Category", "WinLossDetail")]
public class WinLossDetailModelTest
{
    [Fact]
    public void FullName_Should_Return_Null_When_FirstName_And_LastName_Are_Empty()
    {
        // Arrange
        var model = new WinLossDetailModel
        {
            FirstName = "",
            LastName = ""
        };

        // Act
        var result = model.FullName;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FullName_Should_Return_FirstName_When_LastName_Is_Empty()
    {
        // Arrange
        var model = new WinLossDetailModel
        {
            FirstName = "John",
            LastName = ""
        };

        // Act
        var result = model.FullName;

        // Assert
        Assert.Equal("John", result);
    }

    [Fact]
    public void FullName_Should_Return_LastName_When_FirstName_Is_Empty()
    {
        // Arrange
        var model = new WinLossDetailModel
        {
            FirstName = "",
            LastName = "Doe"
        };

        // Act
        var result = model.FullName;

        // Assert
        Assert.Equal("Doe", result);
    }

    [Fact]
    public void FullName_Should_Return_Correctly_Formatted_When_Both_Names_Present()
    {
        // Arrange
        var model = new WinLossDetailModel
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var result = model.FullName;

        // Assert
        Assert.Equal("John/ Doe", result);
    }

    [Fact]
    public void Addition_Operator_Should_Combine_Two_Models_Correctly()
    {
        // Arrange
        var model1 = new WinLossDetailModel
        {
            NetTurnover = 100,
            GrossComm = 10,
            NumberOfTickets = 5
        };

        var model2 = new WinLossDetailModel
        {
            NetTurnover = 200,
            GrossComm = 20,
            NumberOfTickets = 3
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(300, result.NetTurnover);
        Assert.Equal(30, result.GrossComm);
        Assert.Equal(8, result.NumberOfTickets);
    }

    [Fact]
    public void Model_Should_Have_Expected_Properties_And_Types()
    {
        // Arrange
        var model = new WinLossDetailModel();
        var type = typeof(WinLossDetailModel);

        // Assert
        Assert.Equal(typeof(int), type.GetProperty("CustId").PropertyType);
        Assert.Equal(typeof(string), type.GetProperty("CustName").PropertyType);
        Assert.Equal(typeof(string), type.GetProperty("FirstName").PropertyType);
        Assert.Equal(typeof(string), type.GetProperty("LastName").PropertyType);
        Assert.Equal(typeof(string), type.GetProperty("FullName").PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty("IsExistedWatchlist").PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty("RewardAmount").PropertyType);
        Assert.Equal(typeof(string), type.GetProperty("RewardCurrency").PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty("NetTurnover").PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty("GrossComm").PropertyType);
        Assert.Equal(typeof(int), type.GetProperty("NumberOfTickets").PropertyType);
        Assert.Equal(typeof(bool), type.GetProperty("IsDirectMember").PropertyType);
    }
}