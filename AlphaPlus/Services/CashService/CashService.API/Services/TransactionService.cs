namespace AlphaPlus.CashService.API.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Criteria.Transactions;
using Enum;
using Extensions;
using Fanex.Data.Repository;
using Models;
using Models.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(int transactionId);

    Task<ResultModel> UpdateTransactionStatus(int transactionId, string status);
}

public class TransactionService(IDynamicRepository dynamicRepository) : ITransactionService
{
    public async Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria)
    {
        var allMockTransactions = GetMockTransactions();

        // Basic filtering example (can be expanded based on criteria)
        var filteredTransactions = allMockTransactions
            .Where(t => criteria.Status == "All" || t.Status == criteria.Status)
            .Where(t => criteria.TransactionType == null ||
                       (criteria.TransactionType == "deposit" && t.IsDeposit) ||
                       (criteria.TransactionType == "withdrawal" && !t.IsDeposit))
            .ToList();

        var pagedTransactions = filteredTransactions
            .Skip((criteria.Page - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .ToList();

        return await Task.FromResult(new TransactionResponse
        {
            Items = pagedTransactions,
            TotalCount = filteredTransactions.Count,
            Page = criteria.Page,
            PageSize = criteria.PageSize
        });
    }

    public async Task<TransactionModel> GetTransactionByIdAsync(int transactionId)
    {
        return await Task.FromResult(GetMockTransactions().FirstOrDefault(t => t.TransactionId == transactionId));
    }

    public async Task<ResultModel> UpdateTransactionStatus(int transactionId, string status)
    {
        var random = new Random();
        int randomValue = random.Next(2);
        string errMsg = string.Empty;
        if (randomValue != 0)
        {
            return new ResultModel(1, "Transaction status updated successfully.");
        }
        return await Task.FromResult(new ResultModel(randomValue, errMsg));
    }

    private static List<TransactionModel> GetMockTransactions()
    {
        var baseTime = DateTime.Parse("2025-04-15 22:08:19");
        var transactions = new List<TransactionModel>
        {
            new()
            {
                TransactionId = 1,
                Username = "999bb20a00001",
                FirstName = "Sophia",
                LastName = "Miller",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.Completed),
                IsDeposit = true,
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoDeposit),
                ReferenceNumber = "QW352163991323101",
                DateTime = baseTime.AddSeconds(0)
            },
            new()
            {
                TransactionId = 2,
                Username = "999bb20a00002",
                FirstName = "Jackson",
                LastName = "Davis",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.Rejected),
                IsDeposit = true,
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoDeposit),
                ReferenceNumber = "QW352163991323102",
                DateTime = baseTime.AddSeconds(1)
            },
            new()
            {
                TransactionId = 3,
                Username = "999bb20a00003",
                FirstName = "Olivia",
                LastName = "Garcia",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.ProviderRejected),
                IsDeposit = true,
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoDeposit),
                ReferenceNumber = "QW352163991323103",
                DateTime = baseTime.AddSeconds(2)
            },
            new()
            {
                TransactionId = 4,
                Username = "999bb20a00004",
                FirstName = "Liam",
                LastName = "Rodriguez",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.Completed),
                IsDeposit = false, // Changed to withdrawal for variety
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoWithdrawal),
                ReferenceNumber = "QW352163991323104",
                DateTime = baseTime.AddSeconds(3)
            },
            new()
            {
                TransactionId = 5,
                Username = "999bb20a00005",
                FirstName = "Emma",
                LastName = "Martinez",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.Rejected),
                IsDeposit = true,
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoDeposit),
                ReferenceNumber = "QW352163991323105",
                DateTime = baseTime.AddSeconds(4)
            },
            new()
            {
                TransactionId = 6,
                Username = "999bb20a00006",
                FirstName = "Noah",
                LastName = "Hernandez",
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(TransactionStatus.Expired),
                IsDeposit = false, // Changed to withdrawal for variety
                TransactionType = EnumHelper.GetDescription(TransactionType.AutoWithdrawal),
                ReferenceNumber = "QW352163991323106",
                DateTime = baseTime.AddSeconds(5)
            }
        };

        var firstNames = new List<string> { "Ava", "Isabella", "Mia", "Aiden", "Lucas", "Ethan", "Mason", "Logan", "Oliver", "Elijah", "James", "Benjamin", "William", "Michael", "Alexander", "Daniel", "Matthew", "David", "Joseph", "Andrew" };
        var lastNames = new List<string> { "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Young", "Allen", "King", "Wright", "Scott", "Green", "Baker", "Adams", "Nelson", "Carter" };
        var statuses = new List<TransactionStatus> { TransactionStatus.Completed, TransactionStatus.Rejected, TransactionStatus.ProviderRejected, TransactionStatus.Expired };
        var random = new Random();

        for (int i = 7; i <= 26; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Count)];
            var lastName = lastNames[random.Next(lastNames.Count)];
            var status = statuses[random.Next(statuses.Count)];
            var isDeposit = random.Next(2) == 0; // Randomly choose deposit or withdrawal
            var transactionTypeEnum = isDeposit ? TransactionType.AutoDeposit : TransactionType.AutoWithdrawal;

            transactions.Add(new TransactionModel
            {
                TransactionId = i,
                Username = $"999bb20a{i:D5}", // Ensure 5 digits
                FirstName = firstName,
                LastName = lastName,
                Amount = 20.0M,
                Currency = "TB",
                From = "143399301B",
                To = "12345678",
                Status = EnumHelper.GetDescription(status),
                IsDeposit = isDeposit,
                TransactionType = EnumHelper.GetDescription(transactionTypeEnum),
                ReferenceNumber = $"QW352163991323{i + 100}",
                DateTime = baseTime.AddSeconds(i - 1)
            });
        }

        return transactions;
    }
}