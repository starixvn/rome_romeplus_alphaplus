using AlphaPlus.Reports.Site.Features.Shared;

namespace Reports.Site.UnitTests.Features.Shared;

public class ExcelFileViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var fileName = "test-file.xlsx";
        var fileContents = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var viewModel = new ExcelFileViewModel(fileName, fileContents);

        // Assert
        Assert.Equal(fileName, viewModel.FileName);
        Assert.Equal(fileContents, viewModel.FileContents);
    }

    [Fact]
    public void ImplicitConversion_ToByteArray_ShouldReturnFileContents()
    {
        // Arrange
        var fileName = "test-file.xlsx";
        var fileContents = new byte[] { 1, 2, 3, 4, 5 };
        var viewModel = new ExcelFileViewModel(fileName, fileContents);

        // Act
        byte[] result = viewModel;

        // Assert
        Assert.Equal(fileContents, result);
    }
}