using System.Globalization;
using System.Text.Json;
using AlphaPlus.Auth.Enumerations;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Auth.Models;

[Serializable]
public class UserSession : IUserSession
{
    private const string DEFAULT_LANGUAGE = "en-US";
    private const string ISBELONGTO9CURRENCY = "ISBELONGTO9CURRENCY";

    /// <summary>
    /// Values: E,J,CT,CS,TH,KO.
    /// </summary>
    private const string LANGKEY = "LANGKEY";

    private const string CAPTCHACODE = "CAPTCHACODE";
    private const string ADMINID = "ADMINID";
    private const string ADMINNAME = "ADMINNAME";
    private const string CUSTID = "CUSTID";
    private const string ROLEID = "ROLEID";
    private const string SERVERID = "SERVERID";
    private const string USERNAME = "USERNAME";
    private const string NICKNAME = "NICKNAME";
    private const string DISPLAYNICKNAME = "DISPLAYNICKNAME";
    private const string ONUSERID = "ONUSERID";
    private const string SUBACCID = "SUBACCID";
    private const string SUBPAGEACCESS = "SUBPAGEACCESS";
    private const string SUBUSERNAME = "SUBUSERNAME";
    private const string SITE = "SITE";
    private const string SITENAME = "SITENAME";
    private const string CURRENCY = "CURRENCY";
    private const string CURRENCYID = "CURRENCYID";
    private const string ISLOGGED = "ISLOGGED";

    private const string ISPASSWORDEXPIRED = "ISPASSWORDEXPIRED";
    private const string LASTCHANGEDPASSWORD = "LASTCHANGEDPASSWORD";

    // Security code
    private const string ISFORCEDSECURITYCODE = "ISFORCEDSECURITYCODE";

    private const string SECURITYCODE = "SECURITYCODE";
    private const string SECURITYCODERESULT = "SECURITYCODERESULT";
    private const string SHOWSECURITYCODEPOPUP = "SHOWSECURITYCODEPOPUP";
    private const string SECURITYCODETOKEN = "SECURITYCODETOKEN";
    private const string ISENABLEDSECURITYCODEINBETLISTS = "ISENABLEDSECURITYCODEINBETLISTS";
    private const string ISENABLEDSECURITYCODEINSTATEMENT = "ISENABLEDSECURITYCODEINSTATEMENT";

    private const string ISNICKNAMELOGINONLY = "ISNICKNAMELOGINONLY";
    private const string ISADMINCANVIEWVNIP = "ISADMINCANVIEWVNIP";
    private const string ISNICKNAME = "ISNICKNAME";
    private const string ISSHOWCONSOLIDATEDREPORT = "ISSHOWCONSOLIDATEDREPORT";
    private const string ISSHOWBETCHOICELIMIT = "ISSHOWBETCHOICELIMIT";

    private const string ISDOUBLECOMMLOSING = "DOUBLECOMMLOSING";

    private const string ISHIDEVIRTUALSPORTS = "ISHIDEVIRTUALSPORTS";
    private const string ISDISABLEDVIRTUALSPORTS = "ISDISABLEDVIRTUALSPORTS";
    private const string ISDISABLEDAUTOMPT = "ISDISABLEDAUTOMPT";

    private const string SRECOMMENDID = "SRECOMMENDID";
    private const string MRECOMMENDID = "MRECOMMENDID";
    private const string ARECOMMENDID = "ARECOMMENDID";

    private const string ISALLOWEDTOTALBALANCETRANSFER = "ISALLOWEDTOTALBALANCETRANSFER";

    private const string LANGUAGE = "LANGUAGE";

    private const string ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION = "ISLIMITHOSTPERMISSIONONSUBACCOUNTFUNCTION";

    private const string ISDISABLEDESPORTS = "ISDISABLEDESPORTS";
    private const string ISMYANMARODDS = "ISMYANMARODDS";
    private const string ISHIDESPORTSBOOK2 = "ISHIDESPORTSBOOK2";
    private const string DISABLESPORTSBOOK2 = "DISABLESPORTSBOOK2";

