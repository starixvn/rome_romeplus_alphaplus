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

    #region Robot Captcha

    public static int CustId
    {
        get
        {
            return CurrentSession.Get<int>(SessionConstants.CUSTID);
        }

        set
        {
            CurrentSession.Set(SessionConstants.CUSTID, value);
        }
    }

    public static int SubAccId
    {
        get
        {
            return CurrentSession.Get<int>(SessionConstants.SUBACCID);
        }

        set
        {
            CurrentSession.Set(SessionConstants.SUBACCID, value);
        }
    }

    public static string SubAccName
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.SUBACCNAME);
        }

        set
        {
            CurrentSession.Set(SessionConstants.SUBACCNAME, value);
        }
    }

    public static bool IsNickname
    {
        get
        {
            return CurrentSession.Get<bool>(SessionConstants.ISNICKNAME);
        }

        set
        {
            CurrentSession.Set(SessionConstants.ISNICKNAME, value);
        }
    }

    public static string Nickname
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.NICKNAME);
        }

        set
        {
            CurrentSession.Set(SessionConstants.NICKNAME, value);
        }
    }

    public static string EncryptPassword
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.ENCRYPTPASSWORD);
        }

        set
        {
            CurrentSession.Set(SessionConstants.ENCRYPTPASSWORD, value);
        }
    }

    public static int SuperId
    {
        get
        {
            return CurrentSession.Get<int>(SessionConstants.SUPERID);
        }

        set
        {
            CurrentSession.Set(SessionConstants.SUPERID, value);
        }
    }

    public static string Site
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.SITE) ?? string.Empty;
        }

        set
        {
            CurrentSession.Set(SessionConstants.SITE, value);
        }
    }

    public static byte RoleId
    {
        get
        {
            return CurrentSession.Get<byte>(SessionConstants.ROLEID);
        }

        set
        {
            CurrentSession.Set(SessionConstants.ROLEID, value);
        }
    }

    public static string DeviceId
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.DEVICEID);
        }

        set
        {
            CurrentSession.Set(SessionConstants.DEVICEID, value);
        }
    }

    public static string BrowserSize
    {
        get
        {
            return CurrentSession.Get<string>(SessionConstants.BROWSERSIZE);
        }

        set
        {
            CurrentSession.Set(SessionConstants.BROWSERSIZE, value);
        }
    }

    public static void ResetSession()
    {
        CustId = 0;
        Username = null;
        SubAccId = 0;
        SubAccName = null;
        IsNickname = false;
        Nickname = null;
        EncryptPassword = null;
        SuperId = 0;
        Site = null;
        RoleId = 0;
        DeviceId = null;
        BrowserSize = null;
    }

    #endregion
}
