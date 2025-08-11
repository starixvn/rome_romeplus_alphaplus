using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.UnitTests.Models;

public class PageAccessTests
{
    [Fact]
    public void Default_Constructor_Should_Initialize_All_Permissions_As_False()
    {
        // Act
        var pageAccess = new PageAccess();

        // Assert
        Assert.False(pageAccess.CreateAccount);
        Assert.False(pageAccess.UpdateAccount);
        Assert.False(pageAccess.Transfer);
        Assert.False(pageAccess.ViewTotalBets);
        Assert.False(pageAccess.ViewReports);
        Assert.False(pageAccess.ViewLog);
        Assert.False(pageAccess.ViewMemberInfo);
        Assert.False(pageAccess.ViewBetLists);
        Assert.False(pageAccess.CashService);
        Assert.False(pageAccess.CashPromotion);
    }

    [Fact]
    public void CreateAccount_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.CreateAccount = true;
        Assert.True(pageAccess.CreateAccount);

        pageAccess.CreateAccount = false;
        Assert.False(pageAccess.CreateAccount);
    }

    [Fact]
    public void UpdateAccount_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.UpdateAccount = true;
        Assert.True(pageAccess.UpdateAccount);

        pageAccess.UpdateAccount = false;
        Assert.False(pageAccess.UpdateAccount);
    }

    [Fact]
    public void Transfer_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.Transfer = true;
        Assert.True(pageAccess.Transfer);

        pageAccess.Transfer = false;
        Assert.False(pageAccess.Transfer);
    }

    [Fact]
    public void ViewTotalBets_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.ViewTotalBets = true;
        Assert.True(pageAccess.ViewTotalBets);

        pageAccess.ViewTotalBets = false;
        Assert.False(pageAccess.ViewTotalBets);
    }

    [Fact]
    public void ViewReports_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.ViewReports = true;
        Assert.True(pageAccess.ViewReports);

        pageAccess.ViewReports = false;
        Assert.False(pageAccess.ViewReports);
    }

    [Fact]
    public void ViewLog_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.ViewLog = true;
        Assert.True(pageAccess.ViewLog);

        pageAccess.ViewLog = false;
        Assert.False(pageAccess.ViewLog);
    }

    [Fact]
    public void ViewMemberInfo_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.ViewMemberInfo = true;
        Assert.True(pageAccess.ViewMemberInfo);

        pageAccess.ViewMemberInfo = false;
        Assert.False(pageAccess.ViewMemberInfo);
    }

    [Fact]
    public void ViewBetLists_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.ViewBetLists = true;
        Assert.True(pageAccess.ViewBetLists);

        pageAccess.ViewBetLists = false;
        Assert.False(pageAccess.ViewBetLists);
    }

    [Fact]
    public void CashService_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.CashService = true;
        Assert.True(pageAccess.CashService);

        pageAccess.CashService = false;
        Assert.False(pageAccess.CashService);
    }

    [Fact]
    public void CashPromotion_Property_Should_Get_And_Set_Correctly()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act & Assert
        pageAccess.CashPromotion = true;
        Assert.True(pageAccess.CashPromotion);

        pageAccess.CashPromotion = false;
        Assert.False(pageAccess.CashPromotion);
    }

    [Fact]
    public void SetPermission_With_Valid_Key_Should_Set_Permission()
    {
        // Arrange
        var pageAccess = new PageAccess();
        var key = PageAccessType.CreateAccount;

        // Act
        pageAccess.SetPermission(key, true);

        // Assert
        Assert.True(pageAccess.CreateAccount);
    }

    [Fact]
    public void SetPermission_With_Invalid_Key_Should_Not_Throw_Exception()
    {
        // Arrange
        var pageAccess = new PageAccess();
        var invalidKey = "InvalidKey";

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => pageAccess.SetPermission(invalidKey, true));
        Assert.Null(exception);
    }

    [Fact]
    public void GetPermission_With_Valid_Key_Should_Return_Permission_Value()
    {
        // Arrange
        var pageAccess = new PageAccess();
        var key = PageAccessType.Transfer;
        pageAccess.Transfer = true;

        // Act
        var result = pageAccess.GetPermission(key);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetPermission_With_Invalid_Key_Should_Return_False()
    {
        // Arrange
        var pageAccess = new PageAccess();
        var invalidKey = "InvalidKey";

        // Act
        var result = pageAccess.GetPermission(invalidKey);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEmptyPermission_Should_Return_True_When_All_Permissions_Are_False()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act
        var result = pageAccess.IsEmptyPermission();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmptyPermission_Should_Return_False_When_Any_Permission_Is_True()
    {
        // Arrange
        var pageAccess = new PageAccess();
        pageAccess.CreateAccount = true;

        // Act
        var result = pageAccess.IsEmptyPermission();

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void SetFullPermission_Should_Set_All_Permissions_To_True()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act
        pageAccess.SetFullPermission();

        // Assert
        Assert.True(pageAccess.CreateAccount);
        Assert.True(pageAccess.UpdateAccount);
        Assert.True(pageAccess.Transfer);
        Assert.True(pageAccess.ViewTotalBets);
        Assert.True(pageAccess.ViewReports);
        Assert.True(pageAccess.ViewLog);
        Assert.True(pageAccess.ViewMemberInfo);
        Assert.True(pageAccess.ViewBetLists);
    }

    [Fact]
    public void SetCashServicePermission_Should_Set_Cash_Related_Permissions_To_True()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act
        pageAccess.SetCashServicePermission();

        // Assert
        Assert.True(pageAccess.CashService);
        Assert.True(pageAccess.CashPromotion);
        Assert.False(pageAccess.CreateAccount); // Other permissions should remain false
        Assert.False(pageAccess.Transfer);
    }

    [Fact]
    public void Multiple_Properties_Can_Be_Set_Independently()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act
        pageAccess.CreateAccount = true;
        pageAccess.Transfer = true;
        pageAccess.ViewReports = false;
        pageAccess.CashService = true;

        // Assert
        Assert.True(pageAccess.CreateAccount);
        Assert.True(pageAccess.Transfer);
        Assert.False(pageAccess.ViewReports);
        Assert.True(pageAccess.CashService);
        Assert.False(pageAccess.UpdateAccount); // Should remain false
    }

    [Fact]
    public void Properties_Should_Be_Independent_Of_Each_Other()
    {
        // Arrange
        var pageAccess = new PageAccess();

        // Act
        pageAccess.CreateAccount = true;
        pageAccess.UpdateAccount = false;
        pageAccess.Transfer = true;

        // Assert
        Assert.True(pageAccess.CreateAccount);
        Assert.False(pageAccess.UpdateAccount);
        Assert.True(pageAccess.Transfer);
    }

    [Fact]
    public void PageAccess_Should_Be_Serializable()
    {
        // Arrange
        var pageAccess = new PageAccess();
        pageAccess.CreateAccount = true;
        pageAccess.Transfer = true;

        // Act & Assert
        var type = typeof(PageAccess);
        var attributes = type.GetCustomAttributes(typeof(SerializableAttribute), false);

        Assert.True(attributes.Length > 0);
        Assert.IsType<SerializableAttribute>(attributes[0]);
    }
}
