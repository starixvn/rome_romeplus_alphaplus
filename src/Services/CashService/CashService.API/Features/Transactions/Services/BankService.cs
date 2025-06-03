namespace AlphaPlus.CashService.API.Features.Transactions.Services;

using Criteria;
using Fanex.Data.Repository;
using Shared.DTOs;

public interface IBankService
{
    Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType, int custDbGroupId);

    Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId, int custDbGroupId);

    Task<IEnumerable<PaymentProvider>> GetCashAgentProviderAsync(int agentId, int transactionType, int custDbGroupId);
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

    public async Task<IEnumerable<PaymentProvider>> GetCashAgentProviderAsync(int agentId, int transactionType, int custDbGroupId)
    {
        var criteria = new GetCashAgentProviderCriteria
        {
            AgentId = agentId,
            TransType = transactionType,
            CustDbGroupId = custDbGroupId
        };
        var bankList = await dynamicRepository.FetchAsync<PaymentProvider>(criteria);
        return bankList;
    }
}