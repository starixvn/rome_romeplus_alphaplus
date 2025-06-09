using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Payments.Criteria;

[Trait("Category", "Payments")]
public class ValidatePaymentCriteriaTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        int agentId = 123;
        int custId = 456;
        byte transType = 1;
        short custDBGroupId = 10;
        string paymentFrom = "source";
        bool isManual = true;
        short providerId = 20;
        short paymentMethodId = 30;
        string paymentTo = "destination";

        // Act
        var criteria = new ValidatePaymentCriteria(
            agentId, custId, transType, custDBGroupId, paymentFrom,
            isManual, providerId, paymentMethodId, paymentTo);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transType, criteria.TransType);
        Assert.Equal(paymentFrom, criteria.PaymentFrom);
        Assert.Equal(isManual, criteria.IsManual);
        Assert.Equal(providerId, criteria.ProviderId);
        Assert.Equal(paymentMethodId, criteria.PaymentMethodId);
        Assert.Equal(paymentTo, criteria.PaymentTo);
    }

    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetDefaultsCorrectly()
    {
        // Arrange
        int agentId = 123;
        int custId = 456;
        byte transType = 1;
        short custDBGroupId = 10;

        // Act
        var criteria = new ValidatePaymentCriteria(agentId, custId, transType, custDBGroupId);

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transType, criteria.TransType);
        Assert.Null(criteria.PaymentFrom);
        Assert.False(criteria.IsManual);
        Assert.Equal((short)0, criteria.ProviderId);
        Assert.Equal((short)0, criteria.PaymentMethodId);
        Assert.Null(criteria.PaymentTo);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectSPName()
    {
        // Arrange
        short custDBGroupId = 10;
        var criteria = new ValidatePaymentCriteria(1, 2, 3, custDBGroupId);
        string expectedKey = SPNameHelper.CustDBPrefix("API_CashService_CustPaymentValidate", custDBGroupId);

        // Act
        string actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    public void IsValid_ShouldValidateCorrectly(int agentId, int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new ValidatePaymentCriteria(agentId, custId, 1, 1);

        // Act
        bool isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void ErrorProperties_ShouldBeSettable()
    {
        // Arrange
        var criteria = new ValidatePaymentCriteria(1, 1, 1, 1);
        int expectedErrCode = 123;
        string expectedErrMsg = "Error message";
        string expectedErrExtraInfo = "Extra info";

        // Act
        criteria.ErrCode = expectedErrCode;
        criteria.ErrMsg = expectedErrMsg;
        criteria.ErrExtraInfo = expectedErrExtraInfo;

        // Assert
        Assert.Equal(expectedErrCode, criteria.ErrCode);
        Assert.Equal(expectedErrMsg, criteria.ErrMsg);
        Assert.Equal(expectedErrExtraInfo, criteria.ErrExtraInfo);
    }
}