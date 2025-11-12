using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class DirectMemberAgentIdsTests
{
    [Fact]
    public void DirectMemberAgentIds_Should_Have_Correct_Super_Value()
    {
        // Act & Assert
        Assert.Equal(-4, DirectMemberAgentIds.Super);
    }

    [Fact]
    public void DirectMemberAgentIds_Should_Have_Correct_Master_Value()
    {
        // Act & Assert
        Assert.Equal(-3, DirectMemberAgentIds.Master);
    }

    [Fact]
    public void DirectMemberAgentIds_Should_Have_Negative_Values()
    {
        // Act & Assert
        Assert.True(DirectMemberAgentIds.Super < 0);
        Assert.True(DirectMemberAgentIds.Master < 0);
    }

    [Fact]
    public void DirectMemberAgentIds_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            DirectMemberAgentIds.Super,
            DirectMemberAgentIds.Master
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void DirectMemberAgentIds_Super_Should_Be_Less_Than_Master()
    {
        // Act & Assert
        Assert.True(DirectMemberAgentIds.Super < DirectMemberAgentIds.Master);
    }
}
