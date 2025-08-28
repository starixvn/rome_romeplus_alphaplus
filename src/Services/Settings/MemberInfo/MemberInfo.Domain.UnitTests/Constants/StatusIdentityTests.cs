using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class StatusIdentityTests
{
    [Fact]
    public void StatusIdentity_Constants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(0, StatusIdentity.All);
        Assert.Equal(1, StatusIdentity.Open);
        Assert.Equal(2, StatusIdentity.Suspended);
        Assert.Equal(3, StatusIdentity.Closed);
        Assert.Equal(4, StatusIdentity.Disabled);
        Assert.Equal(5, StatusIdentity.Deleted);
    }

    [Fact]
    public void StatusIdentity_Constants_Should_Be_Byte_Type()
    {
        // Assert
        Assert.IsType<byte>(StatusIdentity.All);
        Assert.IsType<byte>(StatusIdentity.Open);
        Assert.IsType<byte>(StatusIdentity.Suspended);
        Assert.IsType<byte>(StatusIdentity.Closed);
        Assert.IsType<byte>(StatusIdentity.Disabled);
        Assert.IsType<byte>(StatusIdentity.Deleted);
    }

    [Fact]
    public void StatusIdentity_Constants_Should_Be_Unique()
    {
        // Arrange
        var values = new[]
        {
            StatusIdentity.All,
            StatusIdentity.Open,
            StatusIdentity.Suspended,
            StatusIdentity.Closed,
            StatusIdentity.Disabled,
            StatusIdentity.Deleted
        };

        // Act
        var uniqueValues = values.Distinct().ToArray();

        // Assert
        Assert.Equal(values.Length, uniqueValues.Length);
    }
}
