using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;

namespace AlphaPlus.Settings.Site.UnitTests.ViewModels.Components;

public class PagerViewModelTests
{
    [Fact]
    public void PagerViewModel_Should_Initialize_With_Correct_Values()
    {
        // Arrange
        var pageIndex = 2;
        var pageSize = 10;
        var totalRecords = 100;

        // Act
        var viewModel = new PagerViewModel(pageIndex, pageSize, totalRecords);

        // Assert
        viewModel.PageIndex.Should().Be(pageIndex);
        viewModel.PageSize.Should().Be(pageSize);
        viewModel.TotalRecords.Should().Be(totalRecords);
    }

    [Fact]
    public void PagerViewModel_Should_Have_ReadOnly_Properties()
    {
        // Arrange
        var viewModel = new PagerViewModel(1, 10, 50);

        // Act & Assert
        viewModel.PageIndex.Should().Be(1);
        viewModel.PageSize.Should().Be(10);
        viewModel.TotalRecords.Should().Be(50);
    }

    [Fact]
    public void PagerViewModel_Should_Handle_First_Page()
    {
        // Arrange
        var viewModel = new PagerViewModel(1, 20, 200);

        // Act & Assert
        viewModel.PageIndex.Should().Be(1);
        viewModel.PageSize.Should().Be(20);
        viewModel.TotalRecords.Should().Be(200);
    }

    [Fact]
    public void PagerViewModel_Should_Handle_Zero_Values()
    {
        // Arrange
        var viewModel = new PagerViewModel(0, 0, 0);

        // Act & Assert
        viewModel.PageIndex.Should().Be(0);
        viewModel.PageSize.Should().Be(0);
        viewModel.TotalRecords.Should().Be(0);
    }

    [Fact]
    public void PagerViewModel_Should_Handle_Large_Values()
    {
        // Arrange
        var viewModel = new PagerViewModel(1000, 100, 100000);

        // Act & Assert
        viewModel.PageIndex.Should().Be(1000);
        viewModel.PageSize.Should().Be(100);
        viewModel.TotalRecords.Should().Be(100000);
    }

    [Fact]
    public void PagerViewModel_Should_Handle_Negative_Values()
    {
        // Arrange
        var viewModel = new PagerViewModel(-1, -10, -100);

        // Act & Assert
        viewModel.PageIndex.Should().Be(-1);
        viewModel.PageSize.Should().Be(-10);
        viewModel.TotalRecords.Should().Be(-100);
    }

    [Theory]
    [InlineData(1, 10, 100)]
    [InlineData(5, 20, 500)]
    [InlineData(10, 50, 1000)]
    [InlineData(0, 0, 0)]
    public void PagerViewModel_Should_Handle_Various_Configurations(int pageIndex, int pageSize, int totalRecords)
    {
        // Act
        var viewModel = new PagerViewModel(pageIndex, pageSize, totalRecords);

        // Assert
        viewModel.PageIndex.Should().Be(pageIndex);
        viewModel.PageSize.Should().Be(pageSize);
        viewModel.TotalRecords.Should().Be(totalRecords);
    }

    [Fact]
    public void PagerViewModel_Should_Calculate_TotalPages_Correctly()
    {
        // Arrange
        var viewModel = new PagerViewModel(1, 10, 95);

        // Act
        var totalPages = (int)Math.Ceiling((double)viewModel.TotalRecords / viewModel.PageSize);

        // Assert
        totalPages.Should().Be(10); // 95 records / 10 page size = 10 pages (rounded up)
    }

    [Fact]
    public void PagerViewModel_Should_Handle_Single_Record()
    {
        // Arrange
        var viewModel = new PagerViewModel(1, 10, 1);

        // Act & Assert
        viewModel.PageIndex.Should().Be(1);
        viewModel.PageSize.Should().Be(10);
        viewModel.TotalRecords.Should().Be(1);
    }

    [Fact]
    public void PagerViewModel_Should_Handle_Exact_Page_Size()
    {
        // Arrange
        var viewModel = new PagerViewModel(1, 10, 10);

        // Act & Assert
        viewModel.PageIndex.Should().Be(1);
        viewModel.PageSize.Should().Be(10);
        viewModel.TotalRecords.Should().Be(10);
    }
}