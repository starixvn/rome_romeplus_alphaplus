namespace AlphaPlus.CashService.API.Services;

using AlphaPlus.CashService.API.Features.Shared.DTOs;
using Criteria.Bank;
using Fanex.Data.Repository;

public interface IBankService
{
    Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType, int custDbGroupId);

    Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId, int custDbGroupId);

    Task<IEnumerable<BankInfo>> GetCashAgentProviderAsync(int agentId, int transactionType, int custDbGroupId);
}

public class BankService(IDynamicRepository dynamicRepository) : IBankService
{
    public async Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType, int custDbGroupId)
    {
        var criteria = new GetCashAgentBankInfoCriteria
        {
            AgentId = agentId,
            TransType = transactionType,
            CustDbGroupId = custDbGroupId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);
        return bankList;
    }

    public async Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId, int custDbGroupId)
    {
        var criteria = new GetCashMemberBankInfoCriteria
        {
            CustId = custId,
            CustDbGroupId = custDbGroupId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);

        return bankList;
    }

    public async Task<IEnumerable<BankInfo>> GetCashAgentProviderAsync(int agentId, int transactionType, int custDbGroupId)
    {
        var criteria = new GetCashAgentProviderCriteria
        {
            AgentId = agentId,
            TransType = transactionType,
            CustDbGroupId = custDbGroupId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);
        return bankList;
    }
}