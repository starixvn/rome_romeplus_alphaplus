using System.Collections;
using System.Text;
using System.Text.Json;
using Alpha.Main.Extensions;
using Alpha.Main.Models;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Features.Account;

public interface IAccountService
{
    Task<bool> AfterSignIn(SignInParam param);

    Task CheckIn();

    Task<ReCheckInResult> ReCheckIn(int onlineUserId);

    Task SignOut(string reason = null);
}

public class AccountService(HttpClient client, IUserSession session) : IAccountService
{
    private readonly HttpClient client = client;
    private readonly IUserSession session = session;

    public async Task CheckIn()
    {
        var onlineUserId = await client.PostAsJsonAsync<int>("api/auth/check-in", new
        {
            CustId = session.CustId,
            SubAccId = session.SubAccId,
            SessionId = session.NetSessionId,
            ServerId = session.ServerId,
            IP = session.UserLoginIp
        });

        session.OnUserId = onlineUserId;
    }

    public Task<ReCheckInResult> ReCheckIn(int onlineUserId)
        => client.GetAsJsonAsync<ReCheckInResult>($"api/auth/re-check-in?onlineUserId={onlineUserId}");

    public async Task SignOut(string reason = null)
    {
        using StringContent payload = new(JsonSerializer.Serialize(new
        {
            onlineUserId = session.OnUserId,
            username = session.IsSubAcc ? session.SubUserName : session.UserName,
            nickname = session.IsNickname ? session.NickName : string.Empty,
            roleId = session.RoleId,
            serverId = session.ServerId,
            reason
        }),
        Encoding.UTF8,
        "application/json");

        using HttpResponseMessage response = await client.PostAsync("api/auth/check-out", payload);
        response.EnsureSuccessStatusCode();
    }

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
            Platform = param.RequestInfo.Platform,
            SessionId = session.NetSessionId
        });


        if (result.IsFailed)
        {
            return false;
        }


        SetUserSessionValues(result);

        session.UserLoginIp = param.RequestInfo.LoginIp;

        return true;

    }

    private void SetUserSessionValues(SignInResult signInResult)
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

            if (session.IsSubAcc)
            {
                session.SubPageAccess = (PageAccess)signInResult.SubPageAccess ?? new PageAccess();
            }
            else
            {
                var subPageAccess = new PageAccess();
                subPageAccess.SetFullPermission();
                session.SubPageAccess = subPageAccess;
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

        // SetProductStatus(session, values);
        // SetProductStatusToDictionary(session, values);

        // setUserStatusAction(session, values);

        ProductTransform.TransformProductStatus(session, []);

        try
        {
            session.IsDoubleCommLosing = signInResult.Status.DoubleCommLosing;
            session.IsDisabledAutoMPT = signInResult.Status.DisableAutoMPT;
            session.SRecommendId = signInResult.Status.SRecommend;
            session.MRecommendId = signInResult.Status.MRecommend;
            session.ARecommendId = signInResult.Status.ARecommend;
            session.IsAdminCanViewVNIP = signInResult.Status.IsAdminCanViewVNIP;
            session.IsAllowedTotalBalanceTransfer = signInResult.IsAllowTotalBalanceTransfer;
            session.IsShowConsolidatedReport = signInResult.IsShowConsolidatedReport;
            session.IsShowBetChoiceLimit = signInResult.IsShowBetChoiceLimit;
            session.AGIP = signInResult.SiteDB.AGIP;
            session.SiteDBGroupId = signInResult.SiteDB.SiteDBGroupId;


            session.IsMyanmarOdds = signInResult.Status.IsMyanmarOdds;

            // if (!string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
            //     && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", session.AdminId), RegexOptions.IgnoreCase)
            //     && session.IsInternal)
            // {
            //     session.IsLimitedHostPermissionOnSubAccountFunction = true;
            // }
            // else
            // {
            //     session.IsLimitedHostPermissionOnSubAccountFunction = false;
            // }
        }
        catch { }
    }

    private static string GetSiteName(int roleId)
    {
        var userLevel = Enumeration.FromValue<UserLevel>((byte)roleId);
        return userLevel.DisplayName;
    }

    // private Task<SiteDBConfig> GetSiteDBConfig(int custId)
    //     => dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));
}