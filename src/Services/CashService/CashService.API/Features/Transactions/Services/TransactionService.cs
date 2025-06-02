namespace AlphaPlus.CashService.API.Features.Transactions.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria;
using Fanex.Data.Repository;
using Fanex.Logging;
using Models;
using Shared.Requests;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, int siteDbGroupId);

    Task<ResultModel> RevertDepositTransactionToPending(RevertDepositTransactionToPendingCommand command);

    Task<ResultModel> RevertWithdrawalTransactionToPending(RevertWithdrawalTransactionToPendingCommand command);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId);

    void OnAfterUpdateTransaction(string cashAccumCreditTbl, int siteDbGroupId, int custDbGroupId, List<WriteLogDBCommandModel> logs);
}

public class TransactionService(IDynamicRepository dynamicRepository, ICommonService commonService, ILogger logger)
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

    public void OnAfterUpdateTransaction(string cashAccumCreditTbl, int siteDbGroupId, int custDbGroupId, List<WriteLogDBCommandModel> logs)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await SyncCashAccumulateCreditAsync(cashAccumCreditTbl, custDbGroupId);
                await WriteLogAsync(logs, siteDbGroupId, custDbGroupId);
            }
            catch (Exception exception)
            {
                await logger.ErrorAsync(exception.Message, exception);
            }
        });
    }

    private async Task SyncCashAccumulateCreditAsync(string cashAccumCreditTbl, int custDbGroupId)
    {
        if (!string.IsNullOrWhiteSpace(cashAccumCreditTbl))
        {
            var command = new SyncCashAccumCreditCommand
            {
                CashAccumCreditTbl = cashAccumCreditTbl,
                CustDbGroupId = custDbGroupId
            };
            await dynamicRepository.ExecuteAsync(command);

            if (command.ErrCode != 0)
            {
                await logger.ErrorAsync($"SyncCashAccumulateCreditAsync failed: {command.ErrMsg}");
            }
        }
    }

    private async Task WriteLogAsync(List<WriteLogDBCommandModel> logs, int siteDbGroupId, int custDbGroupId)
    {
        if (logs.Count > 0)
        {
            foreach (var logItem in logs)
            {
                switch (logItem.LogTypeId)
                {
                    case 1:
                        var logModel = new TransactionLog
                        {
                            UserId = logItem.UserId,
                            FromCustId = logItem.FromCustId,
                            ToCustId = logItem.ToCustId,
                            IP = logItem.Ip,
                            Doing = logItem.Doing,
                        };
                        await WriteTransactionLog(logModel, siteDbGroupId);
                        break;

                    case 2 or 3 or 4:
                        await WriteLogRunSp(logItem, custDbGroupId);
                        break;
                }
            }
        }
    }

    private async Task WriteTransactionLog(TransactionLog transactionLog, int siteDbGroupId)
    {
        try
        {
            var command = new InsertTransactionLogTransferCommand(transactionLog, -1);
            await dynamicRepository.ExecuteAsync(command);
        }
        catch (Exception exception)
        {
            await logger.ErrorAsync(exception.Message, exception);
            var command = new InsertTransactionLogTransferCommand(transactionLog, siteDbGroupId);
            await dynamicRepository.ExecuteAsync(command);
        }
    }

    private async Task WriteLogRunSp(WriteLogDBCommandModel log, int siteDbGroupId)
    {
        var request = new BaseActionRequest
        {
            ActorId = log.CustId,
            CustDBGroupId = siteDbGroupId,
            UserId = log.UserId,
        };
        await commonService.WriteLogDBCommand(request, log);
    }
}