    private const string DISABLESUPERLIVE = "DISABLESUPERLIVE";

    private const string HIDEPRODUCT = "HIDEPRODUCT";
    private const string DISABLEDPRODUCT = "DISABLEDPRODUCT";
    private const string SYNCEDPRODUCT = "SYNCEDPRODUCT";

    private const string USERLOGINIP = "USERLOGINIP";

    private const string ISSHOWSKIPFORCECHANGPASSWORD = "ISSHOWSKIPFORCECHANGPASSWORD";
    private string asNetSessionId;

    private const string IndirectCustIdSessionKey = "IndirectCustId";
    private const string IndirectSubAccIdSessionKey = "IndirectSubAccId";
    private const string IndirectUsernameSessionKey = "IndirectUsername";

    private const string DARKMODENAMESESSIONKEY = "DARKMODENAME";
    private const string ASSETAPPPATHSESSIONKEY = "ASSETAPPPATH";

    private const string ISOFFERPROTECTEDMODESESSIONKEY = "ISOFFERPROTECTEDMODE";
    private const string ISPROTECTEDMODESESSIONKEY = "ISPROTECTEDMODE";

    public const string AUTOFOLLOWBETISDUMMYAGENTSESSIONKEY = "AUTOFOLLOWBETISDUMMYAGENT";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public HttpContext Current => _httpContextAccessor.HttpContext;

    public string Language
    {
        get => GetProperty(LANGUAGE, DEFAULT_LANGUAGE);
        set => SetProperty(Language, value);
    }

    public string LangKey
    {
        get => GetProperty(LANGKEY, "E");
        set => SetProperty(LANGKEY, value);
    }

    public CultureInfo Culture => new(Language);

    public string CaptchaCode
    {
        get => GetProperty<string>(CAPTCHACODE);
        set => SetProperty(CAPTCHACODE, value);
    }
    public bool IsBelongTo9Currency
    {
        get => GetProperty(ISBELONGTO9CURRENCY, false);
        set => SetProperty(ISBELONGTO9CURRENCY, value);
    }

    public int AdminId
    {
        get => GetProperty(ADMINID, 0);
        set => SetProperty(ADMINID, value);
    }

    public string AdminName
    {
        get => GetProperty<string>(ADMINNAME);
        set => SetProperty(ADMINNAME, value);
    }

    public int CustId
    {
        get => GetProperty(CUSTID, 0);
        set => SetProperty(CUSTID, value);
    }

    public byte RoleId
    {
        get => GetProperty(ROLEID, byte.MinValue);
        set => SetProperty(ROLEID, value);
    }

    public int ServerId
    {
        get => GetProperty(SERVERID, 0);
        set => SetProperty(SERVERID, value);
    }

    public string UserName
    {
        get => GetProperty<string>(USERNAME);
        set => SetProperty(USERNAME, value);
    }

    public string NickName
    {
        get => GetProperty<string>(NICKNAME);
        set => SetProperty(NICKNAME, value);
    }

    public string DisplayNickName
    {
        get => GetProperty<string>(DISPLAYNICKNAME);
        set => SetProperty(DISPLAYNICKNAME, value);
    }

    public bool IsNicknameLoginOnly
    {
        get => GetProperty(ISNICKNAMELOGINONLY, false);
        set => SetProperty(ISNICKNAMELOGINONLY, value);
    }

    public bool IsNickname
    {
        get => GetProperty(ISNICKNAME, false);
        set => SetProperty(ISNICKNAME, value);
    }

    public int OnUserId
    {
        get => GetProperty(ONUSERID, 0);
        set => SetProperty(ONUSERID, value);
    }

    public int SubAccId
    {
        get => GetProperty(SUBACCID, 0);
        set => SetProperty(SUBACCID, value);
    }

    public bool IsSubAcc => SubAccId != 0;

    public bool IsInternal => AdminId != 0;

    public string Site
    {
        get => GetProperty<string>(SITE);
        set => SetProperty(SITE, value);
    }

    public PageAccess SubPageAccess
    {
        get => GetProperty(SUBPAGEACCESS, new PageAccess());
        set => SetProperty(SUBPAGEACCESS, value);
    }

