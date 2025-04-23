using AlphaPlus.Reports.Features.BonusDetail;

namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail;

[Trait("Category", "BonusDetail")]
public class ConfirmPaymentCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var transId = 123456789L;
        var paymentDate = new DateTime(2025, 3, 17);
        var userId = 3;
        var loginIP = "127.0.0.1";
        var language = "en";

        // Act
        var criteria = new ConfirmPaymentCriteria(custId, subAccId, transId, paymentDate, userId, loginIP, language);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(transId, criteria.TransId);
        Assert.Equal(paymentDate, criteria.PaymentDate);
        Assert.Equal(userId, criteria.UserId);
        Assert.Equal(loginIP, criteria.LoginIP);
        Assert.Equal(language, criteria.Language);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new ConfirmPaymentCriteria(1, 2, 123456789L, DateTime.Now, 3, "127.0.0.1", "en");

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Bonus_ConfirmPayment", settingKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCriteriaIsValid()
    {
        // Arrange
        var criteria = new ConfirmPaymentCriteria(1, 2, 123456789L, DateTime.Now, 3, "127.0.0.1", "en");

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsZero()
    {
        // Arrange
        var criteria = new ConfirmPaymentCriteria(0, 2, 123456789L, DateTime.Now, 3, "127.0.0.1", "en");

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenTransIdIsZero()
    {
        // Arrange
        var criteria = new ConfirmPaymentCriteria(1, 2, 0, DateTime.Now, 3, "127.0.0.1", "en");

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenUserIdIsZero()
    {
        // Arrange
        var criteria = new ConfirmPaymentCriteria(1, 2, 123456789L, DateTime.Now, 0, "127.0.0.1", "en");

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
}
