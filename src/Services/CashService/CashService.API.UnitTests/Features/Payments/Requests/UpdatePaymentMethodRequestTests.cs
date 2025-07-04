using AlphaPlus.CashService.API.Features.Payments.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Requests;

public class UpdatePaymentMethodRequestTests
{
    [Fact]
    public void IsValid_WhenValidData_ReturnsTrue()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenInvalidActorId_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 0,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenInvalidProviderId_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 0,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenActorIdIsNegative_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = -1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenProviderIdIsNegative_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = -1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenBothIdsAreZero_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 0,
            ProviderId = 0,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "test",
            MerchantID = "test",
            AppSecret = "test",
            APIResult = true
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenEmptyStrings_ReturnsTrue()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = string.Empty,
            MerchantID = string.Empty,
            AppSecret = string.Empty,
            APIResult = false
        };

        // Act
        var result = request.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetAPIConfigJson_WhenAllPropertiesAreSet_ReturnsValidJson()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            APIMethodKey = "test-key",
            MerchantID = "test-merchant",
            AppSecret = "test-secret"
        };

        // Act
        var result = request.GetAPIConfigJson();

        // Assert
        var expected = "{\"APIMethodKey\":\"test-key\",\"MerchantID\":\"test-merchant\",\"AppSecret\":\"test-secret\"}";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetAPIConfigJson_WhenPropertiesAreEmpty_ReturnsJsonWithEmptyStrings()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            APIMethodKey = "",
            MerchantID = "",
            AppSecret = ""
        };

        // Act
        var result = request.GetAPIConfigJson();

        // Assert
        var expected = "{\"APIMethodKey\":\"\",\"MerchantID\":\"\",\"AppSecret\":\"\"}";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetAPIConfigJson_WhenPropertiesContainSpecialCharacters_ReturnsEscapedJson()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            APIMethodKey = "key\"with\"quotes",
            MerchantID = "merchant\\with\\backslashes",
            AppSecret = "secret\nwith\nnewlines"
        };

        // Act
        var result = request.GetAPIConfigJson();

        // Assert
        var expected = "{\"APIMethodKey\":\"key\"with\"quotes\",\"MerchantID\":\"merchant\\with\\backslashes\",\"AppSecret\":\"secret\nwith\nnewlines\"}";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange & Act
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 123,
            SubAccId = 456,
            IP = "192.168.1.1",
            Language = "en-US",
            UserId = 789,
            SiteDBGroupId = 101,
            CustDBGroupId = 202,
            ProviderId = 303,
            TransactionType = 1,
            Method = 2,
            APIMethodKey = "test-key",
            MerchantID = "test-merchant",
            AppSecret = "test-secret",
            APIResult = true
        };

        // Assert
        Assert.Equal(123, request.ActorId);
        Assert.Equal(456, request.SubAccId);
        Assert.Equal("192.168.1.1", request.IP);
        Assert.Equal("en-US", request.Language);
        Assert.Equal(789, request.UserId);
        Assert.Equal(101, request.SiteDBGroupId);
        Assert.Equal(202, request.CustDBGroupId);
        Assert.Equal(303, request.ProviderId);
        Assert.Equal(1, request.TransactionType);
        Assert.Equal(2, request.Method);
        Assert.Equal("test-key", request.APIMethodKey);
        Assert.Equal("test-merchant", request.MerchantID);
        Assert.Equal("test-secret", request.AppSecret);
        Assert.True(request.APIResult);
    }

    [Fact]
    public void APIResult_WhenSetToTrue_ReturnsTrue()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            APIResult = true
        };

        // Act & Assert
        Assert.True(request.APIResult);
    }

    [Fact]
    public void APIResult_WhenSetToFalse_ReturnsFalse()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            APIResult = false
        };

        // Act & Assert
        Assert.False(request.APIResult);
    }
} 