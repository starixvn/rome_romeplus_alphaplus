using System.Globalization;

namespace AlphaPlus.Core.Models;

public class UserSessionExt : IUserSession
{
    public string NetSessionId => UserSession.Current.NetSessionId;

    public int AdminId
    {
        get => UserSession.Current.AdminId;
        set => UserSession.Current.AdminId = value;
    }

    public string AdminName
    {
        get => UserSession.Current.AdminName;
        set => UserSession.Current.AdminName = value;
    }

    public int ARecommendId
    {
        get => UserSession.Current.ARecommendId;
        set => UserSession.Current.ARecommendId = value;
    }

    public string CaptchaCode
    {
        get => UserSession.Current.CaptchaCode;
        set => UserSession.Current.CaptchaCode = value;
    }

    public CultureInfo Culture => UserSession.Current.Culture;

    public string Currency
    {
        get => UserSession.Current.Currency;
        set => UserSession.Current.Currency = value;
    }

    public int CurrencyId
    {
        get => UserSession.Current.CurrencyId;
        set => UserSession.Current.CurrencyId = value;
    }

    public int CustId
    {
        get => UserSession.Current.CustId;
        set => UserSession.Current.CustId = value;
    }

    public bool DisableSportsbook2
    {
        get => UserSession.Current.DisableSportsbook2;
        set => UserSession.Current.DisableSportsbook2 = value;
    }

    public string DisplayNickName
    {
        get => UserSession.Current.DisplayNickName;
        set => UserSession.Current.DisplayNickName = value;
    }

    public bool IsAdminCanViewVNIP
    {
        get => UserSession.Current.IsAdminCanViewVNIP;
        set => UserSession.Current.IsAdminCanViewVNIP = value;
    }

    public bool IsAgent => UserSession.Current.IsAgent;

    public bool IsAllowedTotalBalanceTransfer
    {
        get => UserSession.Current.IsAllowedTotalBalanceTransfer;
        set => UserSession.Current.IsAllowedTotalBalanceTransfer = value;
    }

    public bool IsBelongTo9Currency
    {
        get => UserSession.Current.IsBelongTo9Currency;
        set => UserSession.Current.IsBelongTo9Currency = value;
    }

    public bool IsDisabledAutoMPT
    {
        get => UserSession.Current.IsDisabledAutoMPT;
        set => UserSession.Current.IsDisabledAutoMPT = value;
    }

    public bool IsShowBetChoiceLimit
    {
        get => UserSession.Current.IsShowBetChoiceLimit;
        set => UserSession.Current.IsShowBetChoiceLimit = value;
    }

    public bool IsDisabledESports
    {
        get => UserSession.Current.IsDisabledESports;
        set => UserSession.Current.IsDisabledESports = value;
    }

    public bool IsDisabledSuperLive
    {
        get => UserSession.Current.IsDisabledSuperLive;
        set => UserSession.Current.IsDisabledSuperLive = value;
    }

    public bool IsDoubleCommLosing
    {
        get
        {
            return UserSession.Current.IsDoubleCommLosing;
        }

        set
        {
            UserSession.Current.IsDoubleCommLosing = value;
        }
    }

    public bool IsForcedSecurityCode
    {
        get => UserSession.Current.IsForcedSecurityCode;

        set => UserSession.Current.IsForcedSecurityCode = value;
    }

    public bool IsHideSportsbook2
    {
        get => UserSession.Current.IsHideSportsbook2;
        set => UserSession.Current.IsHideSportsbook2 = value;
    }

    public bool IsInternal => UserSession.Current.IsInternal;

    public bool IsInternalWebVPN
    {
        get => UserSession.Current.IsInternalWebVPN;
        set => UserSession.Current.IsInternalWebVPN = value;
    }

    public bool IsLimitedHostPermissionOnSubAccountFunction
    {
        get => UserSession.Current.IsLimitedHostPermissionOnSubAccountFunction;
        set => UserSession.Current.IsLimitedHostPermissionOnSubAccountFunction = value;
    }

    public bool IsLogged
    {
        get => UserSession.Current.IsLogged;
        set => UserSession.Current.IsLogged = value;
    }

    public bool IsMaster => UserSession.Current.IsMaster;

    public bool IsMyanmarOdds
    {
        get => UserSession.Current.IsMyanmarOdds;
        set => UserSession.Current.IsMyanmarOdds = value;
    }

    public bool IsNickname
    {
        get => UserSession.Current.IsNickname;
        set => UserSession.Current.IsNickname = value;
    }

    public bool IsNicknameLoginOnly
    {
        get => UserSession.Current.IsNicknameLoginOnly;
        set => UserSession.Current.IsNicknameLoginOnly = value;
    }

    public bool IsPasswordExpired
    {
        get => UserSession.Current.IsPasswordExpired;
        set => UserSession.Current.IsPasswordExpired = value;
    }

    public bool IsShowConsolidatedReport
    {
        get => UserSession.Current.IsShowConsolidatedReport;
        set => UserSession.Current.IsShowConsolidatedReport = value;
    }

    public bool IsSubAcc => UserSession.Current.IsSubAcc;

    public bool IsSuper => UserSession.Current.IsSuper;

    public string LangKey
    {
        get => UserSession.Current.LangKey;
        set => UserSession.Current.LangKey = value;
    }

    public string Language
    {
        get => UserSession.Current.Language;
        set => UserSession.Current.Language = value;
    }

    public DateTime LastChangedPassword
    {
        get => UserSession.Current.LastChangedPassword;
        set => UserSession.Current.LastChangedPassword = value;
    }

