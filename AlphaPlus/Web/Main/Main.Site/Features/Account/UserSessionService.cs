using System.Text.RegularExpressions;
using AlphaPlus.Core.Models;

using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site.Features.Account;
public interface IUserSessionService
{
    void SetUserSessionValues(SignInResult signInResult);
}

public class UserSessionService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUserSession userSession) : IUserSessionService
{
    private const string DCSStaticAppPathSessionKey = "DCSStaicAppPathSessionKey";
    private const string CustomerDateLimitSessionKey = "CustomerDateLimit";
    private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";
    private const string MainDomainSessionKey = "MainDomain";
    private const string EnableJackpotSessionKey = "GameEnable_Jackpot";
    private readonly IConfiguration configuration = configuration;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    private readonly IUserSession userSession = userSession;

    public void SetUserSessionValues(SignInResult signInResult)
    {
        if (userSession == null)
        {
            return;
        }

        userSession.CustId = signInResult.CustId;
        userSession.UserName = signInResult.UserName?.ToUpperInvariant().Trim();
        userSession.SubAccId = signInResult.SubAccId;
        userSession.SubUserName = signInResult.SubUsername;

        if (userSession.IsSubAcc)
        {
            userSession.SubPageAccess = (PageAccess)signInResult.SubPageAccess ?? new PageAccess();
        }
        else
        {
            var subPageAccess = new PageAccess();
            subPageAccess.SetFullPermission();
            userSession.SubPageAccess = subPageAccess;
        }

        userSession.SiteName = GetSiteName(signInResult.RoleId);
        userSession.Site = signInResult.Site;
        userSession.Currency = signInResult.Currency;
        userSession.CurrencyId = signInResult.CurrencyId;
        userSession.RoleId = signInResult.RoleId;
        userSession.ServerId = signInResult.ServerId;
        userSession.AdminId = signInResult.AdminId;
        userSession.AdminName = signInResult.AccUserName;
        userSession.IsLogged = userSession.IsInternal || !signInResult.IsFirstLogin;
        userSession.NickName = signInResult.Nickname?.ToUpperInvariant();
        userSession.IsNickname = signInResult.IsNickName;
        userSession.DisplayNickName = userSession.IsNickname ? userSession.NickName : userSession.UserName;

        // Security code
        userSession.IsForcedSecurityCode = signInResult.IsForcedChangeSecurityCode;
        userSession.SecurityCode = signInResult.SecurityCode;

        try
        {
            DateTime pwdExpiry = signInResult.PwdExpiry;
            userSession.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;

            var defaultDateSkipForceChangePass = new DateTime(1900, 1, 1);
            userSession.IsShowSkipForceChangePassword = userSession.IsLogged && DateTime.Compare(pwdExpiry, defaultDateSkipForceChangePass) > 0;
        }
        catch
        {
            userSession.IsPasswordExpired = false;
            userSession.IsShowSkipForceChangePassword = false;
        }

        SetUserStatusToSession(signInResult.Status);

        userSession.IsAllowedTotalBalanceTransfer = signInResult.IsAllowTotalBalanceTransfer;
        userSession.IsShowConsolidatedReport = signInResult.IsShowConsolidatedReport;
        userSession.IsShowBetChoiceLimit = signInResult.IsShowBetChoiceLimit;
        userSession.AGIP = signInResult.SiteDB.AGIP;
        userSession.SiteDBGroupId = signInResult.SiteDB.SiteDBGroupId;
        userSession.CustDBGroupId = signInResult.SiteDB.CustDBGroupId;

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

        userSession.AutoFollowBetIsDummyAgent = signInResult.AutoFollowBet;

        userSession.SetProperty(EnableJackpotSessionKey, signInResult.OfferedJackpotProducts);

        userSession.IsInternalWebVPN = signInResult.AdminId > 0 && AlphaContext.Current.Request.Query["isVpn"] == "1";

        userSession.IndirectCustId = signInResult.UplineCustId;
        userSession.IndirectSubAccId = signInResult.UplineSubAccId;
        userSession.IndirectUsername = signInResult.UplineUsername;

        userSession.DarkModeName = signInResult.DarkModeName;
        userSession.AssetAppPath = signInResult.AssetAppPath;
        userSession.SetProperty(DCSStaticAppPathSessionKey, signInResult.DCSStaticAppPath);

        userSession.LangKey = GetUserLangKey(signInResult.LanguageUsage);
        userSession.Language = GetUserLanguage(signInResult.LanguageUsage);
        userSession.UserLoginIp = signInResult.LoginIp;
        userSession.OfferedProductGroupIds = signInResult.OfferedProductGroupIds;

        userSession.SetProperty("languageusage", !string.IsNullOrWhiteSpace(signInResult.LanguageUsage) ? GetUserLanguage(signInResult.LanguageUsage) : signInResult.LanguageUsage);
        SetCookieLanguage(userSession.Language);
    }

