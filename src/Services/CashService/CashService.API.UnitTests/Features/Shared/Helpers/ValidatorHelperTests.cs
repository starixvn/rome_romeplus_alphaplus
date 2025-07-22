using AlphaPlus.CashService.API.Features.Shared.Helpers;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Helpers;

public class ValidatorHelperTests
{
    [Fact]
    public void IsValidAccountHolderName_WhenValidName_ReturnsTrue()
    {
        // Arrange
        var validName = "John Doe";

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(validName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountHolderName_WhenEmptyString_ReturnsFalse()
    {
        // Arrange
        var emptyName = "";

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(emptyName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountHolderName_WhenWhitespaceString_ReturnsFalse()
    {
        // Arrange
        var whitespaceName = "   ";

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(whitespaceName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountHolderName_WhenNullString_ReturnsFalse()
    {
        // Arrange
        string? nullName = null;

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(nullName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountHolderName_WhenNameExceeds255Characters_ReturnsFalse()
    {
        // Arrange
        var longName = new string('A', 256);

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(longName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountHolderName_WhenNameIsExactly255Characters_ReturnsTrue()
    {
        // Arrange
        var exactLengthName = new string('A', 255);

        // Act
        var result = ValidatorHelper.IsValidAccountHolderName(exactLengthName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenValidAccountNumber_ReturnsTrue()
    {
        // Arrange
        var validAccountNumber = "1234567890";

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(validAccountNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenEmptyString_ReturnsFalse()
    {
        // Arrange
        var emptyAccountNumber = "";

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(emptyAccountNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenWhitespaceString_ReturnsFalse()
    {
        // Arrange
        var whitespaceAccountNumber = "   ";

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(whitespaceAccountNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenNullString_ReturnsFalse()
    {
        // Arrange
        string? nullAccountNumber = null;

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(nullAccountNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenContainsNonDigits_ReturnsFalse()
    {
        // Arrange
        var invalidAccountNumber = "12345ABC";

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(invalidAccountNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenExceeds100Characters_ReturnsFalse()
    {
        // Arrange
        var longAccountNumber = new string('1', 101);

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(longAccountNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNumber_WhenExactly100Characters_ReturnsTrue()
    {
        // Arrange
        var exactLengthAccountNumber = new string('1', 100);

        // Act
        var result = ValidatorHelper.IsValidAccountNumber(exactLengthAccountNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidCountryCode_WhenValidCountryCode_ReturnsTrue()
    {
        // Arrange
        var validCountryCode = "60";

        // Act
        var result = ValidatorHelper.IsValidCountryCode(validCountryCode);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidCountryCode_WhenEmptyString_ReturnsFalse()
    {
        // Arrange
        var emptyCountryCode = "";

        // Act
        var result = ValidatorHelper.IsValidCountryCode(emptyCountryCode);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCountryCode_WhenWhitespaceString_ReturnsFalse()
    {
        // Arrange
        var whitespaceCountryCode = "   ";

        // Act
        var result = ValidatorHelper.IsValidCountryCode(whitespaceCountryCode);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCountryCode_WhenNullString_ReturnsFalse()
    {
        // Arrange
        string? nullCountryCode = null;

        // Act
        var result = ValidatorHelper.IsValidCountryCode(nullCountryCode);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCountryCode_WhenContainsNonDigits_ReturnsFalse()
    {
        // Arrange
        var invalidCountryCode = "6A";

        // Act
        var result = ValidatorHelper.IsValidCountryCode(invalidCountryCode);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenValidMobileNumber_ReturnsTrue()
    {
        // Arrange
        var validMobileNumber = "60123456789";

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(validMobileNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenEmptyString_ReturnsFalse()
    {
        // Arrange
        var emptyMobileNumber = "";

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(emptyMobileNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenWhitespaceString_ReturnsFalse()
    {
        // Arrange
        var whitespaceMobileNumber = "   ";

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(whitespaceMobileNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenNullString_ReturnsFalse()
    {
        // Arrange
        string? nullMobileNumber = null;

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(nullMobileNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenContainsNonDigits_ReturnsFalse()
    {
        // Arrange
        var invalidMobileNumber = "6012345678A";

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(invalidMobileNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMobileNumber_WhenContainsSpecialCharacters_ReturnsFalse()
    {
        // Arrange
        var invalidMobileNumber = "6012345678-";

        // Act
        var result = ValidatorHelper.IsValidMobileNumber(invalidMobileNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNickname_WhenValidNickname_ReturnsTrue()
    {
        // Arrange
        var validNickname = "My Account";

        // Act
        var result = ValidatorHelper.IsValidAccountNickname(validNickname);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountNickname_WhenEmptyString_ReturnsTrue()
    {
        // Arrange
        var emptyNickname = "";

        // Act
        var result = ValidatorHelper.IsValidAccountNickname(emptyNickname);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountNickname_WhenNullString_ReturnsTrue()
    {
        // Arrange
        string? nullNickname = null;

        // Act
        var result = ValidatorHelper.IsValidAccountNickname(nullNickname);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidAccountNickname_WhenExceeds255Characters_ReturnsFalse()
    {
        // Arrange
        var longNickname = new string('A', 256);

        // Act
        var result = ValidatorHelper.IsValidAccountNickname(longNickname);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAccountNickname_WhenExactly255Characters_ReturnsTrue()
    {
        // Arrange
        var exactLengthNickname = new string('A', 255);

        // Act
        var result = ValidatorHelper.IsValidAccountNickname(exactLengthNickname);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenPositiveNumber_ReturnsTrue()
    {
        // Arrange
        var validWithdrawalCount = 5;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(validWithdrawalCount);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenZero_ReturnsFalse()
    {
        // Arrange
        var zeroWithdrawalCount = 0;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(zeroWithdrawalCount);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenNegativeNumber_ReturnsFalse()
    {
        // Arrange
        var negativeWithdrawalCount = -1;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(negativeWithdrawalCount);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenMaximumValue_ReturnsTrue()
    {
        // Arrange
        var maxWithdrawalCount = 32767;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(maxWithdrawalCount);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenExceedsMaximumValue_ReturnsFalse()
    {
        // Arrange
        var exceededWithdrawalCount = 32768;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(exceededWithdrawalCount);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidWithdrawalCount_WhenMinimumValue_ReturnsTrue()
    {
        // Arrange
        var minWithdrawalCount = 1;

        // Act
        var result = ValidatorHelper.IsValidWithdrawalCount(minWithdrawalCount);

        // Assert
        Assert.True(result);
    }
} 