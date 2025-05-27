namespace AlphaPlus.CashService.API.Features.Transactions.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Newtonsoft.Json;
using Shared.Requests;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, int siteDbGroupId);

    Task<ResultModel> RevertDepositTransactionToPending(RevertDepositTransactionToPendingCommand command);

    Task<ResultModel> RevertWithdrawalTransactionToPending(RevertWithdrawalTransactionToPendingCommand command);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId);

    Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId);

    void OnAfterUpdateTransactionSuccess(string cashAccumCreditTbl, int custDbGroupId,
        List<WriteLogDBCommandModel> logs);

    void OnAfterUpdateTransactionFailed(int siteDbGroupId, List<WriteLogDBCommandModel> logs);
}

public class TransactionService(IDynamicRepository dynamicRepository, ICommonService commonService)
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

    public async Task<ResultModel> RevertDepositTransactionToPending(RevertDepositTransactionToPendingCommand command)
    {
        await dynamicRepository.ExecuteAsync(command);
        return new ResultModel
        {
            Code = command.ErrCode,
            Message = command.ErrMsg
        };
    }

    public async Task<ResultModel> RevertWithdrawalTransactionToPending(RevertWithdrawalTransactionToPendingCommand command)
    {
        await dynamicRepository.ExecuteAsync(command);
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

    private async Task WriteTransactionLog(TransactionLog transactionLog, int siteDbGroupId)
    {
        var command = new InsertTransactionLogTransferCommand(transactionLog, siteDbGroupId);
        await dynamicRepository.ExecuteAsync(command);
    }

    public void OnAfterUpdateTransactionSuccess(string cashAccumCreditTbl, int custDbGroupId, List<WriteLogDBCommandModel> logs)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await SyncCashAccumCreditAsync(cashAccumCreditTbl, custDbGroupId);
                await WriteLogTransfer(logs, -1);
                await WriteLogRunSp(logs, siteDbGroupId: -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public void OnAfterUpdateTransactionFailed(int siteDbGroupId, List<WriteLogDBCommandModel> logs)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await WriteLogTransfer(logs, siteDbGroupId);
                await WriteLogRunSp(logs, siteDbGroupId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    private async Task WriteLogTransfer(List<WriteLogDBCommandModel> logs, int siteDbGroupId)
    {
        if (logs.Count > 0)
        {
            foreach (var logItem in logs)
            {
                var logModel = new TransactionLog
                {
                    UserId = logItem.UserId,
                    FromCustId = logItem.FromCustId,
                    ToCustId = logItem.ToCustId,
                    IP = logItem.Ip,
                    Doing = logItem.Doing,
                };
                await WriteTransactionLog(logModel, siteDbGroupId);
            }
        }
    }

    private async Task WriteLogRunSp(List<WriteLogDBCommandModel> logs, int siteDbGroupId)
    {
        if (logs.Count > 0)
        {
            foreach (var logItem in logs)
            {
                var request = new BaseActionRequest
                {
                    ActorId = logItem.CustId,
                    SiteDB = siteDbGroupId,
                    UserId = logItem.UserId,
                };
                await commonService.WriteLogDBCommand(request, logItem);
            }
        }
    }
}