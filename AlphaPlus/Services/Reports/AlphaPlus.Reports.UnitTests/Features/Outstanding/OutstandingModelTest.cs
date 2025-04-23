using AlphaPlus.Reports.Features.Outstanding;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class OutstandingModelTest
{
    [Fact]
    public void AdditionOperator_ShouldAddAllProperties()
    {
        // Arrange
        var model1 = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400,
            ToWin = 500,
            AgentRisk = 600,
            MasterRisk = 700,
            SuperRisk = 800
        };

        var model2 = new OutstandingModel
        {
            MemberOutstanding = 1000,
            AgentOutstanding = 2000,
            MasterOutstanding = 3000,
            SuperOutstanding = 4000,
            ToWin = 5000,
            AgentRisk = 6000,
            MasterRisk = 7000,
            SuperRisk = 8000
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(1100, result.MemberOutstanding);
        Assert.Equal(2200, result.AgentOutstanding);
        Assert.Equal(3300, result.MasterOutstanding);
        Assert.Equal(4400, result.SuperOutstanding);
        Assert.Equal(5500, result.ToWin);
        Assert.Equal(6600, result.AgentRisk);
        Assert.Equal(7700, result.MasterRisk);
        Assert.Equal(8800, result.SuperRisk);
    }

    [Fact]
    public void AdditionOperator_WithNullValues_ShouldAddCorrectly()
    {
        // Arrange
        var model1 = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400,
            ToWin = null,
            AgentRisk = 600,
            MasterRisk = null,
            SuperRisk = 800
        };

        var model2 = new OutstandingModel
        {
            MemberOutstanding = 1000,
            AgentOutstanding = 2000,
            MasterOutstanding = 3000,
            SuperOutstanding = 4000,
            ToWin = 5000,
            AgentRisk = null,
            MasterRisk = 7000,
            SuperRisk = null
        };

        // Act
        var result = model1 + model2;

        // Assert
        Assert.Equal(1100, result.MemberOutstanding);
        Assert.Equal(2200, result.AgentOutstanding);
        Assert.Equal(3300, result.MasterOutstanding);
        Assert.Equal(4400, result.SuperOutstanding);
        Assert.Equal(5000, result.ToWin);
        Assert.Equal(600, result.AgentRisk);
        Assert.Equal(7000, result.MasterRisk);
        Assert.Equal(800, result.SuperRisk);
    }

    [Theory]
    [InlineData(1)] // Assuming 1 is Member level
    public void GetValueByLevel_ForMemberLevel_ShouldReturnMemberOutstanding(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400
        };

        // Act
        var result = model.GetValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.MemberOutstanding, result);
    }

    [Theory]
    [InlineData(2)] // Assuming 2 is Agent level
    public void GetValueByLevel_ForAgentLevel_ShouldReturnAgentOutstanding(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400
        };

        // Act
        var result = model.GetValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.AgentOutstanding, result);
    }

    [Theory]
    [InlineData(3)] // Assuming 3 is Master level
    public void GetValueByLevel_ForMasterLevel_ShouldReturnMasterOutstanding(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400
        };

        // Act
        var result = model.GetValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.MasterOutstanding, result);
    }

    [Theory]
    [InlineData(4)] // Assuming 4 is Super level
    public void GetValueByLevel_ForSuperLevel_ShouldReturnSuperOutstanding(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            MemberOutstanding = 100,
            AgentOutstanding = 200,
            MasterOutstanding = 300,
            SuperOutstanding = 400
        };

        // Act
        var result = model.GetValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.SuperOutstanding, result);
    }

    [Theory]
    [InlineData(2)] // Assuming 2 is Agent level
    public void GetSMARiskValueByLevel_ForAgentLevel_ShouldReturnAgentRisk(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            AgentRisk = 600,
            MasterRisk = 700,
            SuperRisk = 800
        };

        // Act
        var result = model.GetSMARiskValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.AgentRisk, result);
    }

    [Theory]
    [InlineData(3)] // Assuming 3 is Master level
    public void GetSMARiskValueByLevel_ForMasterLevel_ShouldReturnMasterRisk(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            AgentRisk = 600,
            MasterRisk = 700,
            SuperRisk = 800
        };

        // Act
        var result = model.GetSMARiskValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.MasterRisk, result);
    }

    [Theory]
    [InlineData(4)] // Assuming 4 is Super level
    public void GetSMARiskValueByLevel_ForSuperLevel_ShouldReturnSuperRisk(byte userLevelId)
    {
        // Arrange
        var model = new OutstandingModel
        {
            AgentRisk = 600,
            MasterRisk = 700,
            SuperRisk = 800
        };

        // Act
        var result = model.GetSMARiskValueByLevel(userLevelId);

        // Assert
        Assert.Equal(model.SuperRisk, result);
    }
}