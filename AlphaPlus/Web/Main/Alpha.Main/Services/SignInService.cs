using System.Collections;
using Alpha.Main.Extensions;
using Alpha.Main.Models;
using Alpha.Main.Params.Authorization;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Services;

public interface ISignInService
{
    Task<bool> AfterSignIn(SignInParam param);
}

public class SignInService(HttpClient client) : ISignInService
{
    private readonly HttpClient client = client;

    public async Task<bool> AfterSignIn(SignInParam param)
    {
        var result = await client.PostAsJsonAsync<SignInResult>("api/auth/after", new
        {
            CustId = param.CustId,
            Password = param.Password,
            SubAccId = param.SubAccId,
            Nickname = param.Nickname,
            IsNickname = param.IsNickname,
            LoginTime = param.LoginTime,
            Language = param.Language,
            LoginIp = param.RequestInfo.LoginIp,
            CountryCode = param.RequestInfo.CountryCode,
            LoginDomain = param.RequestInfo.LoginDomain,
            BrowserName = param.RequestInfo.BrowserName,
            Platform = param.RequestInfo.Platform
        });


        if (result.Code != 0)
        {
            return false;
        }

        var outputs = new Hashtable
        {
            { "custid", result.CustId },
            { "roleid", result.RoleId },
            { "username", result.UserName },
            { "sitename", GetSiteName(result.RoleId) },
            { "subaccid", param.SubAccId },
            { "subaccname", param.IsSubAcc ? result.SubUsername: string.Empty },
            { "adminid", 0 },
            { "isshowconsolidated", await IsShowConsolidateReport(result.CustId) },
            { "isShowBetChoiceLimit", await IsShownBetChoiceLimit(result.CustId) },
        };

        // TODO: DuplicationLogin
        // if (!isDisabledDuplicationLogin)
        // {
        //     int serverId = (int)outputs["serverid"];
        //     this.dao.Acc_Checkin(custId, subAccId, userSession.NetSessionId, serverId, requestInfo["loginip"].ToString(), out int onUserId);
        //     outputs.Add("on_userid", onUserId);
        // }

        // this.GetCustomerStatus(custId, adminId, ref outputs, systemLogin);

        // var siteDBConfig = await GetSiteDBConfig(command.CustId);
        // outputs.Add("AGIP", siteDBConfig.AGIP);
        // outputs.Add("SiteDBGroupId", siteDBConfig.SiteDBGroupId);
        // UserSession.Current.SetUserSessionValues(outputs, userSessionService.SetUserStatusToSession);

        SetUserSessionValues(UserSession.Current, result);

        return true;

    }

    public void SetUserSessionValues(IUserSession session, SignInResult signInResult)
    {
        if (session == null)
        {
            return;
        }

        try
        {
            session.CustId = signInResult.CustId;
            session.UserName = signInResult.UserName?.ToUpperInvariant().Trim();
            session.SubAccId = signInResult.SubAccId;
            session.SubUserName = signInResult.SubUsername;
            session.SubPageAccess = (PageAccess)signInResult.SubPageAccess ?? new PageAccess();
            if (!session.IsSubAcc)
            {
                session.SubPageAccess.SetFullPermission();
            }

            session.SiteName = GetSiteName(signInResult.RoleId);
            session.Site = signInResult.Site;
            session.Currency = signInResult.Currency;
            session.CurrencyId = signInResult.CurrencyId;
            session.RoleId = signInResult.RoleId;
            session.ServerId = signInResult.ServerId;
            session.AdminId = signInResult.AdminId;
            session.AdminName = signInResult.AccUserName;
            session.IsLogged = session.IsInternal || !signInResult.IsFirstLogin;
            session.NickName = signInResult.Nickname;
            session.IsNickname = signInResult.IsNickName;
            session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

            // Security code
            session.IsForcedSecurityCode = signInResult.IsForcedChangeSecurityCode;
            session.SecurityCode = signInResult.SecurityCode;
        }
        catch
        {
        }


        try
        {
            DateTime pwdExpiry = signInResult.PwdExpiry;
            session.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;

            var defaultDateSkipForceChangePass = new DateTime(1900, 1, 1);
            session.IsShowSkipForceChangePassword = session.IsLogged && DateTime.Compare(pwdExpiry, defaultDateSkipForceChangePass) > 0;
        }
        catch
        {
            session.IsPasswordExpired = false;
            session.IsShowSkipForceChangePassword = false;
        }

        // if (values.Contains("doublecommlosing"))
        // {
        //     session.IsDoubleCommLosing = (bool)values["doublecommlosing"];
        // }




        // if (values.Contains("on_userid") && values["on_userid"] != null && values["on_userid"] != DBNull.Value)
        // {
        //     session.OnUserId = int.Parse(values["on_userid"].ToString());
        // }

        // SetProductStatus(session, values);
        // SetProductStatusToDictionary(session, values);

        // setUserStatusAction(session, values);

        // try
        // {
        //     session.IsDisabledAutoMPT = (bool)values["disableautompt"];
        //     session.SRecommendId = Convert.ToInt32(values["srecommend"]);
        //     session.MRecommendId = Convert.ToInt32(values["mrecommend"]);
        //     session.ARecommendId = Convert.ToInt32(values["arecommend"]);
        //     session.IsAdminCanViewVNIP = (bool)values["isadmincanviewvnip"];
        //     session.IsAllowedTotalBalanceTransfer = (bool)values["isallowedtotalbalancetransfer"];
        //     session.IsShowConsolidatedReport = (bool)values["isshowconsolidated"];
        //     session.IsShowBetChoiceLimit = (bool)values["isShowBetChoiceLimit"];
        //     session.AGIP = values["AGIP"].ToString();
        //     session.SiteDBGroupId = Convert.ToInt32(values["SiteDBGroupId"]);

        //     bool isMyanmarOdds = false;
        //     if (values["ismyanmarodds"] != null && values["ismyanmarodds"] != DBNull.Value)
        //     {
        //         _ = bool.TryParse(values["ismyanmarodds"].ToString(), out isMyanmarOdds);
        //     }

        //     session.IsMyanmarOdds = isMyanmarOdds;

        //     if (!string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
        //         && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", session.AdminId), RegexOptions.IgnoreCase)
        //         && session.IsInternal)
        //     {
        //         session.IsLimitedHostPermissionOnSubAccountFunction = true;
        //     }
        //     else
        //     {
        //         session.IsLimitedHostPermissionOnSubAccountFunction = false;
        //     }
        // }
        // catch { }
    }

    private static string GetSiteName(int roleId)
    {
        var userLevel = Enumeration.FromValue<UserLevel>((byte)roleId);
        return userLevel.DisplayName;
    }

    private Task<bool> IsShowConsolidateReport(int custId)
    {
        return Task.FromResult(true);
        // var command = new CheckIsShowConsolidatedGroupMenuCommand { CustId = custId };
        // await dynamicRepository.ExecuteAsync(command);
        // return command.IsShownMenu;
    }

    private static Task<bool> IsShownBetChoiceLimit(int custId)
    {
        // TODO: IsShownBetChoiceLimit
        return Task.FromResult(false);
    }

    // private Task<SiteDBConfig> GetSiteDBConfig(int custId)
    //     => dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));
}