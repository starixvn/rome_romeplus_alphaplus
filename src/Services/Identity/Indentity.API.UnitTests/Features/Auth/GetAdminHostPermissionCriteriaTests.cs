using AlphaPlus.Identity.API.Features.Auth;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Identity.API.UnitTests.Features.Auth;

public class GetAdminHostPermissionCriteriaTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var adminId = 123;
        var functionId = 456;
        var functionName = "TestFunction";

        // Act
        var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

        // Assert
        criteria.CustId.Should().Be(adminId);
        criteria.FuncId.Should().Be(functionId);
        criteria.FuncName.Should().Be(functionName);
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithNullFunctionId_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var adminId = 123;
        int? functionId = null;
        var functionName = "TestFunction";

        // Act
        var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

        // Assert
        criteria.CustId.Should().Be(adminId);
        criteria.FuncId.Should().BeNull();
        criteria.FuncName.Should().Be(functionName);
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithNullFunctionName_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var adminId = 123;
        var functionId = 456;
        string functionName = null;

        // Act
        var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

        // Assert
        criteria.CustId.Should().Be(adminId);
        criteria.FuncId.Should().Be(functionId);
        criteria.FuncName.Should().BeNull();
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithBothNullParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var adminId = 123;
        int? functionId = null;
        string functionName = null;

        // Act
        var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

        // Assert
        criteria.CustId.Should().Be(adminId);
        criteria.FuncId.Should().BeNull();
        criteria.FuncName.Should().BeNull();
        criteria.ErrCode.Should().Be(0);
        criteria.ErrMsg.Should().BeNull();
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetAdminHostPermissionCriteria(123, 456, "TestFunction");

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        settingKey.Should().Be("Acc_Con_CheckPermission");
    }

    public class IsValidTests : GetAdminHostPermissionCriteriaTests
    {
        [Theory]
        [InlineData(1, 123, null)]
        [InlineData(1, null, "TestFunction")]
        [InlineData(1, 123, "TestFunction")]
        [InlineData(999, 456, "AnotherFunction")]
        public void IsValid_WithValidParameters_ShouldReturnTrue(int adminId, int? functionId, string functionName)
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0, 123, null)]
        [InlineData(-1, null, "TestFunction")]
        [InlineData(-100, 456, "TestFunction")]
        public void IsValid_WithInvalidAdminId_ShouldReturnFalse(int adminId, int? functionId, string functionName)
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(1, null, null)]
        [InlineData(1, null, "")]
        [InlineData(1, null, "   ")]
        public void IsValid_WithNoFunctionIdAndNoFunctionName_ShouldReturnFalse(int adminId, int? functionId, string functionName)
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(-1, null, "")]
        [InlineData(-100, null, "   ")]
        public void IsValid_WithInvalidAdminIdAndNoFunctionParameters_ShouldReturnFalse(int adminId, int? functionId, string functionName)
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(adminId, functionId, functionName);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithValidAdminIdAndFunctionId_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, null);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithValidAdminIdAndFunctionName_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, null, "TestFunction");

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithValidAdminIdAndBothFunctionParameters_ShouldReturnTrue()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }
    }

    public class PropertySettersTests : GetAdminHostPermissionCriteriaTests
    {
        [Fact]
        public void ErrCode_CanBeSetAndRetrieved()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");
            var expectedErrCode = 5;

            // Act
            criteria.ErrCode = expectedErrCode;

            // Assert
            criteria.ErrCode.Should().Be(expectedErrCode);
        }

        [Fact]
        public void ErrMsg_CanBeSetAndRetrieved()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");
            var expectedErrMsg = "Permission denied";

            // Act
            criteria.ErrMsg = expectedErrMsg;

            // Assert
            criteria.ErrMsg.Should().Be(expectedErrMsg);
        }

        [Fact]
        public void ErrMsg_CanBeSetToNull()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");
            criteria.ErrMsg = "Some error message";

            // Act
            criteria.ErrMsg = null;

            // Assert
            criteria.ErrMsg.Should().BeNull();
        }

        [Fact]
        public void ErrCode_CanBeSetToZero()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");
            criteria.ErrCode = 5;

            // Act
            criteria.ErrCode = 0;

            // Assert
            criteria.ErrCode.Should().Be(0);
        }

        [Fact]
        public void ErrCode_CanBeSetToNegativeValue()
        {
            // Arrange
            var criteria = new GetAdminHostPermissionCriteria(1, 123, "TestFunction");
            var expectedErrCode = -1;

            // Act
            criteria.ErrCode = expectedErrCode;

            // Assert
            criteria.ErrCode.Should().Be(expectedErrCode);
        }
    }
}
