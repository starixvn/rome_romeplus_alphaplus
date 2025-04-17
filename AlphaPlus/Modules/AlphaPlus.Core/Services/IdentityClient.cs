using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Services;

public interface IIdentityClient
{
    Task<AccountStatus> GetCustomerStatus(int custId, int subAccId);
    Task<KickOutInfo> ReCheckIn(int onlineUserId);
    Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId);
    Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId);
    Task<BelongingInfo> CheckBelongingMultiple(int custId, int subAccId, string custIds);
}

public class IdentityClient(HttpClient httpClient) : IIdentityClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<AccountStatus> GetCustomerStatus(int custId, int subAccId)
        => await httpClient.GetAsJsonAsync<AccountStatus>($"api/authorize/customer-status/{custId}/{subAccId}").ConfigureAwait(false);

    public async Task<KickOutInfo> ReCheckIn(int onlineUserId)
        => await httpClient.GetAsJsonAsync<KickOutInfo>($"api/auth/re-check-in?onlineUserId={onlineUserId}").ConfigureAwait(false);

    public async Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(subAccId), subAccId.ToString()),
            KeyValuePair.Create(nameof(transId), transId.ToString()),
            KeyValuePair.Create(nameof(downlineCustId), downlineCustId.ToString())
        });

        return await httpClient.GetAsJsonAsync<BelongingInfo>($"api/authorize/belonging?{queryString}").ConfigureAwait(false);
    }

    public async Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(subAccId), subAccId.ToString()),
            KeyValuePair.Create(nameof(transId), transId.ToString()),
            KeyValuePair.Create(nameof(downlineCustId), downlineCustId.ToString())
        });

        return await httpClient.GetAsJsonAsync<BelongingInfo>($"api/authorize/belonging-consolidatedgroup{queryString}").ConfigureAwait(false);
    }

    public async Task<BelongingInfo> CheckBelongingMultiple(int custId, int subAccId, string custIds)
        => await httpClient.PostAsJsonAsync<BelongingInfo>($"api/authorize/belonging-multiple", new { custId, subAccId, custIds }).ConfigureAwait(false);
}