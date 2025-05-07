namespace AlphaPlus.CashService.API.Services;

using AlphaPlus.Shared;
using Criteria.Bank;
using DTOs.Common;
using Fanex.Cryptography.Aes;
using Fanex.Data.Repository;

public interface IBankService
{
    Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType);

    Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId);
}

public class BankService(IDynamicRepository dynamicRepository, IAesService encryptionService) : IBankService
{
    private readonly string _encryptSecretKey = AlphaPlusConstants.EncryptSecretKey;

    public async Task<IEnumerable<BankInfo>> GetCashAgentBankInfoAsync(int agentId, int transactionType)
    {
        var criteria = new GetCashAgentBankInfoCriteria
        {
            AgentId = agentId,
            TransType = transactionType
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);
        return DecryptBankNumber(bankList);
    }

    public async Task<IEnumerable<BankInfo>> GetMemberBankInfoAsync(int custId)
    {
        var criteria = new GetCashMemberBankInfoCriteria
        {
            CustId = custId
        };
        var bankList = await dynamicRepository.FetchAsync<BankInfo>(criteria);

        return DecryptBankNumber(bankList);
    }

    private IEnumerable<BankInfo> DecryptBankNumber(IEnumerable<BankInfo> bankList)
    {
        var decryptBankNumber = bankList as BankInfo[] ?? bankList.ToArray();
        foreach (var bank in decryptBankNumber)
        {
            bank.BankAccountNumber = encryptionService.Decrypt(bank.BankAccountNumber, _encryptSecretKey);
        }
        return decryptBankNumber;
    }
}