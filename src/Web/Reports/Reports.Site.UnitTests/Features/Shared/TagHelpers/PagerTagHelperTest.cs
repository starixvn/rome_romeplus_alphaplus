using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class PagerTagHelperTest
{
    [Fact]
    public void Process_When_TotalRecordsIsZero_Should_SetTagNameToNull()
    {
        // Arrange
        var tagHelper = new PagerTagHelper
        {
            Model = new PagerViewModel(PageIndex: 1, PageSize: 10, TotalRecords: 0)
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("pager",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Null(output.TagName);
    }

    [Fact]
    public void Process_When_TotalRecordsIsNotZero_Should_SetTagNameToDiv()
    {
        // Arrange
        var tagHelper = new PagerTagHelper
        {
            Model = new PagerViewModel(PageIndex: 1, PageSize: 10, TotalRecords: 100)
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("pager",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Process_When_TotalRecordsIsNotZero_Should_SetTagModeToStartTagAndEndTag()
    {
        // Arrange
        var tagHelper = new PagerTagHelper
        {
            Model = new PagerViewModel(PageIndex: 1, PageSize: 10, TotalRecords: 100)
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("pager",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_When_TotalRecordsIsNotZero_Should_AddCorrectAttributes()
    {
        // Arrange
        var tagHelper = new PagerTagHelper
        {
            Model = new PagerViewModel(PageIndex: 2, PageSize: 20, TotalRecords: 100)
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test");
        var output = new TagHelperOutput("pager",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("report-pager", output.Attributes["id"].Value);
        Assert.Equal("control-pager", output.Attributes["class"].Value);
        Assert.Equal(Resources.Page, output.Attributes["data-labelpage"].Value);
        Assert.Equal(Resources.of, output.Attributes["data-labelof"].Value);
        Assert.Equal(Resources.Pager_DisplayItems, output.Attributes["data-labeldisplayitems"].Value);
        Assert.Equal(2, output.Attributes["data-pageindex"].Value);
        Assert.Equal(20, output.Attributes["data-pagesize"].Value);
        Assert.Equal(100, output.Attributes["data-totalrecords"].Value);
    }
}