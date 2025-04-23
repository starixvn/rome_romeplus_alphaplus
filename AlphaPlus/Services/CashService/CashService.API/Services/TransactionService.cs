namespace AlphaPlus.CashService.API.Services;

using Criteria.Transactions;
using Enum;
using Fanex.Data.Repository;
using Models.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(int id);
}

public class TransactionService(IDynamicRepository dynamicRepository) : ITransactionService
{
    public async Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria)
    {
        //var transactions = await dynamicRepository.FetchAsync<TransactionModel>(criteria);
        var transactions = await Task.FromResult(GetMockTransactions());

        // Apply filters

        return new TransactionResponse
        {
            Items = transactions.ToList(),
            TotalCount = criteria.TotalItems,
            Page = criteria.Page,
            PageSize = criteria.PageSize
        };
    }

    public async Task<TransactionModel> GetTransactionByIdAsync(int id)
    {
        //return await dynamicRepository.GetAsync<TransactionModel>(new GetTransactionsByIdCriteria { TransactionId = id });
        return await Task.FromResult(GetMockTransactions().FirstOrDefault(t => t.TransactionId == id));
    }

    private static List<TransactionModel> GetMockTransactions()
    {
        return
        [
            new()
            {
                TransactionId = 1,
                Username = "999bb20a00001",
                FirstName = "TB",
                LastName = "BT",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = TransactionStatus.Completed,
                IsDeposit = true,
                TransactionType = TransactionType.AutoDeposit,
                ReferenceNumber = "QW352163991323101",
                DateTime = DateTime.Parse("2025-04-15 22:08:19")
            },

            new()
            {
                TransactionId = 2,
                Username = "user2",
                FirstName = "John",
                LastName = "Doe",
                Amount = 50.0M,
                Currency = "USD",
                From = "AccountA",
                To = "AccountB",
                Status = TransactionStatus.Rejected,
                TransactionType = TransactionType.AutoDeposit,
                ReferenceNumber = "REF123456789",
                DateTime = DateTime.Parse("2025-04-16 10:15:00")
            },

            new()
            {
                TransactionId = 3,
                Username = "user3",
                FirstName = "Jane",
                LastName = "Smith",
                Amount = 100.0M,
                Currency = "EUR",
                From = "Wallet1",
                To = "Wallet2",
                Status = TransactionStatus.ProviderRejected,
                IsDeposit = true,
                TransactionType = TransactionType.AutoDeposit,
                ReferenceNumber = "REF987654321",
                DateTime = DateTime.Parse("2025-04-17 14:30:00")
            },

            new()
            {
                TransactionId = 4    ,
                Username = "user4",
                FirstName = "Alice",
                LastName = "Brown",
                Amount = 75.0M,
                Currency = "GBP",
                From = "Bank1",
                To = "Bank2",
                Status = TransactionStatus.Completed,
                IsDeposit = false,
                TransactionType = TransactionType.ManualWithdrawal,
                ReferenceNumber = "REF456123789",
                DateTime = DateTime.Parse("2025-04-18 09:45:00")
            },

            new()
            {
                TransactionId = 5,
                Username = "user5",
                FirstName = "Bob",
                LastName = "White",
                Amount = 200.0M,
                Currency = "CAD",
                From = "Card1",
                To = "Card2",
                Status = TransactionStatus.Rejected,
                IsDeposit = true,
                TransactionType = TransactionType.AutoDeposit,
                ReferenceNumber = "REF321654987",
                DateTime = DateTime.Parse("2025-04-19 16:20:00")
            },

            new()
            {
                TransactionId = 6,
                Username = "user6",
                FirstName = "Charlie",
                LastName = "Green",
                Amount = 300.0M,
                Currency = "AUD",
                From = "AccountX",
                To = "AccountY",
                Status = TransactionStatus.Expired,
                TransactionType = TransactionType.AutoWithdrawal,
                ReferenceNumber = "REF654987321",
                DateTime = DateTime.Parse("2025-04-20 12:00:00")
            }
        ];
    }
}