    public int MRecommendId
    {
        get => UserSession.Current.MRecommendId;
        set => UserSession.Current.MRecommendId = value;
    }

    public string NickName
    {
        get => UserSession.Current.NickName;
        set => UserSession.Current.NickName = value;
    }

    public int OnUserId
    {
        get => UserSession.Current.OnUserId;
        set => UserSession.Current.OnUserId = value;
    }

    public byte RoleId
    {
        get => UserSession.Current.RoleId;
        set => UserSession.Current.RoleId = value;
    }

    public string SecurityCode
    {
        get => UserSession.Current.SecurityCode;
        set => UserSession.Current.SecurityCode = value;
    }

    public string SecurityCodeToken
    {
        get => UserSession.Current.SecurityCodeToken;
        set => UserSession.Current.SecurityCodeToken = value;
    }

    public int ServerId
    {
        get => UserSession.Current.ServerId;
        set => UserSession.Current.ServerId = value;
    }

    public bool ShowSecurityCodePopup
    {
        get => UserSession.Current.ShowSecurityCodePopup;
        set => UserSession.Current.ShowSecurityCodePopup = value;
    }

    public bool IsEnabledSecurityCodeInBetLists
    {
        get => UserSession.Current.IsEnabledSecurityCodeInBetLists;
        set => UserSession.Current.IsEnabledSecurityCodeInBetLists = value;
    }

    public bool IsEnabledSecurityCodeInStatement
    {
        get => UserSession.Current.IsEnabledSecurityCodeInStatement;
        set => UserSession.Current.IsEnabledSecurityCodeInStatement = value;
    }

    public string Site
    {
        get => UserSession.Current.Site;
        set => UserSession.Current.Site = value;
    }

    public string SiteName
    {
        get => UserSession.Current.SiteName;
        set => UserSession.Current.SiteName = value;
    }

    public int SRecommendId
    {
        get => UserSession.Current.SRecommendId;
        set => UserSession.Current.SRecommendId = value;
    }

    public int SubAccId
    {
        get => UserSession.Current.SubAccId;
        set => UserSession.Current.SubAccId = value;
    }

    public PageAccess SubPageAccess
    {
        get => UserSession.Current.SubPageAccess;
        set => UserSession.Current.SubPageAccess = value;
    }

    public string SubUserName
    {
        get => UserSession.Current.SubUserName;
        set => UserSession.Current.SubUserName = value;
    }

    public int UserId
    {
        get => UserSession.Current.UserId;
    }

    public string UserLoginIp
    {
        get => UserSession.Current.UserLoginIp;
        set => UserSession.Current.UserLoginIp = value;
    }

    public string UserName
    {
        get => UserSession.Current.UserName;
        set => UserSession.Current.UserName = value;
    }

    public bool IsShowSkipForceChangePassword
    {
        get => UserSession.Current.IsShowSkipForceChangePassword;
        set => UserSession.Current.IsShowSkipForceChangePassword = value;
    }

    public bool IsIndirectLoggedIn
    {
        get => UserSession.Current.IsIndirectLoggedIn;
    }

    public int IndirectCustId
    {
        get => UserSession.Current.IndirectCustId;
        set => UserSession.Current.IndirectCustId = value;
    }

    public int IndirectSubAccId
    {
        get => UserSession.Current.IndirectSubAccId;
        set => UserSession.Current.IndirectSubAccId = value;
    }

    public string IndirectUsername
    {
        get => UserSession.Current.IndirectUsername;
        set => UserSession.Current.IndirectUsername = value;
    }

    public bool IsOfferProtectedMode
    {
        get => UserSession.Current.IsOfferProtectedMode;
        set => UserSession.Current.IsOfferProtectedMode = value;
    }

    public bool IsProtectedMode
    {
        get => UserSession.Current.IsProtectedMode;
        set => UserSession.Current.IsProtectedMode = value;
    }

    public bool GetDisabledProduct(int productId) => UserSession.Current.GetDisabledProduct(productId);

    public bool GetHideProduct(int productId) => UserSession.Current.GetHideProduct(productId);

    public bool GetSyncedProduct(int productId) => UserSession.Current.GetSyncedProduct(productId);

    public void SetDisabledProduct(int productId, bool value) => UserSession.Current.SetDisabledProduct(productId, value);

    public void SetHideProduct(int productId, bool value) => UserSession.Current.SetHideProduct(productId, value);

    public void SetSyncedProduct(int productId, bool value) => UserSession.Current.SetSyncedProduct(productId, value);

    public string DarkModeName
    {
        get => UserSession.Current.DarkModeName;
        set => UserSession.Current.DarkModeName = value;
    }

    public string AssetAppPath
    {
        get => UserSession.Current.AssetAppPath;
        set => UserSession.Current.AssetAppPath = value;
    }

    public bool AutoFollowBetIsDummyAgent
    {
        get => UserSession.Current.AutoFollowBetIsDummyAgent;
        set => UserSession.Current.AutoFollowBetIsDummyAgent = value;
    }

    public string AGIP
    {
        get => UserSession.Current.AGIP;
        set => UserSession.Current.AGIP = value;
    }

    public int SiteDBGroupId
    {
        get => UserSession.Current.SiteDBGroupId;
        set => UserSession.Current.SiteDBGroupId = value;
    }

    public void SetProperty(string propertyName, object value) => UserSession.Current.SetProperty(propertyName, value);

    public T GetProperty<T>(string propertyName, T defaultValue = default) => UserSession.Current.GetProperty(propertyName, defaultValue);
}