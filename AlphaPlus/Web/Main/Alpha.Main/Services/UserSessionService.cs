using System.Collections;
using Alpha.Main.Constants;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Alpha.Main.Services;

public interface IUserSessionService
{
    void SetUserStatusToSession(IUserSession session, Hashtable userData);
}

public class UserSessionService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUserSession userSession) : IUserSessionService
{
    private readonly IConfiguration configuration = configuration;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    private readonly IUserSession userSession = userSession;
    private readonly string systemLogin;

    public void SetUserStatusToSession(IUserSession session, Hashtable userData)
    {
        //ProductManager.SetProductsStatusToSession(userData);

        var custId = session.CustId;
        var subAccId = session.SubAccId;
        var isInternal = session.IsInternal;
        var masterId = userData.ContainsKey("mrecommend")
            ? Convert.ToInt32(userData["mrecommend"])
            : 0;

        AddEnabledProductsByGameToSession(session.Site, session.CurrencyId);
        AddCustomerDateLimitToSession(custId, isInternal);

        AddUpdateSubAccountPermissionToSession(session.AdminId, isInternal, systemLogin);

        AddSecurityCodeForBetListsAndStatementToSession(custId, subAccId);
        AddIsProtectedModeToSession(userData);

        AddMainDomainToSession();
        // AddAutoFollowBetIsDummyAgent(custId, session.RoleId, masterId);

        SetDisplayLanguage(userData);
    }

    private void AddEnabledProductsByGameToSession(string site, int currency)
    {
        // foreach (var gameName in configuration.GetValue<string[]>("EnabledGames"))
        // {
        //     // TODO: GetEnabledProductsByGame
        //     // var offeredProducts = productService.GetEnabledProductsByGame();
        //     // userSession.SetProperty(SessionKey.GameEnableSessionKey + gameName, offeredProducts.GetProductBySiteAndCurrency(site, currency));
        // }
    }

    private void AddUpdateSubAccountPermissionToSession(int adminId, bool isInternal, string systemLogin)
    {
        // TODO: 
        // var isAdminCanUpdateSubAccount =
        //     !(isInternal && adminId > 0 && systemLogin.Equals("NAP", StringComparison.Ordinal))
        //     || adminPermissionService.HasUpdateSubAccountPermission(adminId);

        // userSession.SetProperty(SessionKey.IsAdminCanUpdateSubAccountSessionKey, isAdminCanUpdateSubAccount);
    }

    private void AddCustomerDateLimitToSession(int custId, bool isInternal)
    {
        // TODO: GetCustomerDateLimit
        // var dateLimit = customerDateLimitService.GetCustomerDateLimit(custId, isInternal);

        // if (dateLimit != DateTime.MinValue)
        // {
        //     userSession.SetProperty(SessionKey.CustomerDateLimitSessionKey, dateLimit);
        // }
    }

    private void AddSecurityCodeForBetListsAndStatementToSession(int custId, int subAccId)
    {
        // var function = dbUserSessionService.GetSecurityCodeOptionalFunction(custId, subAccId);
        // userSession.IsEnabledSecurityCodeInBetLists = function.BetList;
        // userSession.IsEnabledSecurityCodeInStatement = function.Statement;
    }

    private void AddIsProtectedModeToSession(Hashtable userData)
    {
        // var isOfferProtectedMode = AppSettings.IsProtectedModeOfferBySite(userSession.Site);
        // var isProtectedMode = isOfferProtectedMode && (bool)userData["isprotectedmode"];

        // userSession.IsOfferProtectedMode = isOfferProtectedMode;
        // userSession.IsProtectedMode = isProtectedMode;

        // if (isProtectedMode)
        // {
        //     // Skip force change password
        //     userSession.IsPasswordExpired = false;

        //     // Skip force change security code
        //     if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
        //     {
        //         userSession.IsForcedSecurityCode = false;
        //     }
        // }
    }

    private void AddMainDomainToSession()
    {
        // userSession.SetProperty(
        //     SessionKey.MainDomainSessionKey,
        //     $"//{HttpContext.Current.Request.Headers["Host"]}");
    }

    // private void AddAutoFollowBetIsDummyAgent(int custId, byte roleId, int masterId)
    //     => userSession.AutoFollowBetIsDummyAgent =
    //     RoleIdConstants.Agent == roleId
    //     && AppSettings.AutoFollowBetMasterId == masterId
    //     && autoFollowBetService.IsDummyAgent(custId);

    private void SetDisplayLanguage(Hashtable hashtable)
    {
        const string UsageLanguageKey = "languageusage";
        var languageCode = string.Empty;

        if (hashtable.ContainsKey(UsageLanguageKey))
        {
            var languageKey = (string)hashtable[UsageLanguageKey];

            if (!string.IsNullOrWhiteSpace(languageKey))
            {
                languageCode = Language.CodeKeys[languageKey] ?? null;

                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    userSession.Language = languageCode;
                    userSession.LangKey = languageKey;
                    SetCookieLanguage(languageCode);
                }
            }
        }

        userSession.SetProperty(UsageLanguageKey, languageCode);
    }

    private void SetCookieLanguage(string languageCode)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var domain = httpContext.Request.Host.ToString();
        var subDomain = domain.Split('.')[0];
        var mainDomain = domain.Replace(subDomain, string.Empty);
        var cookieExpires = DateTime.Now.AddDays(365);
        var cookieOptions = new CookieOptions { Expires = cookieExpires, Path = "/", Domain = mainDomain, HttpOnly = false };

        httpContext.Response.Cookies.Append("LANGUAGE", languageCode, cookieOptions);
        httpContext.Response.Cookies.Append("hidLanguage", languageCode, cookieOptions);
    }
}
