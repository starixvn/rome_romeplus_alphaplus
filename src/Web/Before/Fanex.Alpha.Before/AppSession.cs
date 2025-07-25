using Fanex.Alpha.Before.Constants;
using Fanex.Alpha.Before.Extensions;
using Fanex.Alpha.Before.Helpers;

namespace Fanex.Alpha.Before;

public static class AppSession
{
    private static ISession CurrentSession => HttpContextHelper.Current.Session;

    public static string Language
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.LANGUAGE);
        }

        set
        {
            CurrentSession.Set(SessionConstants.LANGUAGE, value);
        }
    }

    public static string LangKey
    {
        get
        {
            return Language switch
            {
                "en-US" => "E",
                "ja-JP" => "J",
                "zh-TW" => "CT",// Traditional
                "zh-CN" => "CS",// Simplified
                "th-TH" => "TH",
                "ko-KR" => "KO",
                "vi-VN" => "VI",
                _ => "E",
            };
        }
    }

    public static string EncryptCode
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.ENCRYPTCODE);
        }

        set
        {
            CurrentSession.Set<string>(SessionConstants.ENCRYPTCODE, value);
        }
    }


    public static string CookieCode
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.COOKIECODE);
        }

        set
        {
            CurrentSession.Set<string>(SessionConstants.COOKIECODE, value);
        }
    }

    public static string Username
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.USERNAME);
        }

        set
        {
            CurrentSession.Set<string>(SessionConstants.USERNAME, value);
        }
    }

    public static string DarkModeName
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.DARKMODENAME) ?? string.Empty;
        }

        set
        {
            CurrentSession.Set(SessionConstants.DARKMODENAME, value);
        }
    }
}
