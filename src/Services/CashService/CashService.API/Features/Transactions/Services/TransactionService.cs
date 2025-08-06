namespace AlphaPlus.CashService.API.Features.Transactions.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;
using AlphaPlus.CashService.API.Features.PendingDeposit.Models;
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

    Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, short siteDbGroupId);

    Task<ResultModel> RevertDepositTransactionToPending(RevertTransaction model);

    Task<ResultModel> RevertWithdrawalTransactionToPending(RevertTransaction model);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, short custDbGroupId);

    void OnAfterUpdateTransaction(string cashAccumCreditTbl, short siteDbGroupId, short custDbGroupId, List<WriteLogDBCommandModel> logs);

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, int custDbGroupId);
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

    public async Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, int custDbGroupId)
    {
        var memberSetting = new MemberSettingDeposit();
        var criteria = new GetMemberSettingDepositCriteria
        {
            CustId = custId,
            CustDbGroupId = custDbGroupId
        };
        await dynamicRepository.GetAsync<MemberSettingDeposit>(criteria);
        memberSetting.AgentGivenCredit = criteria.AgentGivenCredit;
        memberSetting.CreditMembers_GivenCredit = criteria.CreditMembers_GivenCredit;

        return memberSetting;
    }

    public async Task<TransactionModel> GetTransactionByIdAsync(long transactionId, int custId, short siteDbGroupId)
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

    public async Task<ResultModel> RevertDepositTransactionToPending(RevertTransaction model)
    {
        var command = new RevertDepositTransactionToPendingCommand
        {
            TransId = model.TransId,
            Remark = model.Remark,
            AgentId = model.UserId,
            UserId = model.UserId,
            SubAccId = model.SubAccId,
            Ip = model.IP,
            SiteDbGroupId = model.SiteDBGroupId,
            CustDbGroupId = model.CustDBGroupId,
        };
        var logs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command);
        OnAfterUpdateTransaction(command.CashAccumCreditTbl, command.SiteDbGroupId, command.CustDbGroupId, logs.ToList());

        return new ResultModel
        {
            Code = command.ErrCode,
            Message = command.ErrMsg
        };
    }

    public async Task<ResultModel> RevertWithdrawalTransactionToPending(RevertTransaction model)
    {
        var memberSettings = await GetMemberSettingDepositAsync(model.CustId, model.CustDBGroupId);
        var command = new RevertWithdrawalTransactionToPendingCommand
        {
            TransId = model.TransId,
            AgentId = model.UserId,
            Remark = model.Remark,
            UserId = model.UserId,
            SubAccId = model.SubAccId,
            Ip = model.IP,
            SiteDbGroupId = model.SiteDBGroupId,
            CustDbGroupId = model.CustDBGroupId,
            AgentGivenCredit = memberSettings.AgentGivenCredit,
            CreditMembersGivenCredit = memberSettings.CreditMembers_GivenCredit
        };
        var logs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command);
        OnAfterUpdateTransaction(command.CashAccumCreditTbl, command.SiteDbGroupId, command.CustDbGroupId, logs.ToList());
        return new ResultModel
        {
            Code = command.ErrCode,
            Message = command.ErrMsg
        };
    }

    public async Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, short custDbGroupId)
    {
        var criteria = new GetMembersCriteria
        {
            CustId = agentId,
            CustDbGroupId = custDbGroupId
        };
        return await dynamicRepository.FetchAsync<MemberInfo>(criteria);
    }

    public void OnAfterUpdateTransaction(string cashAccumCreditTbl, short siteDbGroupId, short custDbGroupId, List<WriteLogDBCommandModel> logs)
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

    private async Task WriteLogAsync(List<WriteLogDBCommandModel> logs, int siteDbGroupId, short custDbGroupId)
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

    private async Task WriteLogRunSp(WriteLogDBCommandModel log, short custDbGroupId)
    {
        var request = new BaseActionRequest
        {
            ActorId = log.CustId,
            CustDBGroupId = custDbGroupId,
            UserId = log.UserId,
        };

        await commonService.WriteLogDBCommand(request, log);
    }
}