using AlphaPlus.Reports.Features.BonusDetail;

namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail;

[Trait("Category", "BonusDetail")]
public class BonusEntityTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var createdDate = new DateTime(2025, 3, 17);
        var transId = 123456789L;
        var masterName = "Master";
        var agentName = "Agent";
        var custId = 1;
        var username = "testuser";
        var amount = 100.50m;
        var paymentDate = new DateTime(2025, 3, 18);
        byte statusId = 1;
        var isComplained = true;

        // Act
        var entity = new BonusEntity
        {
            CreatedDate = createdDate,
            TransId = transId,
            MasterName = masterName,
            AgentName = agentName,
            CustId = custId,
            Username = username,
            Amount = amount,
            PaymentDate = paymentDate,
            StatusId = statusId,
            IsComplained = isComplained
        };

        // Assert
        Assert.Equal(createdDate, entity.CreatedDate);
        Assert.Equal(transId, entity.TransId);
        Assert.Equal(masterName, entity.MasterName);
        Assert.Equal(agentName, entity.AgentName);
        Assert.Equal(custId, entity.CustId);
        Assert.Equal(username, entity.Username);
        Assert.Equal(amount, entity.Amount);
        Assert.Equal(paymentDate, entity.PaymentDate);
        Assert.Equal(statusId, entity.StatusId);
        Assert.Equal(isComplained, entity.IsComplained);
    }

    [Fact]
    public void CreatedDateString_ShouldReturnFormattedDate()
    {
        // Arrange
        var createdDate = new DateTime(2025, 3, 17, 14, 30, 0);
        var entity = new BonusEntity { CreatedDate = createdDate };

        // Act
        var createdDateString = entity.CreatedDateString;

        // Assert
        Assert.Equal("03/17/2025 02:30 PM", createdDateString);
    }

    [Fact]
    public void PaymentDateString_ShouldReturnFormattedDate()
    {
        // Arrange
        var paymentDate = new DateTime(2025, 3, 18);
        var entityWithDate = new BonusEntity { PaymentDate = paymentDate };
        var entityWithoutDate = new BonusEntity { PaymentDate = null };

        // Act & Assert
        Assert.Equal("03/18/2025", entityWithDate.PaymentDateString);
        Assert.Equal(string.Empty, entityWithoutDate.PaymentDateString);
    }

    [Fact]
    public void MinConfirmPaymentDate_ShouldReturnFormattedDate()
    {
        // Arrange
        var createdDate = new DateTime(2025, 3, 17);
        var entity = new BonusEntity { CreatedDate = createdDate };

        // Act
        var minConfirmPaymentDate = entity.MinConfirmPaymentDate;

        // Assert
        Assert.Equal("03/17/2025", minConfirmPaymentDate);
    }

    [Fact]
    public void IsCancelled_ShouldReturnTrue_WhenStatusIdIsCancelled()
    {
        // Arrange
        var entity = new BonusEntity { StatusId = BonusStatusIdConst.Cancelled };

        // Act
        var isCancelled = entity.IsCancelled;

        // Assert
        Assert.True(isCancelled);
    }

    [Fact]
    public void IsCancelled_ShouldReturnFalse_WhenStatusIdIsNotCancelled()
    {
        // Arrange
        var entity = new BonusEntity { StatusId = 2 };

        // Act
        var isCancelled = entity.IsCancelled;

        // Assert
        Assert.False(isCancelled);
    }

    [Fact]
    public void CanConfirmPaymentDate_ShouldReturnTrue_WhenStatusIdIsPending()
    {
        // Arrange
        var entity = new BonusEntity { StatusId = BonusStatusIdConst.Pending };

        // Act
        var canConfirmPaymentDate = entity.CanConfirmPaymentDate;

        // Assert
        Assert.True(canConfirmPaymentDate);
    }

    [Fact]
    public void CanConfirmPaymentDate_ShouldReturnFalse_WhenStatusIdIsNotPending()
    {
        // Arrange
        var entity = new BonusEntity { StatusId = 1 };

        // Act
        var canConfirmPaymentDate = entity.CanConfirmPaymentDate;

        // Assert
        Assert.False(canConfirmPaymentDate);
    }
}
