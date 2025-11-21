using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Models;

public class TransferModelTests
{
    [Fact]
    public void TransferModel_Should_Initialize_With_Default_Value()
    {
        // Act
        var model = new TransferModel();

        // Assert
        Assert.False(model.IsAllDayBalTransfer);
    }

    [Fact]
    public void TransferModel_Should_Set_IsAllDayBalTransfer_Property()
    {
        // Act
        var model = new TransferModel
        {
            IsAllDayBalTransfer = true
        };

        // Assert
        Assert.True(model.IsAllDayBalTransfer);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TransferModel_Should_Handle_Both_Boolean_Values(bool value)
    {
        // Act
        var model = new TransferModel
        {
            IsAllDayBalTransfer = value
        };

        // Assert
        Assert.Equal(value, model.IsAllDayBalTransfer);
    }
}
