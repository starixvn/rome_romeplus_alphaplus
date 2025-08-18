using AlphaPlus.AccountInfo.Site.Features.Shared;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.Shared;

public class DirectMemberAgentIdsTests
{
    [Fact]
    public void DirectMemberAgentIds_ShouldHaveCorrectConstantValues()
    {
        // Assert
        Assert.Equal(-4, DirectMemberAgentIds.Super);
        Assert.Equal(-3, DirectMemberAgentIds.Master);
    }

    [Fact]
    public void DirectMemberAgentIds_ShouldBePublicClass()
    {
        // Assert
        Assert.True(typeof(DirectMemberAgentIds).IsPublic);
        Assert.False(typeof(DirectMemberAgentIds).IsAbstract);
        Assert.False(typeof(DirectMemberAgentIds).IsSealed);
    }

    [Fact]
    public void DirectMemberAgentIds_ShouldHaveAllConstantsAsIntegers()
    {
        // Get all public constants from the DirectMemberAgentIds class
        var constants = typeof(DirectMemberAgentIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly);

        // Assert
        Assert.True(constants.All(f => f.FieldType == typeof(int)), "All constants should be integers");
        Assert.Equal(2, constants.Count()); // Should have exactly 2 constants
    }

    [Fact]
    public void DirectMemberAgentIds_Constants_ShouldBeNegativeValues()
    {
        // Assert
        Assert.True(DirectMemberAgentIds.Super < 0, "Super should be negative");
        Assert.True(DirectMemberAgentIds.Master < 0, "Master should be negative");
    }

    [Fact]
    public void DirectMemberAgentIds_Super_ShouldBeLessThanMaster()
    {
        // Assert
        Assert.True(DirectMemberAgentIds.Super < DirectMemberAgentIds.Master, "Super should be less than Master");
    }
}
