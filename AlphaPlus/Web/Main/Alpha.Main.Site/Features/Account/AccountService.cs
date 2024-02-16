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
            return false;
        }

        userSessionService.SetUserSessionValues(result);

        session.UserLoginIp = param.RequestInfo.LoginIp;

        return true;
    }
}