namespace AlphaPlus.CashService.API.Features.Transactions.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Newtonsoft.Json;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, int siteDbGroupId);

    Task<ResultModel> RevertTransactionToPending(RevertTransactionToPendingCommand command);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId);

    Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId);

    Task WriteTransactionLog(TransactionLog transactionLog, int? siteDbGroupId);
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

    public async Task<ResultModel> RevertTransactionToPending(RevertTransactionToPendingCommand command)
    {
        await dynamicRepository.ExecuteAsync(command);
        Console.WriteLine(JsonConvert.SerializeObject(command));
        return new ResultModel
        {
            Code = command.ErrCode,
            Message = command.ErrMsg
        };
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

    public async Task WriteTransactionLog(TransactionLog transactionLog, int? siteDbGroupId)
    {
        var command = new InsertTransactionLogTransferCommand(transactionLog, siteDbGroupId);
        await dynamicRepository.ExecuteAsync(command);
    }
}