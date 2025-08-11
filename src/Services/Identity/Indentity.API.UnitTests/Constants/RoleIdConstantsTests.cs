namespace AlphaPlus.Identity.API.UnitTests.Constants;

public class RoleIdConstantsTests
{
    [Fact]
    public void Super_ShouldReturnCorrectValue()
    {
        // Act & Assert
        RoleIdConstants.Super.Should().Be(4);
    }

    [Fact]
    public void Master_ShouldReturnCorrectValue()
    {
        // Act & Assert
        RoleIdConstants.Master.Should().Be(3);
    }

    [Fact]
    public void Agent_ShouldReturnCorrectValue()
    {
        // Act & Assert
        RoleIdConstants.Agent.Should().Be(2);
    }

    [Fact]
    public void Member_ShouldReturnCorrectValue()
    {
        // Act & Assert
        RoleIdConstants.Member.Should().Be(1);
    }

    [Fact]
    public void RoleHierarchy_ShouldBeInDescendingOrder()
    {
        // Act & Assert
        RoleIdConstants.Super.Should().BeGreaterThan(RoleIdConstants.Master);
        RoleIdConstants.Master.Should().BeGreaterThan(RoleIdConstants.Agent);
        RoleIdConstants.Agent.Should().BeGreaterThan(RoleIdConstants.Member);
    }

    [Fact]
    public void AllRoleIds_ShouldBePositive()
    {
        // Act & Assert
        RoleIdConstants.Super.Should().BePositive();
        RoleIdConstants.Master.Should().BePositive();
        RoleIdConstants.Agent.Should().BePositive();
        RoleIdConstants.Member.Should().BePositive();
    }

    [Fact]
    public void RoleIds_ShouldBeUnique()
    {
        // Arrange
        var roleIds = new[]
        {
            RoleIdConstants.Super,
            RoleIdConstants.Master,
            RoleIdConstants.Agent,
            RoleIdConstants.Member
        };

        // Act & Assert
        roleIds.Should().OnlyHaveUniqueItems();
    }
}
