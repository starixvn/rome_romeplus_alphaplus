using System.Text;
using System.Text.Json;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.Account;

public interface IAccountService
{
    Task<SignInResult> AfterSignIn(SignInParam param);

    Task CheckIn();

    Task<ReCheckInResult> ReCheckIn(int onlineUserId);

    Task CheckOut(string reason = null);

    Task<SignInResult> InternalLogin(InternalLoginParam param);

    Task<SignInResult> IndirectLogin(IndirectLoginParam param);
}

public class AccountService(HttpClient client, IUserSession session, IUserSessionService userSessionService) : IAccountService
{
    private readonly HttpClient client = client;
    private readonly IUserSession session = session;
    private readonly IUserSessionService userSessionService = userSessionService;

    public async Task CheckIn()
    {
        var onlineUserId = await client.PostAsJsonAsync<int>("api/auth/check-in", new
        {
            session.CustId,
            session.SubAccId,
            SessionId = session.NetSessionId,
            session.ServerId,
            IP = session.UserLoginIp
        });

        session.OnUserId = onlineUserId;
    }

    public Task<ReCheckInResult> ReCheckIn(int onlineUserId)
        => client.GetAsJsonAsync<ReCheckInResult>($"api/auth/re-check-in?onlineUserId={onlineUserId}");

    public async Task CheckOut(string reason = null)
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

    public async Task<SignInResult> AfterSignIn(SignInParam param)
    {
        var result = await client.PostAsJsonAsync<SignInResult>("api/auth/after", new
        {
            param.CustId,
            param.Password,
            param.SubAccId,
            param.Nickname,
            param.IsNickname,
            param.LoginTime,
            param.Language,
            param.RequestInfo.LoginIp,
            param.RequestInfo.CountryCode,
            param.RequestInfo.LoginDomain,
            param.RequestInfo.BrowserName,
            param.RequestInfo.Platform,
            SessionId = session.NetSessionId
        });

        if (result.IsFailed)
        {
            return result;
        }

        result.DarkModeName = param.DarkModeName;
        result.AssetAppPath = param.AssetAppPath;
        result.DCSStaticAppPath = param.DCSStaticAppPath;

        userSessionService.SetUserSessionValues(result);

        session.UserLoginIp = param.RequestInfo.LoginIp;

        return result;
    }

    [HttpGet]
    public async Task<SignInResult> InternalLogin(InternalLoginParam param)
    {
        var query = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(param.AdminId), param.AdminId.ToString()),
            KeyValuePair.Create(nameof(param.Language), param.Language),
            KeyValuePair.Create(nameof(param.RequestInfo.CountryCode), param.RequestInfo.CountryCode),
            KeyValuePair.Create(nameof(param.Username), param.Username),
            KeyValuePair.Create(nameof(param.RequestInfo.BrowserName), param.RequestInfo.BrowserName),
            KeyValuePair.Create(nameof(param.RequestInfo.LoginDomain), param.RequestInfo.LoginDomain),
            KeyValuePair.Create(nameof(param.RequestInfo.LoginIp), param.RequestInfo.LoginIp),
            KeyValuePair.Create(nameof(param.LoginTime), param.LoginTime.ToString()),
            KeyValuePair.Create(nameof(param.RequestInfo.Platform), param.RequestInfo.Platform)
        });

        var result = await client.GetAsJsonAsync<SignInResult>($"api/auth/internal-login{query}");

        if (result.IsSuccessful)
        {
            userSessionService.SetUserSessionValues(result);
        }

        return result;
    }

    [HttpGet]
    public async Task<SignInResult> IndirectLogin(IndirectLoginParam param)
    {
        var query = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(param.CustId), param.CustId.ToString()),
            KeyValuePair.Create(nameof(param.UplineCustId), param.UplineCustId.ToString()),
            KeyValuePair.Create(nameof(param.UplineSubAccId), param.UplineSubAccId.ToString()),
            KeyValuePair.Create(nameof(param.UplineUsername), param.UplineUsername),
            KeyValuePair.Create(nameof(param.SubAccId), param.SubAccId.ToString()),
            KeyValuePair.Create(nameof(param.Language), param.Language),
            KeyValuePair.Create(nameof(param.RequestInfo.CountryCode), param.RequestInfo.CountryCode),
            KeyValuePair.Create(nameof(param.Username), param.Username),
            KeyValuePair.Create(nameof(param.RequestInfo.BrowserName), param.RequestInfo.BrowserName),
            KeyValuePair.Create(nameof(param.RequestInfo.LoginDomain), param.RequestInfo.LoginDomain),
            KeyValuePair.Create(nameof(param.RequestInfo.LoginIp), param.RequestInfo.LoginIp),
            KeyValuePair.Create(nameof(param.LoginTime), param.LoginTime.ToString()),
            KeyValuePair.Create(nameof(param.RequestInfo.Platform), param.RequestInfo.Platform)
        });

        var result = await client.GetAsJsonAsync<SignInResult>($"api/auth/indirect-login{query}");

        if (result.IsSuccessful)
        {
            result.DarkModeName = param.DarkModeName;
            result.AssetAppPath = param.AssetAppPath;

            userSessionService.SetUserSessionValues(result);
        }

        return result;
    }
}