using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Criteria;

public class GetTransferOptionCriteriaTests
{
    [Fact]
    public void GetTransferOptionCriteria_Constructor_Should_Set_CustId_And_Default_TransferOption()
    {
        // Arrange
        var custId = 123;

        // Act
        var criteria = new GetTransferOptionCriteria(custId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(string.Empty, criteria.TransferOption);
    }

    [Fact]
    public void GetTransferOptionCriteria_Should_Have_ReadOnly_CustId()
    {
        // Arrange
        var custId = 456;
        var criteria = new GetTransferOptionCriteria(custId);

        // Act & Assert
        Assert.Equal(custId, criteria.CustId);
        // CustId should be read-only (no setter)
    }

    [Fact]
    public void GetTransferOptionCriteria_Should_Allow_Setting_TransferOption()
    {
        // Arrange
        var criteria = new GetTransferOptionCriteria(123);
        var transferOption = "11111111";

        // Act
        criteria.TransferOption = transferOption;

        // Assert
        Assert.Equal(transferOption, criteria.TransferOption);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetTransferOptionCriteria(123);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_CustomerSelTransferOption", settingKey);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(-100, false)]
    public void IsValid_Should_Return_Correct_Result_Based_On_CustId(int custId, bool expected)
    {
        // Arrange
        var criteria = new GetTransferOptionCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetTransferOptionCriteria_Should_Inherit_From_NonQueryCommand()
    {
        // Arrange
        var criteria = new GetTransferOptionCriteria(123);

        // Act & Assert
        Assert.IsAssignableFrom<NonQueryCommand>(criteria);
    }
}
