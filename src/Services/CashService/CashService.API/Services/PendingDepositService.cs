namespace AlphaPlus.CashService.API.Services;

using AlphaPlus.CashService.API.Features.Shared.Models;
using Criteria.PendingDeposit;
using Fanex.Data.Repository;
using Models.PendingCommons;

public interface IPendingDepositService
{
    Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo();

    Task<CreditAmount> GetLimitAmtDepositAsync(int agentId, int custId, string siteDbIp = "");

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, string siteDbIp = "");

    Task<ResultModel> AddManualDepositAsync(CreateManualModel param, string sitDbIp = "");
}

public class PendingDepositService(IDynamicRepository dynamicRepository,
    ICommonService commonService,
    ITransactionService transactionService) : IPendingDepositService
{
    public async Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo()
        => await dynamicRepository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());

    public async Task<decimal> GetAccumulateCreditAsync(int agentId)
    {
        var criteria = new GetAccumulateCreditCriteria
        {
            CustId = agentId
        };
        return await dynamicRepository.GetAsync<decimal>(criteria);
    }

    public async Task<CreditAmount> GetLimitAmtDepositAsync(int agentId, int custId, string siteDbIp)
    {
        var acCumCreditReturn = await GetAccumulateCreditAsync(agentId);

        var criteria = new GetLimitAmtDepositCriteria
        {
            CustId = custId,
            AccumCreditReturn = acCumCreditReturn
        };
        return await dynamicRepository.GetAsync<CreditAmount>(criteria, siteDbIp);
    }

    public async Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, string siteDbIp = "")
    {
        var criteria = new GetMemberSettingDepositCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<MemberSettingDeposit>(criteria, siteDbIp);
    }

    public async Task<ResultModel> AddManualDepositAsync(CreateManualModel param, string sitDbIp = "")
    {
        var siteDbConf = await commonService.GetServerDBConfigByCustIdAsync(param.CustId);
        var memberSettings = await GetMemberSettingDepositAsync(param.CustId, siteDbConf.AGIP_DBCUST);
        var command = new CreateManualDepositCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            AgentGivenCredit = memberSettings.AgentGivenCredit,
            MemberUsedCredit = memberSettings.MemberUsedCredit,
            MinLimitAmt = memberSettings.MinLimitAmt,
            MaxLimitAmt = memberSettings.MaxLimitAmt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
        };

        await dynamicRepository.ExecuteAsync(command, siteDbConf.AGIP);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        if (command.ErrCode == 0)
        {
            SyncCashAccumulatedCredit(command.CashAccumCreditTbl, siteDbConf.CustDBGroupId);
        }
        return result;
    }

    private void SyncCashAccumulatedCredit(string cashAccumulatedCreditTbl, int custDbGroupId)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await transactionService.SyncCashAccumCreditAsync(cashAccumulatedCreditTbl, custDbGroupId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}