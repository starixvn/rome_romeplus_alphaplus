using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.PendingCommons;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPendingWithdrawalService
{
    Task<IEnumerable<BankAccountInfo>> GetBankWithdrawalAccounts();
    Task<IEnumerable<MemberLoginNameInfo>> GetMemberWithdrawalLoginNames();
    Task<ResultModel> InsertWithdrawAccountInfo(PendingInfoDetail withdrawalInfoDetail);
}

public class PendingWithdrawalService(IDynamicRepository repository) : IPendingWithdrawalService
{
    public async Task<IEnumerable<BankAccountInfo>> GetBankWithdrawalAccounts()
    {
        return GetDummyBankAccountInfo();
        //return await repository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());
    }

    public async Task<IEnumerable<MemberLoginNameInfo>> GetMemberWithdrawalLoginNames()
    {
        return GetDummyMemberLoginInfo();
        //return await repository.FetchAsync<MemberLoginNameInfo>(new GetMemberLoginNameCriteria());
    }

    public async Task<ResultModel> InsertWithdrawAccountInfo(PendingInfoDetail withdrawalInfoDetail)
    {
        return new ResultModel(0, "Success");
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
                DateAndTime = DateTime.Now.AddDays(-2)
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
                DateAndTime = DateTime.Now.AddHours(-12)
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
                DateAndTime = DateTime.Now.AddDays(-4)
            }
        };
    }
    
    private static IEnumerable<MemberLoginNameInfo> GetDummyMemberLoginInfo()
    {
        return new List<MemberLoginNameInfo>
        {
            new() { MemberLoginName = "awilliams321", From = 500, To = 3500 },
            new() { MemberLoginName = "mbrown555", From = 100, To = 1500 },
            new() { MemberLoginName = "edavis789", From = 200, To = 2000 },
            new() { MemberLoginName = "tgreen222", From = 300, To = 2500 },
            new() { MemberLoginName = "swilson444", From = 50, To = 500 }
        };
    }
}
