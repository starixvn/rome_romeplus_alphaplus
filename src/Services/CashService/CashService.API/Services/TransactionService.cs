namespace AlphaPlus.CashService.API.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Criteria.Transactions;
using Fanex.Data.Repository;
using Models.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, int siteDbGroupId);

    Task<ResultModel> UpdateTransactionStatus(UpdateTransactionStatusRemarkCommand command);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId);

    Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId);
}

public class TransactionService(IDynamicRepository dynamicRepository)
    : ITransactionService
{
    public async Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria)
    {
        var rawData = await dynamicRepository.FetchAsync<TransactionModel>(criteria);

        return new TransactionResponse
        {
            Items = rawData.ToList(),
            TotalCount = criteria.TotalRecords,
        };
    }

    public async Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, int siteDbGroupId)
    {
        var criteria = new GetTransactionDetailCriteria
        {
            TransId = transactionId,
            CustId = custId,
            SiteDbGroupId = siteDbGroupId
        };
        var transaction = await dynamicRepository.GetAsync<TransactionModel>(criteria);
        return transaction;
    }

    public async Task<ResultModel> UpdateTransactionStatus(UpdateTransactionStatusRemarkCommand command)
    {
        var random = new Random();
        var randomValue = random.Next(2);
        var errMsg = string.Empty;
        if (randomValue != 0)
        {
            return new ResultModel(1, "Transaction status updated successfully.");
        }
        return await Task.FromResult(new ResultModel(randomValue, errMsg));
    }

    public async Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId)
    {
        var criteria = new GetMembersCriteria
        {
            CustId = agentId,
            CustDbGroupId = custDbGroupId
        };
        return await dynamicRepository.FetchAsync<MemberInfo>(criteria);
    }

    public async Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId)
    {
        var command = new SyncCashAccumCreditCommand
        {
            CashAccumCreditTbl = cashAccumCreditTbl,
            CustDbGroupId = custDbGroupId
        };
        await dynamicRepository.ExecuteAsync(command);
        return new ResultModel(command.ErrCode, command.ErrMsg);
    }
}