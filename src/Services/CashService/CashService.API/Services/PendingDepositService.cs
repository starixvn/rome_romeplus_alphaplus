namespace AlphaPlus.CashService.API.Services;

using Criteria.PendingDeposit;
using Fanex.Data.Repository;
using Models;
using Models.PendingCommons;

public interface IPendingDepositService
{
    Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo();

    Task<decimal> GetAccumulateCreditAsync(int custId, string siteDbIp = "");

    Task<CreditAmount> GetLimitAmtDepositAsync(int custId, decimal accumCreditReturn);

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, string siteDbIp = "");

    Task<(ResultModel, int, string)> CreateManualDepositAsync(CreateManualModel param);

    Task<ResultModel> AddManualDepositAsync(CreateManualModel param, string sitDbIp = "");
}

public class PendingDepositService(IDynamicRepository dynamicRepository,
    ICommonService commonService,
    ITransactionService transactionService) : IPendingDepositService
{
    public async Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo()
        => await dynamicRepository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());

    public async Task<decimal> GetAccumulateCreditAsync(int custId, string siteDbIp = "")
    {
        var criteria = new GetAccumulateCreditCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<decimal>(criteria, siteDbIp);
    }

    public async Task<CreditAmount> GetLimitAmtDepositAsync(int custId, decimal accumCreditReturn)
    {
        var criteria = new GetLimitAmtDepositCriteria
        {
            CustId = custId,
            AccumCreditReturn = accumCreditReturn
        };
        return await dynamicRepository.GetAsync<CreditAmount>(criteria);
    }

    public async Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, string siteDbIp = "")
    {
        var criteria = new GetMemberSettingDepositCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<MemberSettingDeposit>(criteria, siteDbIp);
    }

    public async Task<(ResultModel, int, string)> CreateManualDepositAsync(CreateManualModel param)
    {
        var command = new CreateManualDepositCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            AgentGivenCredit = param.AgentGivenCredit,
            MemberUsedCredit = param.MemberUsedCredit,
            MinLimitAmt = param.MinLimitAmt,
            MaxLimitAmt = param.MaxLimitAmt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
        };
        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        return (result, command.TransId, command.CashAccumCreditTbl);
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

        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        if (command.ErrCode == 0)
        {
            SyncCashAccumulatedCredit(command.CashAccumCreditTbl, siteDbConf.AGIP_DBCUST);
        }
        return result;
    }

    private void SyncCashAccumulatedCredit(string cashAccumulatedCreditTbl, string custDbIp = "")
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await transactionService.SyncCashAccumCreditAsync(cashAccumulatedCreditTbl, custDbIp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}