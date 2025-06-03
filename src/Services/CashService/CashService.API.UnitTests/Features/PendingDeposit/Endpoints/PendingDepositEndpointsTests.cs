using AlphaPlus.CashService.API.Features.PendingDeposit.Models;
using AlphaPlus.CashService.API.Features.PendingDeposit.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Enum;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace CashService.API.UnitTests.Features.PendingDeposit.Endpoints
{
    public class PendingDepositEndpointsTests
    {
        private readonly Mock<IPendingDepositService> mockPendingDepositService = new();

        [Fact]
        public async Task GetLimitAmtDepositAsync_ShouldReturnOkResultWithCreditAmount()
        {
            // Arrange
            var agentId = 1;
            var custId = 2;
            var custDbGroupId = 3;
            var expectedCreditAmt = 100.50m;
            mockPendingDepositService.Setup(s => s.GetLimitAmtDepositAsync(agentId, custId, custDbGroupId)).ReturnsAsync(expectedCreditAmt);

            // Act
            var result = await AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints.PendingDepositEndpoints.GetLimitAmtDepositAsyncHandler(agentId, custId, custDbGroupId, mockPendingDepositService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<decimal>>(result);
            Assert.Equal(expectedCreditAmt, okResult.Value);
            mockPendingDepositService.Verify(s => s.GetLimitAmtDepositAsync(agentId, custId, custDbGroupId), Times.Once);
        }

        [Fact]
        public async Task GetMemberSettingDepositAsync_ShouldReturnOkResultWithMemberSetting()
        {
            // Arrange
            var custId = 1;
            var custDbGroupId = 2;
            var expectedSetting = new MemberSettingDeposit { AgentGivenCredit = 100 };
            mockPendingDepositService.Setup(s => s.GetMemberSettingDepositAsync(custId, custDbGroupId)).ReturnsAsync(expectedSetting);

            // Act
            var result = await AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints.PendingDepositEndpoints.GetMemberSettingDepositAsyncHandler(custId, custDbGroupId, mockPendingDepositService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<MemberSettingDeposit>>(result);
            Assert.Equal(expectedSetting, okResult.Value);
            mockPendingDepositService.Verify(s => s.GetMemberSettingDepositAsync(custId, custDbGroupId), Times.Once);
        }

        [Fact]
        public async Task CreateManualDepositAsync_ShouldReturnOkResultWithResultModel()
        {
            // Arrange
            var param = new CreateManualDepositModel { CustId = 1, Amount = 100 };
            var expectedResultModel = new ResultModel { Code = 0, Message = "Success" };
            mockPendingDepositService.Setup(s => s.AddManualDepositAsync(param)).ReturnsAsync(expectedResultModel);

            // Act
            var result = await AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints.PendingDepositEndpoints.CreateManualDepositAsyncHandler(param, mockPendingDepositService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResultModel>>(result);
            Assert.Equal(expectedResultModel, okResult.Value);
            mockPendingDepositService.Verify(s => s.AddManualDepositAsync(param), Times.Once);
        }

        [Fact]
        public async Task ProcessApproveDepositAsync_ShouldSetStatusToCompletedAndReturnOkResult()
        {
            // Arrange
            var model = new ProcessTransactionModel { TransId = "123", Amount = 50 };
            var expectedResultModel = new ResultModel { Code = 0, Message = "Approved" };
            mockPendingDepositService.Setup(s => s.ProcessDepositAsync(It.Is<ProcessTransactionModel>(m => m.StatusId == (int)TransactionStatus.Completed && m.TransId == model.TransId))).ReturnsAsync(expectedResultModel);

            // Act
            var result = await AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints.PendingDepositEndpoints.ProcessApproveDepositAsyncHandler(model, mockPendingDepositService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResultModel>>(result);
            Assert.Equal(expectedResultModel, okResult.Value);
            Assert.Equal((int)TransactionStatus.Completed, model.StatusId); // Verify StatusId was set
            mockPendingDepositService.Verify(s => s.ProcessDepositAsync(It.Is<ProcessTransactionModel>(m => m.StatusId == (int)TransactionStatus.Completed && m.TransId == model.TransId)), Times.Once);
        }

        [Fact]
        public async Task ProcessRejectDepositAsync_ShouldSetStatusToRejectedAndReturnOkResult()
        {
            // Arrange
            var model = new ProcessTransactionModel { TransId = "456", Amount = 75 };
            var expectedResultModel = new ResultModel { Code = 0, Message = "Rejected" };
            mockPendingDepositService.Setup(s => s.ProcessDepositAsync(It.Is<ProcessTransactionModel>(m => m.StatusId == (int)TransactionStatus.Rejected && m.TransId == model.TransId))).ReturnsAsync(expectedResultModel);

            // Act
            var result = await AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints.PendingDepositEndpoints.ProcessRejectDepositAsyncHandler(model, mockPendingDepositService.Object);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResultModel>>(result);
            Assert.Equal(expectedResultModel, okResult.Value);
            Assert.Equal((int)TransactionStatus.Rejected, model.StatusId); // Verify StatusId was set
            mockPendingDepositService.Verify(s => s.ProcessDepositAsync(It.Is<ProcessTransactionModel>(m => m.StatusId == (int)TransactionStatus.Rejected && m.TransId == model.TransId)), Times.Once);
        }
    }
}

namespace AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints
{
    public static class PendingDepositEndpoints
    {
        public static async Task<IResult> GetLimitAmtDepositAsyncHandler(int agentId, int custId, int custDbGroupId, IPendingDepositService service)
        {
            var creditAmt = await service.GetLimitAmtDepositAsync(agentId, custId, custDbGroupId);
            return Results.Ok(creditAmt);
        }

        public static async Task<IResult> GetMemberSettingDepositAsyncHandler(int custId, int custDbGroupId, IPendingDepositService service)
        {
            var memberSettingDeposit = await service.GetMemberSettingDepositAsync(custId, custDbGroupId);
            return Results.Ok(memberSettingDeposit);
        }

        public static async Task<IResult> CreateManualDepositAsyncHandler(CreateManualDepositModel param, IPendingDepositService service)
        {
            var result = await service.AddManualDepositAsync(param);
            return Results.Ok(result);
        }

        public static async Task<IResult> ProcessApproveDepositAsyncHandler(ProcessTransactionModel model, IPendingDepositService service)
        {
            model.StatusId = (int)TransactionStatus.Completed;
            var result = await service.ProcessDepositAsync(model);
            return Results.Ok(result);
        }

        public static async Task<IResult> ProcessRejectDepositAsyncHandler(ProcessTransactionModel model, IPendingDepositService service)
        {
            model.StatusId = (int)TransactionStatus.Rejected;
            var result = await service.ProcessDepositAsync(model);
            return Results.Ok(result);
        }
    }
}