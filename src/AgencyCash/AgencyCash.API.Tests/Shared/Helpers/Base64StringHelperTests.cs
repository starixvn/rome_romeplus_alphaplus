using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Shared.Helpers;

[Trait("Category", "Shared")]
public class Base64StringHelperTests
{
    [Fact]
    public void TryConvertToByte_WithValidBase64String_ShouldReturnTrueAndCorrectBytes()
    {
        // Arrange
        string validBase64 = "SGVsbG8gV29ybGQ="; // "Hello World" in base64
        byte[] expectedBytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };

        // Act
        bool result = Base64StringHelper.TryConvertToByte(validBase64, out byte[] actualBytes);

        // Assert
        Assert.True(result);
        Assert.NotNull(actualBytes);
        Assert.Equal(expectedBytes, actualBytes);
    }

    [Fact]
    public void TryConvertToByte_WithValidDataUri_ShouldReturnTrueAndCorrectBytes()
    {
        // Arrange
        string dataUri = "data:image/png;base64,SGVsbG8gV29ybGQ="; // "Hello World" with data URI
        byte[] expectedBytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 };

        // Act
        bool result = Base64StringHelper.TryConvertToByte(dataUri, out byte[] actualBytes);

        // Assert
        Assert.True(result);
        Assert.NotNull(actualBytes);
        Assert.Equal(expectedBytes, actualBytes);
    }

    [Fact]
    public void TryConvertToByte_WithInvalidBase64String_ShouldReturnFalseAndNullBytes()
    {
        // Arrange
        string invalidBase64 = "This is not a valid base64 string!";

        // Act
        bool result = Base64StringHelper.TryConvertToByte(invalidBase64, out byte[] actualBytes);

        // Assert
        Assert.False(result);
        Assert.Null(actualBytes);
    }

    [Fact]
    public void TryConvertToByte_WithMultipleCommas_ShouldHandleCorrectly()
    {
        // Arrange
        string multipleCommas = "data:image/png;base64,SGVsbG8,V29ybGQ=";

        // Act
        bool result = Base64StringHelper.TryConvertToByte(multipleCommas, out byte[] actualBytes);

        // Assert
        Assert.False(result);
        Assert.Null(actualBytes);
    }
}