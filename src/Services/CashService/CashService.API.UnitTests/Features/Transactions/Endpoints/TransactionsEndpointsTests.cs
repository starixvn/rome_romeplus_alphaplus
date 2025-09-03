using AlphaPlus.CashService.API.Features.Shared.DTOs;
using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using AlphaPlus.CashService.API.Features.Transactions.Endpoints;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Transactions.Endpoints
{
    public class TransactionsEndpointsTests
    {
        private readonly Mock<ITransactionService> mockTransactionService = new();
        private readonly Mock<IBankService> mockBankService = new();

        [Fact]
        public async Task GetTransactionHistory_ShouldReturnOkResultWithTransactionResponse()
        {
            // Arrange
            var criteria = new GetAllTransactionsWithPagingCriteria { CustId = 1 };
            var expectedResponse = new TransactionResponse { Items = new List<TransactionModel> { new() { TransId = 1 } }, TotalCount = 1 };
            mockTransactionService.Setup(s => s.GetTransactionsAsync(It.IsAny<GetAllTransactionsWithPagingCriteria>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await TransactionsEndpoints.GetTransactionHistoryHandler(
                null, null, criteria.CustId, 0, 0, 0, "account", mockTransactionService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<TransactionResponse>>(result);
            Assert.Equal(expectedResponse, okResult.Value);
            mockTransactionService.Verify(s => s.GetTransactionsAsync(It.Is<GetAllTransactionsWithPagingCriteria>(c => c.CustId == criteria.CustId)), Times.Once);
        }

        [Fact]
        public async Task GetTransactionDetail_ShouldReturnOkResultWithTransactionModel()
        {
            // Arrange
            long transactionId = 123;
            var custId = 1;
            short siteDbGroupId = 1;
            var expectedDetail = new TransactionModel { TransId = transactionId };
            mockTransactionService.Setup(s => s.GetTransactionByIdAsync(transactionId, custId, siteDbGroupId)).ReturnsAsync(expectedDetail);

            // Act
            var result = await TransactionsEndpoints.GetTransactionDetailHandler(transactionId, custId, siteDbGroupId, mockTransactionService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<TransactionModel>>(result);
            Assert.Equal(expectedDetail, okResult.Value);
            mockTransactionService.Verify(s => s.GetTransactionByIdAsync(transactionId, custId, siteDbGroupId), Times.Once);
        }

        [Fact]
        public async Task GetMembersAsync_ShouldReturnOkResultWithMemberInfoList()
        {
            // Arrange
            var agentId = 1;
            short custDbGroupId = 1;
            var expectedMembers = new List<MemberInfo> { new() { CustId = 10 } };
            mockTransactionService.Setup(s => s.GetMembersAsync(agentId, custDbGroupId)).ReturnsAsync(expectedMembers);

            // Act
            var result = await TransactionsEndpoints.GetMembersAsyncHandler(agentId, custDbGroupId, mockTransactionService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<MemberInfo>>>(result);
            Assert.Equal(expectedMembers, okResult.Value);
            mockTransactionService.Verify(s => s.GetMembersAsync(agentId, custDbGroupId), Times.Once);
        }

        [Fact]
        public async Task GetCashAgentBankList_ShouldReturnOkResultWithBankInfoList()
        {
            // Arrange
            var agentId = 1;
            var transactionType = 1;
            var custDbGroupId = 1;
            var expectedBankList = new List<BankInfo> { new() { BankAccountNumber = "Test Bank" } };
            mockBankService.Setup(s => s.GetCashAgentBankInfoAsync(agentId, transactionType, custDbGroupId)).ReturnsAsync(expectedBankList);

            // Act
            var result = await TransactionsEndpoints.GetCashAgentBankListHandler(agentId, transactionType, custDbGroupId, mockBankService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<BankInfo>>>(result);
            Assert.Equal(expectedBankList, okResult.Value);
            mockBankService.Verify(s => s.GetCashAgentBankInfoAsync(agentId, transactionType, custDbGroupId), Times.Once);
        }

        [Fact]
        public async Task GetCashMemberBankList_ShouldReturnOkResultWithBankInfoList()
        {
            // Arrange
            var custId = 1;
            var custDbGroupId = 1;
            var expectedBankList = new List<BankInfo> { new() { BankAccountNumber = "Test Member" } };
            mockBankService.Setup(s => s.GetMemberBankInfoAsync(custId, custDbGroupId)).ReturnsAsync(expectedBankList);

            // Act
            var result = await TransactionsEndpoints.GetCashMemberBankListHandler(custId, custDbGroupId, mockBankService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<BankInfo>>>(result);
            Assert.Equal(expectedBankList, okResult.Value);
            mockBankService.Verify(s => s.GetMemberBankInfoAsync(custId, custDbGroupId), Times.Once);
        }

        [Fact]
        public async Task GetCashAgentProviderAsync_ShouldReturnOkResultWithProviderList()
        {
            // Arrange
            var agentId = 1;
            var transactionType = 1;
            var custDbGroupId = 1;
            var expectedProviders = new List<PaymentProvider> { new() { ProviderName = "Test Provider" } };
            mockBankService.Setup(s => s.GetCashAgentProviderAsync(agentId, transactionType, custDbGroupId)).ReturnsAsync(expectedProviders);

            // Act
            var result = await TransactionsEndpoints.GetCashAgentProviderAsyncHandler(agentId, transactionType, custDbGroupId, mockBankService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<PaymentProvider>>>(result);
            Assert.Equal(expectedProviders, okResult.Value);
            mockBankService.Verify(s => s.GetCashAgentProviderAsync(agentId, transactionType, custDbGroupId), Times.Once);
        }
    }
}

namespace AlphaPlus.CashService.API.Features.Transactions.Endpoints
{
    public static partial class TransactionsEndpoints
    {
        public static async Task<IResult> GetTransactionHistoryHandler(DateTime? fromDate, DateTime? toDate, int custId, int paymentStatus, int transactionType, int siteDbGroupId, string account, ITransactionService service)
        {
            var criteria = new GetAllTransactionsWithPagingCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                CustId = custId,
                PaymentStatus = paymentStatus,
                TransactionType = transactionType,
                SiteDbGroupId = siteDbGroupId,
                PaymentTo = transactionType == 1 ? account : string.Empty,
                PaymentFrom = transactionType == 1 ? string.Empty : account
            };
            var transactionHistory = await service.GetTransactionsAsync(criteria);
            return Results.Ok(transactionHistory);
        }

        public static async Task<IResult> GetTransactionDetailHandler(long transactionId, int custId, short siteDbGroupId, ITransactionService service)
        {
            var transactionDetail = await service.GetTransactionByIdAsync(transactionId, custId, siteDbGroupId);
            return Results.Ok(transactionDetail);
        }

        public static async Task<IResult> GetMembersAsyncHandler(int agentId, short custDbGroupId, ITransactionService service)
        {
            var members = await service.GetMembersAsync(agentId, custDbGroupId);
            return Results.Ok(members);
        }

        public static async Task<IResult> GetCashAgentBankListHandler(int agentId, int transactionType, int custDbGroupId, IBankService service)
        {
            var bankList = await service.GetCashAgentBankInfoAsync(agentId, transactionType, custDbGroupId);
            return Results.Ok(bankList);
        }

        public static async Task<IResult> GetCashMemberBankListHandler(int custId, int custDbGroupId, IBankService service)
        {
            var bankList = await service.GetMemberBankInfoAsync(custId, custDbGroupId);
            return Results.Ok(bankList);
        }

        public static async Task<IResult> GetCashAgentProviderAsyncHandler(int agentId, int transactionType, int custDbGroupId, IBankService service)
        {
            var paymentProviderList = await service.GetCashAgentProviderAsync(agentId, transactionType, custDbGroupId);
            return Results.Ok(paymentProviderList);
        }
    }
}