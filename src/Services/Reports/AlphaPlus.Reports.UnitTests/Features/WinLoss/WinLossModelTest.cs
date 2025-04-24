using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.UnitTests.Features.WinLoss;

[Trait("Category", "WinLoss")]
public class WinLossModelTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithDefaultValues()
    {
        // Act
        var model = new WinLossModel();

        // Assert
        Assert.Equal(0, model.CustId);
        Assert.Null(model.CustName);
        Assert.Equal(0, model.Turnover);
        Assert.Equal(0, model.GrossComm);
        Assert.Equal(0, model.MemberWinLoss);
        Assert.Equal(0, model.MemberComm);
        Assert.Equal(0, model.MemberTotal);
        Assert.Equal(0, model.DownlineTotal);
        Assert.Equal(0, model.WinLoss);
        Assert.Equal(0, model.Comm);
        Assert.Equal(0, model.Total);
        Assert.Equal(0, model.Company);
        Assert.True(model.IsDormantAccounts);
        Assert.False(model.IsDirectMember);
    }

    [Fact]
    public void MemberTotal_ShouldEqualSumOf_MemberWinLossAndMemberComm()
    {
        // Arrange
        var model = new WinLossModel
        {
            MemberWinLoss = 100,
            MemberComm = 50
        };

        // Act & Assert
        Assert.Equal(150, model.MemberTotal);
    }

    [Fact]
    public void Total_ShouldEqualSumOf_WinLossAndComm()
    {
        // Arrange
        var model = new WinLossModel
        {
            WinLoss = 200,
            Comm = 75
        };

        // Act & Assert
        Assert.Equal(275, model.Total);
    }

    [Fact]
    public void Company_ShouldCalculateCorrectly()
    {
        // Arrange
        var model = new WinLossModel
        {
            MemberWinLoss = 100,
            MemberComm = 50,
            DownlineTotal = 200,
            WinLoss = 300,
            Comm = 75
        };

        // Act & Assert
        // Company = -(MemberTotal + DownlineTotal + Total)
        // = -((100 + 50) + 200 + (300 + 75))
        // = -(150 + 200 + 375)
        // = -725
        Assert.Equal(-725, model.Company);
    }

    [Fact]
    public void IsDormantAccounts_ShouldReturnTrue_WhenCustIdIsZero()
    {
        // Arrange
        var model = new WinLossModel { CustId = 0 };

        // Act & Assert
        Assert.True(model.IsDormantAccounts);
    }

    [Fact]
    public void IsDormantAccounts_ShouldReturnFalse_WhenCustIdIsNotZero()
    {
        // Arrange
        var model = new WinLossModel { CustId = 1 };

        // Act & Assert
        Assert.False(model.IsDormantAccounts);
    }

    [Fact]
    public void OperatorPlus_ShouldCorrectlyAddModels()
    {
        // Arrange
        var model1 = new WinLossModel
        {
            Turnover = 100,
            GrossComm = 50,
            MemberWinLoss = 200,
            MemberComm = 75,
            DownlineTotal = 150,
            WinLoss = 300,
            Comm = 25
        };

        var model2 = new WinLossModel
        {
            Turnover = 200,
            GrossComm = 60,
            MemberWinLoss = 100,
            MemberComm = 25,
            DownlineTotal = 50,
            WinLoss = 150,
            Comm = 35
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(300, result.Turnover);
        Assert.Equal(110, result.GrossComm);
        Assert.Equal(300, result.MemberWinLoss);
        Assert.Equal(100, result.MemberComm);
        Assert.Equal(400, result.MemberTotal);
        Assert.Equal(200, result.DownlineTotal);
        Assert.Equal(450, result.WinLoss);
        Assert.Equal(60, result.Comm);
        Assert.Equal(510, result.Total);
        Assert.Equal(-1110, result.Company);
    }

    [Fact]
    public void IsDirectMember_ShouldBeSettable()
    {
        // Arrange
        var model = new WinLossModel { IsDirectMember = true };

        // Act & Assert
        Assert.True(model.IsDirectMember);
    }
}