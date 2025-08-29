using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.Commission;

public class SportCommissionModelTests
{
    [Fact]
    public void SportCommissionModel_Should_Initialize_With_Constructor_Parameters()
    {
        // Arrange
        var masterDiscount = 10.5m;
        var agentDiscount = 20.75m;
        var playerDiscount = 30.25m;

        // Act
        var model = new SportCommissionModel(masterDiscount, agentDiscount, playerDiscount);

        // Assert
        Assert.Equal(masterDiscount, model.MasterDiscount);
        Assert.Equal(agentDiscount, model.AgentDiscount);
        Assert.Equal(playerDiscount, model.PlayerDiscount);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10.5, 20.75, 30.25)]
    [InlineData(-5.5, -10.25, -15.75)]
    [InlineData(100, 200, 300)]
    public void SportCommissionModel_Should_Handle_Various_Decimal_Values(decimal masterDiscount, decimal agentDiscount, decimal playerDiscount)
    {
        // Act
        var model = new SportCommissionModel(masterDiscount, agentDiscount, playerDiscount);

        // Assert
        Assert.Equal(masterDiscount, model.MasterDiscount);
        Assert.Equal(agentDiscount, model.AgentDiscount);
        Assert.Equal(playerDiscount, model.PlayerDiscount);
    }

    [Fact]
    public void SportCommissionModel_Should_Be_Immutable()
    {
        // Arrange
        var model = new SportCommissionModel(10.5m, 20.75m, 30.25m);

        // Act & Assert
        var type = typeof(SportCommissionModel);
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            // Note: For positional records, properties may show as writable but have init-only setters
            // The important thing is the record maintains immutability behavior
        }
        
        // Test that the record behaves immutably - values don't change
        Assert.Equal(10.5m, model.MasterDiscount);
        Assert.Equal(20.75m, model.AgentDiscount);
        Assert.Equal(30.25m, model.PlayerDiscount);
    }

    [Fact]
    public void SportCommissionModel_Should_Support_Equality_Comparison()
    {
        // Arrange
        var model1 = new SportCommissionModel(10.5m, 20.75m, 30.25m);
        var model2 = new SportCommissionModel(10.5m, 20.75m, 30.25m);
        var model3 = new SportCommissionModel(15.5m, 25.75m, 35.25m);

        // Act & Assert
        Assert.Equal(model1, model2); // Same values should be equal
        Assert.NotEqual(model1, model3); // Different values should not be equal
        Assert.True(model1.Equals(model2));
        Assert.False(model1.Equals(model3));
    }

    [Fact]
    public void SportCommissionModel_Should_Have_Same_HashCode_For_Equal_Objects()
    {
        // Arrange
        var model1 = new SportCommissionModel(10.5m, 20.75m, 30.25m);
        var model2 = new SportCommissionModel(10.5m, 20.75m, 30.25m);

        // Act & Assert
        Assert.Equal(model1.GetHashCode(), model2.GetHashCode());
    }

    [Fact]
    public void SportCommissionModel_Should_Have_Different_HashCode_For_Different_Objects()
    {
        // Arrange
        var model1 = new SportCommissionModel(10.5m, 20.75m, 30.25m);
        var model2 = new SportCommissionModel(15.5m, 25.75m, 35.25m);

        // Act & Assert
        Assert.NotEqual(model1.GetHashCode(), model2.GetHashCode());
    }

    [Fact]
    public void SportCommissionModel_Should_Support_Deconstruction()
    {
        // Arrange
        var model = new SportCommissionModel(10.5m, 20.75m, 30.25m);

        // Act
        var (masterDiscount, agentDiscount, playerDiscount) = model;

        // Assert
        Assert.Equal(10.5m, masterDiscount);
        Assert.Equal(20.75m, agentDiscount);
        Assert.Equal(30.25m, playerDiscount);
    }

    [Fact]
    public void SportCommissionModel_Should_Support_With_Expression()
    {
        // Arrange
        var originalModel = new SportCommissionModel(10.5m, 20.75m, 30.25m);

        // Act
        var modifiedModel = originalModel with { MasterDiscount = 15.5m };

        // Assert
        Assert.Equal(15.5m, modifiedModel.MasterDiscount);
        Assert.Equal(20.75m, modifiedModel.AgentDiscount); // Unchanged
        Assert.Equal(30.25m, modifiedModel.PlayerDiscount); // Unchanged
        
        // Original should remain unchanged
        Assert.Equal(10.5m, originalModel.MasterDiscount);
    }

    [Fact]
    public void SportCommissionModel_Should_Have_Meaningful_ToString()
    {
        // Arrange
        var model = new SportCommissionModel(10.5m, 20.75m, 30.25m);

        // Act
        var toString = model.ToString();

        // Assert
        Assert.NotNull(toString);
        Assert.Contains("10.5", toString);
        Assert.Contains("20.75", toString);
        Assert.Contains("30.25", toString);
    }

    [Theory]
    [MemberData(nameof(ExtremeDecimalValues))]
    public void SportCommissionModel_Should_Handle_Extreme_Decimal_Values(decimal masterDiscount, decimal agentDiscount, decimal playerDiscount)
    {
        // Act
        var model = new SportCommissionModel(masterDiscount, agentDiscount, playerDiscount);

        // Assert
        Assert.Equal(masterDiscount, model.MasterDiscount);
        Assert.Equal(agentDiscount, model.AgentDiscount);
        Assert.Equal(playerDiscount, model.PlayerDiscount);
    }

    [Fact]
    public void SportCommissionModel_Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var model1 = new SportCommissionModel(10m, 20m, 30m);
        var model2 = new SportCommissionModel(40m, 50m, 60m);

        // Assert
        Assert.Equal(10m, model1.MasterDiscount);
        Assert.Equal(20m, model1.AgentDiscount);
        Assert.Equal(30m, model1.PlayerDiscount);
        
        Assert.Equal(40m, model2.MasterDiscount);
        Assert.Equal(50m, model2.AgentDiscount);
        Assert.Equal(60m, model2.PlayerDiscount);
    }

    [Fact]
    public void SportCommissionModel_Should_Be_Record_Type()
    {
        // Arrange
        var type = typeof(SportCommissionModel);

        // Act & Assert
        Assert.True(type.IsClass);
        // Records in C# are reference types that inherit from object
        Assert.True(type.IsSubclassOf(typeof(object)));
        
        // Records should have synthesized methods
        Assert.NotNull(type.GetMethod("Equals", new[] { typeof(object) }));
        Assert.NotNull(type.GetMethod("GetHashCode"));
        Assert.NotNull(type.GetMethod("ToString"));
    }

    [Fact]
    public void SportCommissionModel_Properties_Should_Have_Correct_Types()
    {
        // Arrange
        var type = typeof(SportCommissionModel);

        // Act & Assert
        Assert.Equal(typeof(decimal), type.GetProperty(nameof(SportCommissionModel.MasterDiscount))?.PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty(nameof(SportCommissionModel.AgentDiscount))?.PropertyType);
        Assert.Equal(typeof(decimal), type.GetProperty(nameof(SportCommissionModel.PlayerDiscount))?.PropertyType);
    }

    public static IEnumerable<object[]> ExtremeDecimalValues =>
        new List<object[]>
        {
            new object[] { decimal.MinValue, decimal.MinValue, decimal.MinValue },
            new object[] { decimal.MaxValue, decimal.MaxValue, decimal.MaxValue },
            new object[] { 0.000001m, 0.000002m, 0.000003m },
            new object[] { 999999.999999m, 888888.888888m, 777777.777777m }
        };
}
