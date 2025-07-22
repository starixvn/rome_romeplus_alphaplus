using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Shared.Helpers;

[Trait("Category", "Shared")]
public class EndpointHelperTest
{
    [Theory]
    [InlineData("/api/cash-members/check-username", "Check Cash Member Username")]
    [InlineData("/api/cash-members", "Register Cash Member")]
    [InlineData("/api/deposit/limit", "Min Max Deposit")]
    [InlineData("/api/deposit/manual", "Manual Deposit")]
    [InlineData("/api/deposit/auto", "Auto Deposit")]
    [InlineData("/api/withdrawal/limit", "Min Max Withdrawal")]
    [InlineData("/api/withdrawal/manual", "Manual Withdrawal")]
    public void ToFunctionName_Should_Return_Correct_Function_Name_For_Static_Paths(string path, string expectedFunctionName)
    {
        // Act
        var result = EndpointHelper.ToFunctionName(path);

        // Assert
        Assert.Equal(expectedFunctionName, result);
    }

    [Theory]
    [InlineData("/api/cash-members/check-username/", "Check Cash Member Username")]
    [InlineData("/api/cash-members/", "Register Cash Member")]
    public void ToFunctionName_Should_Handle_Trailing_Slashes(string path, string expectedFunctionName)
    {
        // Act
        var result = EndpointHelper.ToFunctionName(path);

        // Assert
        Assert.Equal(expectedFunctionName, result);
    }

    [Theory]
    [InlineData("/api/deposit/123/status", "Update Deposit Transaction Status")]
    [InlineData("/api/deposit/ABC123/status", "Update Deposit Transaction Status")]
    [InlineData("/api/deposit/123/payslip", "Add Payslip To Manual Deposit")]
    [InlineData("/api/deposit/XYZ456/payslip", "Add Payslip To Manual Deposit")]
    [InlineData("/api/withdrawal/789/status", "Update Withdrawal Transaction Status")]
    [InlineData("/api/withdrawal/DEF789/status", "Update Withdrawal Transaction Status")]
    public void ToFunctionName_Should_Handle_Dynamic_Segments(string path, string expectedFunctionName)
    {
        // Act
        var result = EndpointHelper.ToFunctionName(path);

        // Assert
        Assert.Equal(expectedFunctionName, result);
    }

    [Fact]
    public void ToFunctionName_Should_Return_Empty_String_For_Unmapped_Paths()
    {
        // Arrange
        var path = "/api/unknown/endpoint";

        // Act
        var result = EndpointHelper.ToFunctionName(path);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("Register Cash Member", true)]
    [InlineData("REGISTER CASH MEMBER", true)]
    [InlineData("register cash member", true)]
    [InlineData("Check Cash Member Username", false)]
    [InlineData("Manual Deposit", false)]
    public void IsRegisterCashMember_Should_Return_Correct_Value(string functionName, bool expected)
    {
        // Act
        var result = EndpointHelper.IsRegisterCashMember(functionName);

        // Assert
        Assert.Equal(expected, result);
    }
}