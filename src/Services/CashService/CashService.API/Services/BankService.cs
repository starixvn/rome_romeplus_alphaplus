namespace AlphaPlus.CashService.API.Services;

using Criteria.Bank;
using DTOs.Common;
using Fanex.Data.Repository;

public interface IBankService
{
    Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType, string siteDbIp = "");

    Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId, string siteDbIp = "");

    Task<IEnumerable<BankInfo>> GetCashAgentProviderAsync(int agentId, int transactionType, string siteDbIp = "");
}

public class BankService(IDynamicRepository dynamicRepository) : IBankService
{
    public async Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType, string siteDbIp = "")
    {
        var criteria = new GetCashAgentBankInfoCriteria
        {
            AgentId = agentId,
            TransType = transactionType
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria, siteDbIp);
        return bankList;
    }

    public async Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId, string siteDbIp = "")
    {
        var criteria = new GetCashMemberBankInfoCriteria
        {
            CustId = custId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria, siteDbIp);

        return bankList;
    }

    public async Task<IEnumerable<BankInfo>> GetCashAgentProviderAsync(int agentId, int transactionType, string siteDbIp = "")
    {
        var criteria = new GetCashAgentProviderCriteria
        {
            AgentId = agentId,
            TransType = transactionType
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria, siteDbIp);
        return bankList;
    }
}