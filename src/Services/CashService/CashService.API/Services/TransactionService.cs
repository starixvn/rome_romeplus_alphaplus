namespace AlphaPlus.CashService.API.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Criteria.Transactions;
using Enum;
using Extensions;
using Fanex.Data.Repository;
using Models.Transactions;

public interface ITransactionService
{
    Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria);

    Task<TransactionModel> GetTransactionByIdAsync(int transactionId);

    Task<ResultModel> UpdateTransactionStatus(UpdateTransactionStatusRemarkCommand command);

    Task<IEnumerable<MemberInfo>> GetMembersAsync(int agentId, int custDbGroupId);

    Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId);
}

public class TransactionService(IDynamicRepository dynamicRepository)
    : ITransactionService
{
    public async Task<TransactionResponse> GetTransactionsAsync(GetAllTransactionsWithPagingCriteria criteria)
    {
        var allMockTransactions = GetMockTransactions();

        var filteredTransactions = allMockTransactions
            .Where(t => criteria.Status == "All" || t.Status == criteria.Status)
            .Where(t => (criteria.TransactionType == 1 && t.IsDeposit) ||
                       (criteria.TransactionType == 2 && !t.IsDeposit))
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

    public async Task<ResultModel> UpdateTransactionStatus(UpdateTransactionStatusRemarkCommand command)
    {
        var random = new Random();
        var randomValue = random.Next(2);
        var errMsg = string.Empty;
        if (randomValue != 0)
        {
            return new ResultModel(1, "Transaction status updated successfully.");
        }
        return await Task.FromResult(new ResultModel(randomValue, errMsg));
    }

    private static List<TransactionModel> GetMockTransactions()
    {
        var random = new Random();
        var baseTime = DateTime.Now.AddDays(-30);
        var transactions = new List<TransactionModel>();

        var firstNames = new[] { "Sophia", "Jackson", "Olivia", "Liam", "Emma", "Noah", "Ava", "Isabella", "Mia", "Aiden" };
        var lastNames = new[] { "Miller", "Davis", "Garcia", "Rodriguez", "Martinez", "Hernandez", "Wilson", "Moore", "Taylor", "Anderson" };
        var statuses = new[] { TransactionStatus.Completed, TransactionStatus.Rejected, TransactionStatus.ProviderRejected, TransactionStatus.Expired };
        var currencies = new[] { "TB", "USD", "EUR", "GBP" };
        var amounts = new[] { 20.0M, 50.0M, 100.0M, 200.0M, 500.0M, 1000.0M };
        var betCredits = new[] { 0.0M, 10.0M, 20.0M, 50.0M, 100.0M, 200.0M, 500.0M };
        var fromAccounts = new[] { "143399301B", "143399302B", "143399303B", "143399304B" };
        var toAccounts = new[] { "12345678", "87654321", "11223344", "44332211" };

        for (var i = 1; i <= 1000; i++)
        {
            var isDeposit = random.Next(2) == 0;
            var transactionType = isDeposit ? TransactionType.AutoDeposit : TransactionType.AutoWithdrawal;
            var status = statuses[random.Next(statuses.Length)];
            var amount = amounts[random.Next(amounts.Length)];
            var betCredit = betCredits[random.Next(betCredits.Length)];
            var currency = currencies[random.Next(currencies.Length)];
            var from = fromAccounts[random.Next(fromAccounts.Length)];
            var to = toAccounts[random.Next(toAccounts.Length)];
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var username = $"999bb20a{i:D5}";
            var referenceNumber = $"QW{random.Next(100000, 999999)}{random.Next(100000, 999999)}";
            var createdDate = baseTime.AddMinutes(random.Next(43200)); // Random time within last 30 days
            var lastUpdatedTime = createdDate.AddMinutes(random.Next(1, 60)); // Random time after created date

            transactions.Add(new TransactionModel
            {
                TransactionId = i,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Amount = amount,
                BetCredit = betCredit,
                Currency = currency,
                From = from,
                To = to,
                Status = EnumHelper.GetDescription(status),
                IsDeposit = isDeposit,
                TransactionType = EnumHelper.GetDescription(transactionType),
                ReferenceNumber = referenceNumber,
                CreatedDate = createdDate,
                LastUpdatedDate = lastUpdatedTime,
                Remark = $"Transaction remarks for {referenceNumber}"
            });
        }

        return transactions;
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

    public async Task<ResultModel> SyncCashAccumCreditAsync(string cashAccumCreditTbl, int custDbGroupId)
    {
        var command = new SyncCashAccumCreditCommand
        {
            CashAccumCreditTbl = cashAccumCreditTbl,
            CustDbGroupId = custDbGroupId
        };
        await dynamicRepository.ExecuteAsync(command);
        return new ResultModel(command.ErrCode, command.ErrMsg);
    }
}