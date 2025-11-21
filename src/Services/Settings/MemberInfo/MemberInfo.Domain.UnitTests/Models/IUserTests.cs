using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Models;

public class IUserTests
{
    [Fact]
    public void IUser_Interface_Should_Be_Defined()
    {
        // Arrange & Act
        var userType = typeof(IUser);

        // Assert
        Assert.True(userType.IsInterface);
    }

    [Fact]
    public void IUser_Interface_Should_Have_Required_Properties()
    {
        // Arrange
        var userType = typeof(IUser);

        // Act & Assert
        Assert.NotNull(userType.GetProperty("LoggedInUserId"));
        Assert.NotNull(userType.GetProperty("Id"));
        Assert.NotNull(userType.GetProperty("Name"));
        Assert.NotNull(userType.GetProperty("Site"));
        Assert.NotNull(userType.GetProperty("SubPageAccess"));
    }

    [Fact]
    public void IUser_Interface_Should_Have_Required_Methods()
    {
        // Arrange
        var userType = typeof(IUser);

        // Act & Assert
        Assert.NotNull(userType.GetMethod("GetHideProduct"));
        Assert.NotNull(userType.GetMethod("GetSyncedProduct"));
        Assert.NotNull(userType.GetMethod("GetDisabledProduct"));
    }

    [Fact]
    public void IUser_Interface_Should_Have_Correct_Property_Types()
    {
        // Arrange
        var userType = typeof(IUser);

        // Act & Assert
        Assert.Equal(typeof(int), userType.GetProperty("LoggedInUserId")?.PropertyType);
        Assert.Equal(typeof(int), userType.GetProperty("Id")?.PropertyType);
        Assert.Equal(typeof(string), userType.GetProperty("Name")?.PropertyType);
        Assert.Equal(typeof(string), userType.GetProperty("Site")?.PropertyType);
        Assert.Equal(typeof(AlphaPlus.Core.Models.PageAccess), userType.GetProperty("SubPageAccess")?.PropertyType);
    }
}
