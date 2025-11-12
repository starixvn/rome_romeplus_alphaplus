using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Models;

public class CustInfoTests
{
    [Fact]
    public void CustInfo_Should_Initialize_With_Default_Values()
    {
        // Act
        var custInfo = new CustInfo();

        // Assert
        Assert.Equal(0, custInfo.CustId);
        Assert.Equal(0, custInfo.RoleId);
        Assert.Null(custInfo.Username);
        Assert.Null(custInfo.FirstName);
        Assert.Null(custInfo.LastName);
        Assert.Null(custInfo.Username2);
        Assert.Null(custInfo.AgentId);
    }

    [Fact]
    public void CustInfo_Should_Set_Init_Properties()
    {
        // Act
        var custInfo = new CustInfo
        {
            CustId = 123,
            RoleId = 2,
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Username2 = "johndoe"
        };

        // Assert
        Assert.Equal(123, custInfo.CustId);
        Assert.Equal(2, custInfo.RoleId);
        Assert.Equal("testuser", custInfo.Username);
        Assert.Equal("John", custInfo.FirstName);
        Assert.Equal("Doe", custInfo.LastName);
        Assert.Equal("johndoe", custInfo.Username2);
    }

    [Fact]
    public void CustInfo_Should_Set_AgentId_Property()
    {
        // Arrange
        var custInfo = new CustInfo();

        // Act
        custInfo.AgentId = 456;

        // Assert
        Assert.Equal(456, custInfo.AgentId);
    }

    [Fact]
    public void CustInfo_Should_Handle_Null_AgentId()
    {
        // Arrange
        var custInfo = new CustInfo { AgentId = 123 };

        // Act
        custInfo.AgentId = null;

        // Assert
        Assert.Null(custInfo.AgentId);
    }

    [Fact]
    public void CustInfo_Should_Create_With_All_Properties()
    {
        // Act
        var custInfo = new CustInfo
        {
            CustId = 789,
            RoleId = 3,
            Username = "admin",
            FirstName = "Admin",
            LastName = "User",
            Username2 = "adminuser",
            AgentId = 101
        };

        // Assert
        Assert.Equal(789, custInfo.CustId);
        Assert.Equal(3, custInfo.RoleId);
        Assert.Equal("admin", custInfo.Username);
        Assert.Equal("Admin", custInfo.FirstName);
        Assert.Equal("User", custInfo.LastName);
        Assert.Equal("adminuser", custInfo.Username2);
        Assert.Equal(101, custInfo.AgentId);
    }
}
