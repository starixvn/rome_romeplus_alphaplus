using AlphaPlus.Settings.Site.Features.MemberInfo.Helpers;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.UnitTests.Helpers;

public class HtmlHelpersTests
{
    [Fact]
    public void Alert_Render_Should_Return_Correct_HtmlString()
    {
        // Act
        var result = Alert.Render();

        // Assert
        result.Should().NotBeNull();
        var htmlString = result.ToString();
        htmlString.Should().Contain("alert-message");
        htmlString.Should().Contain("msg-content");
        htmlString.Should().Contain("hidden");
        htmlString.Should().Contain("alert-message-content");
    }

    [Fact]
    public void Pager_Render_With_Valid_Model_Should_Return_Pager_Html()
    {
        // Arrange
        var model = new PagerViewModel(2, 10, 100);

        // Act
        var result = Pager.Render(model);

        // Assert
        result.Should().NotBeNull();
        var htmlString = result.ToString();
        htmlString.Should().Contain("report-pager");
        htmlString.Should().Contain("control-pager");
        htmlString.Should().Contain("data-pageindex='2'");
        htmlString.Should().Contain("data-pagesize='10'");
        htmlString.Should().Contain("data-totalrecords='100'");
        htmlString.Should().Contain(Resources.Page);
        htmlString.Should().Contain(Resources.Of);
    }

    [Fact]
    public void Pager_Render_With_Zero_TotalRecords_Should_Return_Empty_Html()
    {
        // Arrange
        var model = new PagerViewModel(1, 10, 0);

        // Act
        var result = Pager.Render(model);

        // Assert
        result.Should().Be(HtmlString.Empty);
    }

    [Fact]
    public void Pager_Render_With_Null_Model_Should_Return_Empty_Html()
    {
        // Act
        var result = Pager.Render(null);

        // Assert
        result.Should().Be(HtmlString.Empty);
    }

    [Fact]
    public void NoInformation_Render_Should_Return_Correct_Html_With_Colspan()
    {
        // Arrange
        var colspan = 5;

        // Act
        var result = NoInformation.Render(colspan);

        // Assert
        result.Should().NotBeNull();
        var htmlString = result.ToString();
        htmlString.Should().Contain("<tr>");
        htmlString.Should().Contain("</tr>");
        htmlString.Should().Contain("<td");
        htmlString.Should().Contain("colspan='5'");
        htmlString.Should().Contain("align='center'");
        htmlString.Should().Contain(Resources.NoInformationIsAvailable);
    }

    [Fact]
    public void FormatNumber2_Should_Format_Decimal_With_Two_Decimal_Places()
    {
        // Arrange
        decimal value = 1234.567m;

        // Act
        var result = HtmlHelpers.FormatNumber2(value);

        // Assert
        result.Should().Be("1,234.57");
    }

    [Fact]
    public void FormatNumber2_Should_Format_Integer_Correctly()
    {
        // Arrange
        decimal value = 1234;

        // Act
        var result = HtmlHelpers.FormatNumber2(value);

        // Assert
        result.Should().Be("1,234");
    }

    [Fact]
    public void FormatNumber2_Should_Handle_Zero()
    {
        // Arrange
        decimal value = 0;

        // Act
        var result = HtmlHelpers.FormatNumber2(value);

        // Assert
        result.Should().Be("0");
    }

    [Fact]
    public void FormatNumber_Should_Format_Decimal_With_Two_Decimal_Places()
    {
        // Arrange
        decimal value = 1234.567m;

        // Act
        var result = HtmlHelpers.FormatNumber(value);

        // Assert
        result.Should().Be("1,234.57");
    }

    [Fact]
    public void FormatNumber_Should_Format_Integer_With_Zero_Decimals()
    {
        // Arrange
        decimal value = 1234;

        // Act
        var result = HtmlHelpers.FormatNumber(value);

        // Assert
        result.Should().Be("1,234.00");
    }

    [Fact]
    public void FormatNumber_Int_Should_Format_Integer_With_Commas()
    {
        // Arrange
        int value = 1234567;

        // Act
        var result = HtmlHelpers.FormatNumber(value);

        // Assert
        result.Should().Be("1,234,567");
    }

    [Fact]
    public void RenderNumberCell_Should_Render_Td_With_Positive_Value()
    {
        // Arrange
        decimal cellValue = 1234.56m;

        // Act
        var result = HtmlHelpers.RenderNumberCell(cellValue);

        // Assert
        result.Should().Contain("<td");
        result.Should().Contain("</td>");
        result.Should().Contain("1,234.56");
        result.Should().NotContain("negative-value");
    }

    [Fact]
    public void RenderNumberCell_Should_Render_Td_With_Negative_Value_Class()
    {
        // Arrange
        decimal cellValue = -1234.56m;

        // Act
        var result = HtmlHelpers.RenderNumberCell(cellValue);

        // Assert
        result.Should().Contain("<td");
        result.Should().Contain("</td>");
        result.Should().Contain("-1,234.56");
        result.Should().Contain("negative-value");
    }

    [Fact]
    public void RenderNumberCell_With_Custom_Class_Should_Include_Both_Classes()
    {
        // Arrange
        decimal cellValue = 1234.56m;
        string cellClass = "custom-class";

        // Act
        var result = HtmlHelpers.RenderNumberCell(cellValue, cellClass);

        // Assert
        result.Should().Contain("custom-class");
        result.Should().Contain("1,234.56");
    }

    [Fact]
    public void FormatDecimal_Should_Format_Decimal_Without_Red_Color()
    {
        // Arrange
        decimal value = 1234.567m;

        // Act
        var result = HtmlHelpers.FormatDecimal(value, false);

        // Assert
        result.Should().Be("1,234.57");
        result.Should().NotContain("span");
        result.Should().NotContain("negative-value");
    }

    [Fact]
    public void FormatDecimal_Should_Format_Negative_Decimal_With_Red_Color()
    {
        // Arrange
        decimal value = -1234.567m;

        // Act
        var result = HtmlHelpers.FormatDecimal(value, true);

        // Assert
        result.Should().Contain("<span class='negative-value'>");
        result.Should().Contain("</span>");
        result.Should().Contain("-1,234.57");
    }

    [Fact]
    public void FormatDecimal_Should_Format_Positive_Decimal_Without_Red_Color()
    {
        // Arrange
        decimal value = 1234.567m;

        // Act
        var result = HtmlHelpers.FormatDecimal(value, true);

        // Assert
        result.Should().Be("1,234.57");
        result.Should().NotContain("span");
    }

    [Fact]
    public void FormatThreshold_With_Value_Should_Format_Decimal()
    {
        // Arrange
        decimal? number = 1234.56m;

        // Act
        var result = HtmlHelpers.FormatThreshold(number);

        // Assert
        result.Should().Be("1,234.56");
    }

    [Fact]
    public void FormatThreshold_With_Null_Value_Should_Return_Unlimited()
    {
        // Arrange
        decimal? number = null;

        // Act
        var result = HtmlHelpers.FormatThreshold(number);

        // Assert
        result.Should().Be(Resources.Unlimited);
    }
}
