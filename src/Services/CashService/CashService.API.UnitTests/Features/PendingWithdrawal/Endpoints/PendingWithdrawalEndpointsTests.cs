namespace AlphaPlus.CashService.API.UnitTests.Features.PendingWithdrawal.Endpoints;

using AlphaPlus.CashService.API.Features.PendingWithdrawal.Models;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Enum;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

public class PendingWithdrawalEndpointsTests
{
    private readonly Mock<IPendingWithdrawalService> mockService = new();

    [Fact]
    public async Task GetLimitAmtWithdraw_Logic_ShouldReturnCorrectResult()
    {
        // Arrange
        var custId = 1;
        var uplineId = 1;
        var providerId = 1;
        var paymentMethodId = 1;
        var custDbGroupId = 1;

        var expectedResult = new CreditAmount
        {
            MaxAmt = 1000m,
            MinAmt = 100m
        };

        mockService.Setup(x => x.GetLimitAmtWithdrawAsync(It.Is<GetCashLimitAmt>(g =>
            g.CustId == custId &&
            g.UplineId == uplineId &&
            g.ProviderId == providerId &&
            g.PaymentMethodId == paymentMethodId &&
            g.CustDbGroupId == custDbGroupId)))
            .ReturnsAsync(expectedResult);

        // Act - Simulate the endpoint logic
        var cashLimit = await mockService.Object.GetLimitAmtWithdrawAsync(new GetCashLimitAmt
        {
            CustId = custId,
            UplineId = uplineId,
            ProviderId = providerId,
            CustDbGroupId = custDbGroupId,
            PaymentMethodId = paymentMethodId
        });
        var result = Results.Ok(cashLimit);

        // Assert
        var okResult = Assert.IsType<Ok<CreditAmount>>(result);
        Assert.Equal(expectedResult.MaxAmt, okResult.Value.MaxAmt);
        Assert.Equal(expectedResult.MinAmt, okResult.Value.MinAmt);
        mockService.Verify(x => x.GetLimitAmtWithdrawAsync(It.IsAny<GetCashLimitAmt>()), Times.Once);
    }

    [Fact]
    public async Task GetMemberCashBalance_Logic_ShouldReturnCorrectResult()
    {
        // Arrange
        var custId = 1;
        var siteDbGroupId = 1;
        var expectedBalance = 1000m;

        mockService.Setup(x => x.GetCashBalanceAsync(custId, siteDbGroupId))
            .ReturnsAsync(expectedBalance);

        // Act - Simulate the endpoint logic
        var cashBalance = await mockService.Object.GetCashBalanceAsync(custId, siteDbGroupId);
        var result = Results.Ok(cashBalance);

        // Assert
        var okResult = Assert.IsType<Ok<decimal>>(result);
        Assert.Equal(expectedBalance, okResult.Value);
        mockService.Verify(x => x.GetCashBalanceAsync(custId, siteDbGroupId), Times.Once);
    }

    [Fact]
    public async Task GetMemberSettingWithdraw_Logic_ShouldReturnCorrectResult()
    {
        // Arrange
        var custId = 1;
        var expectedSettings = new MemberSettingWithdraw
        {
            WithdrawalLimitCnt = 5,
            MinLimitAmt = 100m,
            MaxLimitAmt = 1000m
        };

        mockService.Setup(x => x.GetMemberSettingWithdrawAsync(custId))
            .ReturnsAsync(expectedSettings);

        // Act - Simulate the endpoint logic
        var memberSettingWithdraw = await mockService.Object.GetMemberSettingWithdrawAsync(custId);
        var result = Results.Ok(memberSettingWithdraw);

        // Assert
        var okResult = Assert.IsType<Ok<MemberSettingWithdraw>>(result);
        Assert.Equal(expectedSettings.WithdrawalLimitCnt, okResult.Value.WithdrawalLimitCnt);
        Assert.Equal(expectedSettings.MinLimitAmt, okResult.Value.MinLimitAmt);
        Assert.Equal(expectedSettings.MaxLimitAmt, okResult.Value.MaxLimitAmt);
        mockService.Verify(x => x.GetMemberSettingWithdrawAsync(custId), Times.Once);
    }

