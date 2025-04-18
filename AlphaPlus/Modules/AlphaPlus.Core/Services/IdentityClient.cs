using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Services;

public interface IIdentityClient
{
    Task<AccountStatus> GetCustomerStatus(int custId, int subAccId);
    Task<KickOutInfo> ReCheckIn(int onlineUserId);
    Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId);
    Task<BelongingInfo> CheckBelongingInDBCust(int custId, int subAccId, long transId, int downlineCustId);
    Task<DownlineInfo> CheckBelongingByUsername(int custId, int subAccId, string username);
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
        var queryString = CreateQueryString(custId, subAccId, transId, downlineCustId);

        return await httpClient.GetAsJsonAsync<BelongingInfo>($"api/authorize/belonging{queryString}").ConfigureAwait(false);
    }

    public async Task<BelongingInfo> CheckBelongingInDBCust(int custId, int subAccId, long transId, int downlineCustId)
    {
        var queryString = CreateQueryString(custId, subAccId, transId, downlineCustId);

        return await httpClient.GetAsJsonAsync<BelongingInfo>($"api/authorize/belonging-dbcust{queryString}").ConfigureAwait(false);
    }

    public async Task<DownlineInfo> CheckBelongingByUsername(int custId, int subAccId, string username)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(subAccId), subAccId.ToString()),
            KeyValuePair.Create(nameof(username), username)
        });

        return await httpClient.GetAsJsonAsync<DownlineInfo>($"api/authorize/belonging-username{queryString}").ConfigureAwait(false);
    }

    public async Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId)
    {
        var queryString = CreateQueryString(custId, subAccId, transId, downlineCustId);

        return await httpClient.GetAsJsonAsync<BelongingInfo>($"api/authorize/belonging-consolidatedgroup{queryString}").ConfigureAwait(false);
    }

    private static QueryString CreateQueryString(int custId, int subAccId, long transId, int downlineCustId)
        => QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(subAccId), subAccId.ToString()),
            KeyValuePair.Create(nameof(transId), transId.ToString()),
            KeyValuePair.Create(nameof(downlineCustId), downlineCustId.ToString())
        });

    public async Task<BelongingInfo> CheckBelongingMultiple(int custId, int subAccId, string custIds)
        => await httpClient.PostAsJsonAsync<BelongingInfo>($"api/authorize/belonging-multiple", new { custId, subAccId, custIds }).ConfigureAwait(false);
}