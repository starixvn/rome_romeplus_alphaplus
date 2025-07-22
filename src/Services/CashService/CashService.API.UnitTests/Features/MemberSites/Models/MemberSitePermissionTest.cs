using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Models;

public class MemberSitePermissionTest
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var permission = new MemberSitePermission();

        // Assert
        Assert.Null(permission.Id);
        Assert.Null(permission.Name);
        Assert.Null(permission.Description);
        Assert.False(permission.Value);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var permission = new MemberSitePermission();
        var id = "test-id";
        var name = "Test Permission";
        var description = "Test Description";
        var value = true;

        // Act
        permission.Id = id;
        permission.Name = name;
        permission.Description = description;
        permission.Value = value;

        // Assert
        Assert.Equal(id, permission.Id);
        Assert.Equal(name, permission.Name);
        Assert.Equal(description, permission.Description);
        Assert.Equal(value, permission.Value);
    }
} 