    public string SubUserName
    {
        get => GetProperty<string>(SUBUSERNAME);
        set => SetProperty(SUBUSERNAME, value);
    }

    public string SiteName
    {
        get => GetProperty<string>(SITENAME);
        set => SetProperty(SITENAME, value);
    }

    public int UserId => IsInternal ? AdminId : CustId;

    public string Currency
    {
        get => GetProperty<string>(CURRENCY);
        set => SetProperty(CURRENCY, value);
    }

    public int CurrencyId
    {
        get => GetProperty(CURRENCYID, 0);
        set => SetProperty(CURRENCYID, value);
    }

    public bool IsLogged
    {
        get => GetProperty(ISLOGGED, false);
        set => SetProperty(ISLOGGED, value);
    }

    public bool IsPasswordExpired
    {
        get => GetProperty(ISPASSWORDEXPIRED, false);
        set => SetProperty(ISPASSWORDEXPIRED, value);
    }

    public DateTime LastChangedPassword
    {
        get => GetProperty(LASTCHANGEDPASSWORD, DateTime.MinValue);
        set => SetProperty(LASTCHANGEDPASSWORD, value);
    }

    public bool IsAdminCanViewVNIP
    {
        get => GetProperty(ISADMINCANVIEWVNIP, false);
        set => SetProperty(ISADMINCANVIEWVNIP, value);
    }

    public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(RoleId);

    public bool IsShowConsolidatedReport
    {
        get => GetProperty(ISSHOWCONSOLIDATEDREPORT, false);
        set => SetProperty(ISSHOWCONSOLIDATEDREPORT, value);
    }

    public bool IsForcedSecurityCode
    {
        get => GetProperty(ISFORCEDSECURITYCODE, false);
        set => SetProperty(ISFORCEDSECURITYCODE, value);
    }

    public string SecurityCode
    {
        get => GetProperty<string>(SECURITYCODE);
        set => SetProperty(ISFORCEDSECURITYCODE, value);
    }

    public bool ShowSecurityCodePopup { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string SecurityCodeToken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsEnabledSecurityCodeInBetLists { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsEnabledSecurityCodeInStatement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsHideVirtualSports { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsDisabledVirtualSports { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsHideSportsbook2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool DisableSportsbook2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsDoubleCommLosing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsDisabledAutoMPT => throw new NotImplementedException();

    public int SRecommendId => throw new NotImplementedException();

    public int MRecommendId => throw new NotImplementedException();

    public int ARecommendId => throw new NotImplementedException();

    public bool IsAllowedTotalBalanceTransfer => throw new NotImplementedException();

    public string NetSessionId => throw new NotImplementedException();

    public bool IsLimitedHostPermisionOnSubAccountFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsShowBetChoiceLimit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsDisabledESports { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsMyanmarOdds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsDisabledSuperLive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string UserLoginIp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsInternalWebVPN { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsShowSkipForceChangePassword { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsIndirectLoggedIn => throw new NotImplementedException();

    public int IndirectCustId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int IndirectSubAccId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string IndirectUsername { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string DarkModeName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string AssetAppPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsOfferProtectedMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsProtectedMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool AutoFollowBetIsDummyAgent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool GetDisabledProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public bool GetHideProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public T GetProperty<T>(string propertyName, T defaultValue = default)
    {
        var value = _httpContextAccessor.HttpContext.Session.GetString(propertyName);
        return value == null ? defaultValue : JsonSerializer.Deserialize<T>(value);
    }

    public bool GetSyncedProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public void SetDisabledProduct(int productId, bool value)
    {
        throw new NotImplementedException();
    }

    public void SetHideProduct(int productId, bool value)
    {
        throw new NotImplementedException();
    }

    public void SetProperty(string propertyName, object value)
    {
        _httpContextAccessor.HttpContext.Session.SetString(propertyName, JsonSerializer.Serialize(value));
    }

    public void SetSyncedProduct(int productId, bool value)
    {
        throw new NotImplementedException();
    }
}