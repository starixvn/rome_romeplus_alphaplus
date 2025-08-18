using System.Globalization;
using Fanex.Alpha.Before.Extensions;

namespace Fanex.Alpha.Before.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData("en-US", "1/2/2024 3:04:05 PM")] // default format
    [InlineData("vi-VN", "02/01/2024 15:04:05")] // dd/MM/yyyy
    [InlineData("th-TH", "2/1/2567 15:04:05")] // d/M/yyyy (Thai Buddhist calendar)
    [InlineData("ja-JP", "2024/01/02 3:04:05")] // yyyy/MM/dd h:mm:ss
    [InlineData("ko-KR", "2024. 1. 2 오후 3:04:05")] // with 오후
    [InlineData("zh-CN", "2024-1-2 下午03_04_05", true)] // then replace - to / and _ to : and 下午
    [InlineData("zh-TW", "2024-1-2 下午03_04_05", true)]
    public void ToStringForCulture_VariousCultures_FormatsCorrectly(string culture, string expectedStartsWith, bool rawFormat = false)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            var dt = new DateTime(2024, 1, 2, 15, 4, 5);
            var actual = dt.ToStringForCulture();

            if (rawFormat)
            {
                // For zh-* the method first formats then replaces characters
                // We validate the replacements occurred
                Assert.Contains("/", actual);
                Assert.Contains(":", actual);
                Assert.DoesNotContain("-", actual);
                Assert.DoesNotContain("_", actual);
                Assert.True(actual.Contains("上午") || actual.Contains("下午"));
            }
            else
            {
                Assert.Equal(expectedStartsWith, actual);
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}


