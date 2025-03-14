using System.Globalization;

namespace Fanex.Alpha.Before.Extensions;

public static class DateTimeExtensions
{
    public static string ToStringForCulture(this DateTime dateTime)
    {
        var dateTimeFormated = dateTime.ToString(GetDateTimeFormat())
            .Replace("-", "/")
            .Replace("_", ":");

        return GetCurrentCulture() switch
        {
            "zh-CN" => dateTimeFormated.Replace("AM", "上午").Replace("PM", "下午"),
            "zh-TW" => dateTimeFormated.Replace("AM", "上午").Replace("PM", "下午"),
            "ko-KR" => dateTimeFormated.Replace("AM", "오전").Replace("PM", "오후"),
            _ => dateTimeFormated
        };
    }

    private static string GetCurrentCulture() 
        => CultureInfo.CurrentCulture.Name;

    private static string GetDateTimeFormat()
        => GetCurrentCulture() switch
        {
            "zh-CN" => "yyyy-M-d tthh_mm_ss",
            "zh-TW" => "yyyy-M-d tthh_mm_ss",
            "th-TH" => "d/M/yyyy HH:mm:ss",
            "ja-JP" => "yyyy/MM/dd h:mm:ss",
            "ko-KR" => "yyyy. M. d tt h:mm:ss",
            "vi-VN" => "dd/MM/yyyy HH:mm:ss",
            _ => "M/d/yyyy h:mm:ss tt"
        };
}