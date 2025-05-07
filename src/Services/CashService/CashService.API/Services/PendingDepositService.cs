using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.PendingCommons;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPendingDepositService
{
    Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo();
    Task<IEnumerable<MemberLoginNameInfo>> GetMemberDepositLoginNames();
    Task<ResultModel> InsertDepositAccountInfo(PendingInfoDetail depositInfoDetail);
}

public class PendingDepositService(IDynamicRepository repository) : IPendingDepositService
{
    public async Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo()
    {
        return GetDummyBankAccountInfo();
        //return await repository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());
    }

    public async Task<IEnumerable<MemberLoginNameInfo>> GetMemberDepositLoginNames()
    {
        return GetDummyMemberLoginInfo();
        //return await repository.FetchAsync<MemberLoginNameInfo>(new GetMemberLoginNameCriteria());
    }

    public async Task<ResultModel> InsertDepositAccountInfo(PendingInfoDetail depositInfoDetail)
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
                Username = "John Doe",
                LoginName = "johndoe123",
                FirstName = "John",
                LastName = "Doe",
                Amount = 1000.50m,
                Currency = "USD",
                From = 12345678,
                To = 87654321,
                ReferenceNo = "REF001",
                DateAndTime = DateTime.Now.AddDays(-1)
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
                DateAndTime = DateTime.Now.AddDays(-2)
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
                DateAndTime = DateTime.Now.AddDays(-3)
            }
        };
    }
    
    private static IEnumerable<MemberLoginNameInfo> GetDummyMemberLoginInfo()
    {
        return new List<MemberLoginNameInfo>
        {
            new() { MemberLoginName = "johndoe123", From = 0, To = 1000 },
            new() { MemberLoginName = "janesmith456", From = 0, To = 1000 },
            new() { MemberLoginName = "bjohnson789", From = 0, To = 1000 },
            new() { MemberLoginName = "asmith123", From = 0, To = 100 },
            new() { MemberLoginName = "rbrown456", From = 0, To = 1001 }
        };
    }
}





