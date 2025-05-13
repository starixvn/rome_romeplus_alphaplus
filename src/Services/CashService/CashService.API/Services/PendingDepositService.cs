using AlphaPlus.CashService.API.Criteria.PendingDeposit;
using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.PendingCommons;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPendingDepositService
{
    Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo();

    Task<decimal> GetAccumulateCreditAsync(int custId, string siteDbIp = "");

    Task<CreditAmount> GetLimitAmtDepositAsync(int custId, decimal accumCreditReturn);

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, string siteDbIp = "");

    Task<(ResultModel, int, string)> CreateManualDepositAsync(CreateManualModel param);
}

public class PendingDepositService(IDynamicRepository dynamicRepository) : IPendingDepositService
{
    public async Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo()
    {
        return GetDummyBankAccountInfo();
        //return await repository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());
    }

    private static IEnumerable<BankAccountInfo> GetDummyBankAccountInfo()
    {
        return new List<BankAccountInfo>
        {
            new()
            {
                Id = 1,
                Username = "John Doe",
                LoginName = "johndoe123",
                FirstName = "John",
                LastName = "Doe",
                Amount = 1000.50m,
                Currency = "USD",
                From = 12345678,
                To = 87654321,
                ReferenceNo = "REF001",
                DateAndTime = DateTime.Now
            },
            new()
            {
                Id = 2,
                Username = "Jane Smith",
                LoginName = "janesmith456",
                FirstName = "Jane",
                LastName = "Smith",
                Amount = 2500.75m,
                Currency = "EUR",
                From = 56781234,
                To = 43218765,
                ReferenceNo = "REF002",
                DateAndTime = DateTime.Now.AddMinutes(-30)
            },
            new()
            {
                Id = 3,
                Username = "Bob Johnson",
                LoginName = "bjohnson789",
                FirstName = "Bob",
                LastName = "Johnson",
                Amount = 500.25m,
                Currency = "GBP",
                From = 98765432,
                To = 23456789,
                ReferenceNo = "REF003",
                DateAndTime = DateTime.Now.AddMinutes(-60)
            }
        };
    }

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
}