using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.Shared.Models;

public class BelongingInfoTests
{
    [Fact]
    public void BelongingInfo_Constructor_Should_Set_All_Properties()
    {
        // Arrange
        var isActive = true;
        var roleId = (byte)5;
        var username = "testuser";

        // Act
        var belongingInfo = new BelongingInfo(isActive, roleId, username);

        // Assert
        Assert.Equal(isActive, belongingInfo.IsActive);
        Assert.Equal(roleId, belongingInfo.RoleId);
        Assert.Equal(username, belongingInfo.Username);
    }

    [Theory]
    [InlineData(true, 3, "user1")]
    [InlineData(false, 0, "")]
    [InlineData(true, 255, "admin")]
    public void BelongingInfo_Should_Handle_Various_Values(bool isActive, byte roleId, string username)
    {
        // Act
        var belongingInfo = new BelongingInfo(isActive, roleId, username);

        // Assert
        Assert.Equal(isActive, belongingInfo.IsActive);
        Assert.Equal(roleId, belongingInfo.RoleId);
        Assert.Equal(username, belongingInfo.Username);
    }

    [Theory]
    [InlineData(true, 3, 5, true)]   // Active and viewer has higher level
    [InlineData(true, 5, 5, false)]  // Active but viewer has same level
    [InlineData(true, 7, 5, false)]  // Active but viewer has lower level
    [InlineData(false, 3, 5, false)] // Not active
    [InlineData(false, 7, 5, false)] // Not active, viewer has lower level
    public void IsValid_Should_Return_Correct_Result(bool isActive, byte roleId, byte viewerLevelId, bool expected)
    {
        // Arrange
        var belongingInfo = new BelongingInfo(isActive, roleId, "testuser");

        // Act
        var result = belongingInfo.IsValid(viewerLevelId);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_Not_Active_Regardless_Of_Level()
    {
        // Arrange
        var belongingInfo = new BelongingInfo(false, 1, "testuser");

        // Act & Assert
        Assert.False(belongingInfo.IsValid(10)); // High level viewer
        Assert.False(belongingInfo.IsValid(1));  // Same level viewer
        Assert.False(belongingInfo.IsValid(0));  // Lower level viewer
    }

    [Fact]
    public void IsValid_Should_Return_False_When_Viewer_Level_Is_Not_Higher()
    {
        // Arrange
        var belongingInfo = new BelongingInfo(true, 5, "testuser");

        // Act & Assert
        Assert.False(belongingInfo.IsValid(5)); // Same level
        Assert.False(belongingInfo.IsValid(3)); // Lower level
        Assert.True(belongingInfo.IsValid(8));  // Higher level
    }

    [Fact]
    public void IsValid_Should_Handle_Maximum_RoleId_Values()
    {
        // Arrange
        var belongingInfo = new BelongingInfo(true, 255, "testuser");

        // Act & Assert
        Assert.False(belongingInfo.IsValid(255)); // Same level
        Assert.False(belongingInfo.IsValid(254)); // Lower level - no higher level exists
    }

    [Fact]
    public void IsValid_Should_Handle_Minimum_RoleId_Values()
    {
        // Arrange
        var belongingInfo = new BelongingInfo(true, 0, "testuser");

        // Act & Assert
        Assert.True(belongingInfo.IsValid(1));  // Any higher level
        Assert.True(belongingInfo.IsValid(255)); // Maximum level
    }
}
