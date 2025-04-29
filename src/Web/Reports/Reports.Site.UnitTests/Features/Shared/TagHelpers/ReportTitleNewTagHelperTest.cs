using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "ReportTitleNewTagHelper")]
public class ReportTitleNewTagHelperTest
{
    private readonly ReportTitleNewTagHelper tagHelper;
    private readonly TagHelperContext context;
    private readonly TagHelperOutput output;
    private readonly PageHeaderViewModel model;

    public ReportTitleNewTagHelperTest()
    {
        model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 5
                }
            )
        );

        tagHelper = new ReportTitleNewTagHelper
        {
            Model = model
        };

        context = new TagHelperContext(
                    new TagHelperAttributeList(),
                    new Dictionary<object, object>(),
                    Guid.NewGuid().ToString());

        output = new TagHelperOutput(
            "report-title-new",
            new TagHelperAttributeList(),
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
    }

    [Fact]
    public void Process_Should_Set_TagName_To_Div()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Process_Should_Set_TagMode_To_StartTagAndEndTag()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_Correct_Class_With_FilterVisible()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("page-title page-title-entered ", output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Add_Hide_Filter_Class_When_FilterVisible_Is_False()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: false,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 5
                }
            )
        );

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("page-title page-title-entered  hide-filter", output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Add_Stack_Mode_Class_When_IsStackMode_Is_True()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: true,
            IsFilterVisible: true,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 5
                }
            )
        );

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("page-title page-title-entered  stack-mode", output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Render_Report_Title()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"text-title\" id=\"report-title\">Test Report</div>", content);
    }

    [Fact]
    public void Process_Should_Render_Entered_Sport_Result_When_TotalEnteredResults_Is_Not_Null()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"entered-non-entered\">", content);
        Assert.Contains("<span class=\"event-date-yesterday\">Yesterday</span>", content);
        Assert.Contains("<span id=\"total-non-entered-today-text\">Today</span>", content);
        Assert.Contains(Resources.NonEntered, content);
        Assert.Contains(Resources.Entered, content);
    }

    [Fact]
    public void Process_Should_Not_Render_Entered_Sport_Result_When_TotalEnteredResults_Is_Null()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: null);

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain("<div class=\"entered-non-entered\">", content);
    }

    [Fact]
    public void Process_Should_Render_Action_Icons_When_IsShowActionIcons_Is_True()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"list-icon\">", content);
        Assert.Contains("<ul>", content);
        Assert.Contains("<li class=\"showfilter-element\">", content);
        Assert.Contains("<li class=\"hidefilter-element\">", content);
        Assert.Contains("<li class=\"normalview-element\">", content);
        Assert.Contains("<li class=\"stackcolumnview-element\">", content);
    }

    [Fact]
    public void Process_Should_Render_Filter_Icons_When_HasHideFilterIcon_Is_True()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("ic-arrow-up-circle icon-filter", content);
        Assert.Contains("ic-arrow-down-circle icon-filter", content);
        Assert.Contains(Resources.HideFilter, content);
        Assert.Contains(Resources.ShowFilter, content);
    }

    [Fact]
    public void Process_Should_Not_Render_Filter_Icons_When_HasHideFilterIcon_Is_False()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: false,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 5
                }
            )
        ); ;

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain("ic-arrow-up-circle icon-filter", content);
        Assert.DoesNotContain("ic-arrow-down-circle icon-filter", content);
    }

    [Fact]
    public void Process_Should_Render_Stack_Icons_When_HasStackIcon_Is_True()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("ic-stack-column ic-switchmode", content);
        Assert.Contains("ic-normal-column ic-switchmode", content);
        Assert.Contains(Resources.Tip_ShowReportInStackColumnView, content);
        Assert.Contains(Resources.Tip_ShowReportInNormalView, content);
    }

    [Fact]
    public void Process_Should_Not_Render_Stack_Icons_When_HasStackIcon_Is_False()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: false,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 5
                }
            )
        ); ;

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.DoesNotContain("ic-stack-column ic-switchmode", content);
        Assert.DoesNotContain("ic-normal-column ic-switchmode", content);
    }

    [Fact]
    public void Process_Should_Show_NonEntered_Yesterday_When_NonEntered_Greater_Than_Zero()
    {
        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"icon-non-entered\" id=\"total-non-entered-yesterday\">", content);
        Assert.DoesNotContain("<div class=\"icon-non-entered\" id=\"total-non-entered-yesterday\" style=display:none", content);
    }

    [Fact]
    public void Process_Should_Hide_NonEntered_Yesterday_When_NonEntered_Is_Zero()
    {
        // Arrange
        tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: new EnteredResultSport(
                SportTypeId: 0,
                SportName: "Test Sport",
                EnteredResultToday: new EnteredResultEntity
                {
                    IsToday = true,
                    EventDate = DateTime.Today,
                    NonEntered = 10
                },
                EnteredResultYesterday: new EnteredResultEntity
                {
                    EventDate = DateTime.Today.AddDays(-1),
                    NonEntered = 0
                }
            )
        );

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("<div class=\"icon-non-entered\" id=\"total-non-entered-yesterday\" style=display:none", content);
    }
}