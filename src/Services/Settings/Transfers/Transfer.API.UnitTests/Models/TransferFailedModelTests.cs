using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Models;

public class TransferFailedModelTests
{
    [Fact]
    public void TransferFailedModel_Should_Initialize_With_Parameters()
    {
        // Arrange
        var custIds = new List<int> { 1, 2, 3 };
        var errMsg = "Transfer failed";

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Equal(custIds, model.CustIds);
        Assert.Equal(errMsg, model.ErrMsg);
    }

    [Fact]
    public void TransferFailedModel_Should_Handle_Empty_CustIds()
    {
        // Arrange
        var custIds = new List<int>();
        var errMsg = "No customers";

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Empty(model.CustIds);
        Assert.Equal(errMsg, model.ErrMsg);
    }

    [Fact]
    public void TransferFailedModel_Should_Handle_Null_ErrMsg()
    {
        // Arrange
        var custIds = new List<int> { 123 };
        string errMsg = null;

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Equal(custIds, model.CustIds);
        Assert.Null(model.ErrMsg);
    }

    [Fact]
    public void TransferFailedModel_Should_Handle_Single_CustId()
    {
        // Arrange
        var custIds = new List<int> { 999 };
        var errMsg = "Single customer error";

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Single(model.CustIds);
        Assert.Equal(999, model.CustIds[0]);
        Assert.Equal(errMsg, model.ErrMsg);
    }

    [Fact]
    public void TransferFailedModel_Should_Have_Value_Type_Properties()
    {
        // Arrange
        var custIds = new List<int> { 1, 2 };
        var errMsg = "Test error";

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Equal(custIds, model.CustIds);
        Assert.Equal(errMsg, model.ErrMsg);
        Assert.IsType<List<int>>(model.CustIds);
    }

    [Fact]
    public void TransferFailedModel_Should_Handle_Large_CustIds_List()
    {
        // Arrange
        var custIds = Enumerable.Range(1, 1000).ToList();
        var errMsg = "Bulk operation failed";

        // Act
        var model = new TransferFailedModel(custIds, errMsg);

        // Assert
        Assert.Equal(1000, model.CustIds.Count);
        Assert.Equal(errMsg, model.ErrMsg);
    }
}