    private static string GetUserLanguage(string langCodeUsage)
    {
        if (string.IsNullOrWhiteSpace(langCodeUsage))
        {
            var userLanguage = UserSession.Current.IsInternal ?
                "en-US" : AlphaContext.Current.Request.Cookies["LANGUAGE"];
            return userLanguage ?? "en-US";
        }

        return langCodeUsage.ToUpperInvariant()
            switch
        {
            "CS" => "zh-CN",
            "CT" => "zh-TW",
            "TH" => "th-TH",
            "VI" => "vi-VN",
            "KO" => "ko-KR",
            "JP" => "ja-JP",
            _ => "en-US"
        };
    }

    private static string GetUserLangKey(string langCodeUsage)
    {
        if (!string.IsNullOrWhiteSpace(langCodeUsage))
        {
            return langCodeUsage.ToUpperInvariant();
        }

        return UserSession.Current.IsInternal ? "E" :
            AlphaContext.Current.Request.Cookies["LANGUAGE"]
            switch
            {
                "zh-CN" => "CS",
                "zh-TW" => "CT",
                "th-TH" => "TH",
                "vi-VN" => "VI",
                "ko-KR" => "KO",
                "ja-JP" => "JP",
                _ => "E"
            };
    }

    private static string GetSiteName(int roleId)
    {
        var userLevel = Enumeration.FromValue<UserLevel>((byte)roleId);
        return userLevel.DisplayName;
    }

    public void SetUserStatusToSession(IDictionary<string, string> status)
    {
        ProductTransform.TransformProductStatus(userSession, status);

        if (status.TryGetValue("doublecommlosing", out string doubleCommLosing))
        {
            userSession.IsDoubleCommLosing = Convert.ToBoolean(doubleCommLosing);
        }

        if (status.TryGetValue("nickname", out string nickname))
        {
            userSession.NickName = (nickname + string.Empty).ToUpperInvariant();
        }

        if (status.TryGetValue("nicknameloginonly", out string nicknameLoginOnly))
        {
            userSession.IsNicknameLoginOnly = !string.IsNullOrEmpty(nicknameLoginOnly) && Convert.ToBoolean(nicknameLoginOnly);
        }

        if (status.TryGetValue("isnickname", out string isNickname))
        {
            userSession.IsNickname = !string.IsNullOrEmpty(isNickname) && Convert.ToBoolean(isNickname);
        }

        userSession.DisplayNickName = userSession.IsNickname ? userSession.NickName : userSession.UserName;

        if (status.TryGetValue("on_userid", out string onUserId) && !string.IsNullOrEmpty(onUserId))
        {
            userSession.OnUserId = int.Parse(onUserId);
        }


        userSession.IsDisabledAutoMPT = Convert.ToBoolean(status["disableautompt"]);
        userSession.SRecommendId = Convert.ToInt32(status["srecommend"]);
        userSession.MRecommendId = Convert.ToInt32(status["mrecommend"]);
        userSession.ARecommendId = Convert.ToInt32(status["arecommend"]);
        userSession.IsAdminCanViewVNIP = Convert.ToBoolean(status["isadmincanviewvnip"]);

        // TODO: Fanex.Products
        //ProductManager.SetProductsStatusToSession(userData);

        AddIsProtectedModeToSession(status);
        AddMainDomainToSession();
    }

    private void AddIsProtectedModeToSession(IDictionary<string, string> userData)
    {
        var isOfferProtectedMode = IsProtectedModeOfferBySite(userSession.Site);
        var isProtectedMode = isOfferProtectedMode && Convert.ToBoolean(userData["isprotectedmode"]);

        userSession.IsOfferProtectedMode = isOfferProtectedMode;
        userSession.IsProtectedMode = isProtectedMode;

        if (isProtectedMode)
        {
            // Skip force change password
            userSession.IsPasswordExpired = false;

            // Skip force change security code
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                userSession.IsForcedSecurityCode = false;
            }
        }
    }

    private bool IsProtectedModeOfferBySite(string customerSite)
    {
        var protectedModeOfferBySite = GetProtectedModeOfferSites();

        return protectedModeOfferBySite.ContainsKey("*")
            || protectedModeOfferBySite.ContainsKey(customerSite);
    }

    private Dictionary<string, bool> GetProtectedModeOfferSites()
    {
        var configValue = configuration.GetValue<string>("ProtectedMode:OfferSites");

        if (string.IsNullOrWhiteSpace(configValue))
        {
            return [];
        }

        var offerSites = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        configValue
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList()
            .ForEach(site => offerSites[site] = true);

        return offerSites;
    }

    private void AddMainDomainToSession()
    {
        userSession.SetProperty(MainDomainSessionKey, $"//{httpContextAccessor.HttpContext.Request.Host}");
    }

    private void SetCookieLanguage(string language)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var domain = httpContext.Request.Host.ToString();
        var subDomain = domain.Split('.')[0];
        var mainDomain = domain.Replace(subDomain, string.Empty);
        var cookieExpires = DateTime.Now.AddDays(365);
        var cookieOptions = new CookieOptions { Expires = cookieExpires, Path = "/", Domain = mainDomain, HttpOnly = false };

        httpContext.Response.Cookies.Append("LANGUAGE", language, cookieOptions);
        httpContext.Response.Cookies.Append("hidLanguage", language, cookieOptions);
    }
}
