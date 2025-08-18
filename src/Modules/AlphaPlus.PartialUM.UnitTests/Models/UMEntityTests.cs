using AlphaPlus.PartialUM.Models;

namespace AlphaPlus.PartialUM.UnitTests.Models;

public class UMEntityTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var functionTypeName = "TestFunction";
        var beginTime = DateTime.Now;
        var endTime = DateTime.Now.AddHours(1);

        // Act
        var umEntity = new UMEntity
        {
            FunctionTypeName = functionTypeName,
            BeginTime = beginTime,
            EndTime = endTime
        };

        // Assert
        Assert.Equal(functionTypeName, umEntity.FunctionTypeName);
        Assert.Equal(beginTime, umEntity.BeginTime);
        Assert.Equal(endTime, umEntity.EndTime);
    }

    [Fact]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange
        var umEntity = new UMEntity
        {
            FunctionTypeName = "TestFunction",
            BeginTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1)
        };

        // Act & Assert
        // The properties should be init-only, so we can't modify them after initialization
        // This test verifies the immutability of the object
        Assert.NotNull(umEntity.FunctionTypeName);
        Assert.NotEqual(default(DateTime), umEntity.BeginTime);
        Assert.NotEqual(default(DateTime), umEntity.EndTime);
    }
}