    [Fact]
    public async Task RevertWithdrawalTransactionToPending_ShouldReturnOkResultWithResultModel()
    {
        // Arrange
        var model = new RevertTransaction { TransId = 1 };
        var expectedResultModel = new ResultModel { Code = 0, Message = "Success" };
        mockService.Setup(s => s.RevertWithdrawalTransactionToPending(model)).ReturnsAsync(expectedResultModel);

        // Act
        var commandResult = await mockService.Object.RevertWithdrawalTransactionToPending(model);
        var result = Results.Ok(commandResult);

        // Assert
        var okResult = Assert.IsType<Ok<ResultModel>>(result);
        Assert.Equal(expectedResultModel, okResult.Value);
        mockService.Verify(s => s.RevertWithdrawalTransactionToPending(model), Times.Once);
    }

    [Fact]
    public async Task CreateManualWithdraw_Logic_ShouldReturnCorrectResult()
    {
        // Arrange
        var model = new AddManualWithdrawalModel
        {
            CustId = 1,
            ProviderId = 1,
            UplineId = 1,
            SubAccId = 1,
            SiteDbGroupId = 1,
            CustDbGroupId = 1,
            PaymentFrom = "Test",
            PaymentTo = "Test",
            Amount = 500m,
            PaySlip = [1, 2, 3],
            Ip = "127.0.0.1",
            Remark = "Test",
            PaymentMethodId = 1
        };

        var expectedResult = new ResultModel(0, "Success");

        mockService.Setup(x => x.AddManualWithdrawAsync(model))
            .ReturnsAsync(expectedResult);

        // Act - Simulate the endpoint logic
        var serviceResult = await mockService.Object.AddManualWithdrawAsync(model);
        var result = Results.Ok(serviceResult);

        // Assert
        var okResult = Assert.IsType<Ok<ResultModel>>(result);
        Assert.Equal(expectedResult.Code, okResult.Value?.Code);
        Assert.Equal(expectedResult.Message, okResult.Value?.Message);
        mockService.Verify(x => x.AddManualWithdrawAsync(model), Times.Once);
    }

    [Fact]
    public async Task ApproveTransaction_Logic_ShouldSetStatusAndReturnCorrectResult()
    {
        // Arrange
        var model = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            PaymentFrom = "Test",
            Remark = "Test",
            Payslip = [1, 2]
        };

        var expectedResult = new ResultModel(0, "Success");

        mockService.Setup(x => x.ProcessWithdrawAsync(It.Is<ProcessWithdrawalTransaction>(m =>
            m.StatusId == (int)TransactionStatus.Completed)))
            .ReturnsAsync(expectedResult);

        // Act - Simulate the endpoint logic
        model.StatusId = (int)TransactionStatus.Completed;
        var serviceResult = await mockService.Object.ProcessWithdrawAsync(model);
        var result = Results.Ok(serviceResult);

        // Assert
        Assert.Equal((int)TransactionStatus.Completed, model.StatusId);
        var okResult = Assert.IsType<Ok<ResultModel>>(result);
        Assert.Equal(expectedResult.Code, okResult.Value?.Code);
        Assert.Equal(expectedResult.Message, okResult.Value?.Message);
        mockService.Verify(x => x.ProcessWithdrawAsync(It.Is<ProcessWithdrawalTransaction>(m =>
            m.StatusId == (int)TransactionStatus.Completed)), Times.Once);
    }

    [Fact]
    public async Task RejectTransaction_Logic_ShouldSetStatusAndReturnCorrectResult()
    {
        // Arrange
        var model = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            PaymentFrom = "Test",
            Remark = "Test",
            Payslip = [1, 2]
        };

        var expectedResult = new ResultModel(0, "Success");

        mockService.Setup(x => x.ProcessWithdrawAsync(It.Is<ProcessWithdrawalTransaction>(m =>
            m.StatusId == (int)TransactionStatus.Rejected)))
            .ReturnsAsync(expectedResult);

        // Act - Simulate the endpoint logic
        model.StatusId = (int)TransactionStatus.Rejected;
        var serviceResult = await mockService.Object.ProcessWithdrawAsync(model);
        var result = Results.Ok(serviceResult);

        // Assert
        Assert.Equal((int)TransactionStatus.Rejected, model.StatusId);
        var okResult = Assert.IsType<Ok<ResultModel>>(result);
        Assert.Equal(expectedResult.Code, okResult.Value?.Code);
        Assert.Equal(expectedResult.Message, okResult.Value?.Message);
        mockService.Verify(x => x.ProcessWithdrawAsync(It.Is<ProcessWithdrawalTransaction>(m =>
            m.StatusId == (int)TransactionStatus.Rejected)), Times.Once);
    }

    public static async Task<IResult> RevertWithdrawalTransactionToPendingHandler(IPendingWithdrawalService service, RevertTransaction command)
    {
        var result = await service.RevertWithdrawalTransactionToPending(command);
        return Results.Ok(result);
    }
}