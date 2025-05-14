using AlphaPlus.CashService.API.Criteria.PendingWithdrawal;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.PendingCommons;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPendingWithdrawalService
{
    Task<IEnumerable<BankAccountInfo>> GetBankWithdrawalAccounts();

    Task<decimal> GetCashBalanceAsync(int custId);

    Task<CreditAmount> GetLimitAmtWithdrawAsync(CashLimitAmt param);

    Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId);

    Task<(ResultModel, int, string)> CreateManualWithdrawAsync(CreateManualModel param);

    Task<ResultModel> AddManualWithdrawAsync(CreateManualModel param);
}

public class PendingWithdrawalService(
    IDynamicRepository dynamicRepository,
    ICommonService commonService,
    ITransactionService transactionService) : IPendingWithdrawalService
{
    public async Task<IEnumerable<BankAccountInfo>> GetBankWithdrawalAccounts()
    {
        return GetDummyBankAccountInfo();
        //return await dynamicRepository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());
    }

    private static IEnumerable<BankAccountInfo> GetDummyBankAccountInfo()
    {
        return new List<BankAccountInfo>
        {
            new()
            {
                Id = 1,
                Username = "Alice Williams",
                LoginName = "awilliams321",
                FirstName = "Alice",
                LastName = "Williams",
                Amount = 3200.00m,
                Currency = "USD",
                From = 44556677,
                To = 77665544,
                ReferenceNo = "REF004",
                DateAndTime = DateTime.Now
            },
            new()
            {
                Id = 2,
                Username = "Michael Brown",
                LoginName = "mbrown555",
                FirstName = "Michael",
                LastName = "Brown",
                Amount = 750.50m,
                Currency = "JPY",
                From = 11223344,
                To = 55667788,
                ReferenceNo = "REF005",
                DateAndTime = DateTime.Now.AddHours(-1)
            },
            new()
            {
                Id = 3,
                Username = "Emma Davis",
                LoginName = "edavis789",
                FirstName = "Emma",
                LastName = "Davis",
                Amount = 1850.25m,
                Currency = "CAD",
                From = 99887766,
                To = 66778899,
                ReferenceNo = "REF006",
                DateAndTime = DateTime.Now.AddMinutes(-20)
            }
        };
    }

    public async Task<decimal> GetCashBalanceAsync(int custId)
    {
        var criteria = new GetCashBalanceCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<decimal>(criteria);
    }

    public async Task<CreditAmount> GetLimitAmtWithdrawAsync(CashLimitAmt param)
    {
        var criteria = new GetLimitAmtWithdrawCriteria
        {
            CustId = param.CustId,
            ProviderId = param.ProviderId,
            UplineId = param.UplineId,
            CashBalance = param.CashBalance
        };
        return await dynamicRepository.GetAsync<CreditAmount>(criteria);
    }

    public async Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId)
    {
        var criteria = new GetMemberSettingWithdrawCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<MemberSettingWithdraw>(criteria);
    }

    public async Task<(ResultModel, int, string)> CreateManualWithdrawAsync(CreateManualModel param)
    {
        var command = new CreateManualWithdrawCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            MinLimitAmt = param.MinLimitAmt,
            MaxLimitAmt = param.MaxLimitAmt,
            WithdrawalLimitCnt = param.WithdrawalLimitCnt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
            ProviderId = param.ProviderId,
        };
        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        return (result, command.TransId, command.CashAccumCreditTbl);
    }

    public async Task<ResultModel> AddManualWithdrawAsync(CreateManualModel param)
    {
        var siteDbConf = await commonService.GetServerDBConfigByCustIdAsync(param.CustId);
        var memberSettings = await GetMemberSettingWithdrawAsync(param.CustId);
        var command = new CreateManualWithdrawCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            MinLimitAmt = memberSettings.MinLimitAmt,
            MaxLimitAmt = memberSettings.MaxLimitAmt,
            WithdrawalLimitCnt = memberSettings.WithdrawalLimitCnt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
            ProviderId = param.ProviderId,
        };
        await dynamicRepository.ExecuteAsync(command, siteDbConf.AGIP);
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