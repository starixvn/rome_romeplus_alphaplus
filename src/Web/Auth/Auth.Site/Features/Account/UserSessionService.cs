using AlphaPlus.Core.Models;
using System.Text.RegularExpressions;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Auth.Site.Features.Account;

public interface IUserSessionService
{
    void SetUserSessionValues(SignInResult signInResult);
}

public class UserSessionService(IUserSession userSession, IConfiguration configuration) : IUserSessionService
{
    private readonly IUserSession userSession = userSession;
    private readonly IConfiguration configuration = configuration;

    private const string CustomerDateLimitSessionKey = "CustomerDateLimit";
    private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";
    private const string EnableJackpotSessionKey = "EnableJackpot";

    public void SetUserSessionValues(SignInResult signInResult)
    {
        userSession.CustId = signInResult.CustId;
        userSession.SubAccId = signInResult.SubAccId;
        userSession.AdminId = signInResult.AdminId;
        userSession.UserName = signInResult.UserName;
        userSession.SubUserName = signInResult.SubUserName;
        userSession.NickName = signInResult.NickName;
        userSession.IsNickname = signInResult.IsNickname;
        userSession.RoleId = signInResult.RoleId;
        userSession.ServerId = signInResult.ServerId;
        userSession.UserLoginIp = signInResult.UserLoginIp;
        userSession.LangKey = signInResult.LangKey;
        userSession.Currency = signInResult.Currency;
        userSession.CurrencyId = signInResult.CurrencyId;
        userSession.Site = signInResult.Site;
        userSession.SRecommendId = signInResult.SRecommendId;
        userSession.MRecommendId = signInResult.MRecommendId;
        userSession.IsLogged = signInResult.IsLogged;
        userSession.IsPasswordExpired = signInResult.IsPasswordExpired;
        userSession.IsForcedSecurityCode = signInResult.IsForcedSecurityCode;
        userSession.SecurityCode = signInResult.SecurityCode;
        userSession.AdminName = signInResult.AdminName;
        userSession.SubPageAccess = signInResult.SubPageAccess;
        userSession.DarkModeName = signInResult.DarkModeName;
        userSession.AssetAppPath = signInResult.AssetAppPath;
        userSession.IsProtectedMode = signInResult.IsProtectedMode;
        userSession.IsOfferProtectedMode = signInResult.IsOfferProtectedMode;

        var siteDB = signInResult.SiteDB ?? new SiteDB();
        userSession.AGIP = siteDB.AGIP;
        userSession.SiteDBGroupId = siteDB.SiteDBGroupId;
        userSession.CustDBGroupId = siteDB.CustDBGroupId;

        var subAccountPermission_LockAdminAccounts = configuration.GetValue<string>("SubAccountPermission_LockAdminAccounts");
        if ((!string.IsNullOrWhiteSpace(subAccountPermission_LockAdminAccounts)
            && Regex.IsMatch(string.Format(",{0},", subAccountPermission_LockAdminAccounts), string.Format(",{0},", userSession.AdminId), RegexOptions.IgnoreCase)
            && userSession.IsInternal)
            || (!signInResult.IsAdminCanUpdateSubAccount && userSession.IsInternal))
        {
            userSession.IsLimitedHostPermissionOnSubAccountFunction = true;
        }
        else
        {
            userSession.IsLimitedHostPermissionOnSubAccountFunction = false;
        }

        userSession.SetProperty(CustomerDateLimitSessionKey, signInResult.DateLimit);
        userSession.SetProperty(IsAdminCanUpdateSubAccountSessionKey, signInResult.IsAdminCanUpdateSubAccount);

        userSession.IsEnabledSecurityCodeInBetLists = signInResult.IsEnabledSecurityCodeInBetLists;
        userSession.IsEnabledSecurityCodeInStatement = signInResult.IsEnabledSecurityCodeInStatement;

        userSession.AutoFollowBetIsDummyAgent = signInResult.AutoFollowBetIsDummyAgent;

        userSession.SetProperty(EnableJackpotSessionKey, signInResult.OfferedJackpotProducts);

        userSession.IsInternalWebVPN = signInResult.AdminId > 0 && AlphaContext.Current.Request.Query["isVpn"] == "1";

        userSession.IndirectCustId = signInResult.UplineCustId;
        userSession.IndirectSubAccId = signInResult.UplineSubAccId;
        userSession.IndirectUsername = signInResult.UplineUsername;
    }
}