namespace AlphaPlus.CashService.API.Services;

using Criteria.Bank;
using DTOs.Common;
using Fanex.Data.Repository;

public interface IBankService
{
    Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType);

    Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId);
}

public class BankService(IDynamicRepository dynamicRepository) : IBankService
{
    public async Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType)
    {
        var criteria = new GetCashAgentBankInfoCriteria
        {
            AgentId = agentId,
            TransType = transactionType
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);
        return bankList;
    }

    public async Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId)
    {
        var criteria = new GetCashMemberBankInfoCriteria
        {
            CustId = custId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);

        return bankList;
    }
}