using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.TagHelpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.TagHelpers;

[Trait("Category", "Shared")]
public class ReportTitleTagHelperTest
{
    private readonly ReportTitleTagHelper _tagHelper;
    private readonly TagHelperContext _context;
    private readonly TagHelperOutput _output;
    private readonly PageHeaderViewModel _model;

    public ReportTitleTagHelperTest()
    {
        _model = new PageHeaderViewModel(
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

        _tagHelper = new ReportTitleTagHelper
        {
            Model = _model
        };

        _context = new TagHelperContext(
                    new TagHelperAttributeList(),
                    new Dictionary<object, object>(),
                    Guid.NewGuid().ToString());

        _output = new TagHelperOutput(
            "report-title",
            new TagHelperAttributeList(),
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var httpContext = new DefaultHttpContext();
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        AlphaContext.Configure(httpContextAccessorMock.Object);
    }

    [Fact]
    public void Process_Should_Set_TagName_To_Div()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        Assert.Equal("div", _output.TagName);
    }

    [Fact]
    public void Process_Should_Set_TagMode_To_StartTagAndEndTag()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        Assert.Equal(TagMode.StartTagAndEndTag, _output.TagMode);
    }

    [Fact]
    public void Process_Should_Add_Correct_Class_With_FilterVisible()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        Assert.Equal("page-title ", _output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Add_Hide_Filter_Class_When_FilterVisible_Is_False()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: false,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        Assert.Equal("page-title  hide-filter", _output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Add_Stack_Mode_Class_When_IsStackMode_Is_True()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: true,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        Assert.Equal("page-title  stack-mode", _output.Attributes["class"].Value);
    }

    [Fact]
    public void Process_Should_Render_Report_Title()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("<div class=\"text-title\" style=\"clear:both\">Test Report</div>", content);
    }

    [Fact]
    public void Process_Should_Render_Entered_Sport_Result_When_TotalEnteredResults_Is_Not_Null()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("<div class=\"entered-non-entered\">", content);
        Assert.Contains("<div class=\"icon-non-entered\">", content);
        Assert.Contains($"<div class=\"text\">{_tagHelper.Model.TotalEnteredResults.EnteredResultYesterday.EventDateText}: {Resources.NonEntered}</div>", content);
        Assert.Contains($"<div class=\"text\">{_tagHelper.Model.TotalEnteredResults.EnteredResultToday.EventDateText}: {Resources.NonEntered}</div>", content);
    }

    [Fact]
    public void Process_Should_Not_Render_Entered_Sport_Result_When_TotalEnteredResults_Is_Null()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: null
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.DoesNotContain("<div class=\"entered-non-entered\">", content);
    }

    [Fact]
    public void Process_Should_Render_Action_Icons_When_IsShowActionIcons_Is_True()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("<div class=\"list-icon\">", content);
        Assert.Contains("<ul>", content);
        Assert.Contains("<li class=\"showfilter-element\">", content);
        Assert.Contains("<li class=\"hidefilter-element\">", content);
        Assert.Contains("<li class=\"normalview-element\">", content);
        Assert.Contains("<li class=\"stackcolumnview-element\">", content);
    }

    [Fact]
    public void Process_Should_Not_Render_Action_Icons_When_IsShowActionIcons_Is_False()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: false,
            HasHideFilterIcon: false,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.DoesNotContain("<div class=\"list-icon\">", content);
    }

    [Fact]
    public void Process_Should_Render_Filter_Icons_When_HasHideFilterIcon_Is_True()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("ic-arrow-up-circle icon-filter", content);
        Assert.Contains("ic-arrow-down-circle icon-filter", content);
        Assert.Contains(Resources.HideFilter, content);
        Assert.Contains(Resources.ShowFilter, content);
    }

    [Fact]
    public void Process_Should_Not_Render_Filter_Icons_When_HasHideFilterIcon_Is_False()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: false,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.DoesNotContain("ic-arrow-up-circle icon-filter", content);
        Assert.DoesNotContain("ic-arrow-down-circle icon-filter", content);
    }

    [Fact]
    public void Process_Should_Render_Stack_Icons_When_HasStackIcon_Is_True()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("ic-stack-column ic-switchmode", content);
        Assert.Contains("ic-normal-column ic-switchmode", content);
        Assert.Contains(Resources.Tip_ShowReportInStackColumnView, content);
        Assert.Contains(Resources.Tip_ShowReportInNormalView, content);
    }

    [Fact]
    public void Process_Should_Not_Render_Stack_Icons_When_HasStackIcon_Is_False()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: false,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.DoesNotContain("ic-stack-column ic-switchmode", content);
        Assert.DoesNotContain("ic-normal-column ic-switchmode", content);
    }

    [Fact]
    public void Process_Should_Show_EnteredResult_Yesterday_When_NonEntered_Is_Zero()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
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
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("<div class=\"icon-entered\">", content);
        Assert.Contains($"<div class=\"text\">{_tagHelper.Model.TotalEnteredResults.EnteredResultYesterday.EventDateText}: {Resources.Entered}</div>", content);
        Assert.Contains("<div class=\"icon-check-circle\"></div>", content);
    }

    [Fact]
    public void Process_Should_Render_Icon_Info_When_IconInfoUrl_Is_Provided()
    {
        // Arrange
        _tagHelper.Model = new PageHeaderViewModel(
            Title: "Test Report",
            HasStackIcon: true,
            HasHideFilterIcon: true,
            IsStackMode: false,
            IsFilterVisible: true,
            IconInfoUrl: "http://test.com/info",
            TotalEnteredResults: _model.TotalEnteredResults
        );

        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.Contains("<a href=\"http://test.com/info\" target=\"_blank\"><span class=\"ic-information\"></span></a>", content);
    }

    [Fact]
    public void Process_Should_Not_Render_Icon_Info_When_IconInfoUrl_Is_Empty()
    {
        // Act
        _tagHelper.Process(_context, _output);

        // Assert
        var content = _output.Content.GetContent();
        Assert.DoesNotContain("<a href=", content);
        Assert.DoesNotContain("ic-information", content);
    }
}