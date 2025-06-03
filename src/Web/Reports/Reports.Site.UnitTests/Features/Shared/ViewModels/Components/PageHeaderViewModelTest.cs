using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests.ViewModels.Components;

[Trait("Category", "Shared")]
public class PageHeaderViewModelTest
{
    [Fact]
    public void Constructor_WithDefaultParameters_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var viewModel = new PageHeaderViewModel("Test Title");

        // Assert
        Assert.Equal("Test Title", viewModel.Title);
        Assert.False(viewModel.HasStackIcon);
        Assert.True(viewModel.HasHideFilterIcon);
        Assert.False(viewModel.IsStackMode);
        Assert.True(viewModel.IsFilterVisible);
        Assert.Null(viewModel.BreadcrumbLevel);
        Assert.Null(viewModel.TotalEnteredResults);
        Assert.Null(viewModel.IconInfoUrl);
        Assert.True(viewModel.IsShowActionIcons);
    }

    [Fact]
    public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var breadcrumb = new BreadcrumbViewModel([], string.Empty);
        var totalEnteredResults = new EnteredResultSport(1, "SportName", null, null);

        // Act
        var viewModel = new PageHeaderViewModel(
            "Test Title",
            true,
            false,
            true,
            false,
            breadcrumb,
            totalEnteredResults,
            "info-url");

        // Assert
        Assert.Equal("Test Title", viewModel.Title);
        Assert.True(viewModel.HasStackIcon);
        Assert.False(viewModel.HasHideFilterIcon);
        Assert.True(viewModel.IsStackMode);
        Assert.False(viewModel.IsFilterVisible);
        Assert.Same(breadcrumb, viewModel.BreadcrumbLevel);
        Assert.Same(totalEnteredResults, viewModel.TotalEnteredResults);
        Assert.Equal("info-url", viewModel.IconInfoUrl);
        Assert.True(viewModel.IsShowActionIcons);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void IsShowActionIcons_DependsOn_HasStackIconAndHasHideFilterIcon(
        bool hasStackIcon,
        bool hasHideFilterIcon,
        bool expectedIsShowActionIcons)
    {
        // Arrange & Act
        var viewModel = new PageHeaderViewModel(
            "Test Title",
            hasStackIcon,
            hasHideFilterIcon);

        // Assert
        Assert.Equal(expectedIsShowActionIcons, viewModel.IsShowActionIcons);
    }

    [Fact]
    public void PageHeaderViewModel_IsRecordType_SupportsValueEquality()
    {
        // Arrange
        var breadcrumb = new BreadcrumbViewModel([], string.Empty);
        var totalEnteredResults = new EnteredResultSport(1, "SportName", null, null);

        var viewModel1 = new PageHeaderViewModel(
            "Test Title",
            true,
            false,
            true,
            false,
            breadcrumb,
            totalEnteredResults,
            "info-url");

        var viewModel2 = new PageHeaderViewModel(
            "Test Title",
            true,
            false,
            true,
            false,
            breadcrumb,
            totalEnteredResults,
            "info-url");

        // Act & Assert
        Assert.Equal(viewModel1, viewModel2);
        Assert.True(viewModel1 == viewModel2);
    }
}