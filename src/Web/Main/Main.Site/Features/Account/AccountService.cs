using System.Text;
using System.Text.Json;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Account;

public interface IAccountService
{
    Task CheckIn();

    Task<ReCheckInResult> ReCheckIn(int onlineUserId);

    Task CheckOut(string reason = null);
}

public class AccountService(HttpClient client, IUserSession session) : IAccountService
{
    private readonly HttpClient client = client;
    private readonly IUserSession session = session;

    public async Task CheckIn()
    {
        var SubAccId = session.IsInternal ? -session.AdminId : session.SubAccId;
        var onlineUserId = await client.PostAsJsonAsync<int>("api/auth/check-in", new
        {
            session.CustId,
            SubAccId,
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
}