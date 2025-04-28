using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class ExcelFileViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var fileName = "test-file";
        var fileContents = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var viewModel = new ExcelFileViewModel(fileName, fileContents);

        // Assert
        Assert.Equal($"{fileName}.xls", viewModel.Name);
        Assert.Equal(fileContents, viewModel.Content);
    }

    [Fact]
    public void Constructor_InvalidFileName_ShouldCreateNewDefaultFileName()
    {
        // Arrange
        string fileName = null;
        var fileContents = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var viewModel = new ExcelFileViewModel(fileName, fileContents);

        // Assert
        Assert.Equal("no-name.xls", viewModel.Name);
    }

    [Fact]
    public void Constructor_InvalidFileNameChars_ShouldReplaceToEmpty()
    {
        // Arrange
        var fileName = "test>file";
        var fileContents = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var viewModel = new ExcelFileViewModel(fileName, fileContents);

        // Assert
        Assert.Equal("testfile.xls", viewModel.Name);
    }
}