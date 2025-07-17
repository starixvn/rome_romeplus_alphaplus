namespace AlphaPlus.CashService.API.UnitTests.Features.PendingWithdrawal.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Models;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

public class PendingWithdrawalServiceTests
{
    private readonly Mock<IDynamicRepository> mockDynamicRepository;
    private readonly Mock<ITransactionService> mockTransactionService;
    private readonly Mock<IPaymentGatewayService> mockPaymentGatewayService;
    private readonly PendingWithdrawalService service;

    public PendingWithdrawalServiceTests()
    {
        mockDynamicRepository = new Mock<IDynamicRepository>();
        mockTransactionService = new Mock<ITransactionService>();
        mockPaymentGatewayService = new Mock<IPaymentGatewayService>();
        service = new PendingWithdrawalService(mockDynamicRepository.Object, mockTransactionService.Object, mockPaymentGatewayService.Object);
    }

    [Fact]
    public async Task GetCashBalanceAsync_ShouldReturnCorrectBalance()
    {
        // Arrange
        var custId = 1;
        var siteDbGroupId = 1;
        var expectedBalance = 1000m;

        mockDynamicRepository
            .Setup(repo => repo.ExecuteAsync(It.IsAny<GetCashBalanceCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((INonQueryCommand criteria, string connectionString, CancellationToken token) =>
            {
                if (criteria is GetCashBalanceCriteria cbCriteria)
                {
                    cbCriteria.TotalCash = expectedBalance;
                }
            })
            .Returns(Task.FromResult(0));

        // Act
        var result = await service.GetCashBalanceAsync(custId, siteDbGroupId);

        // Assert
        Assert.Equal(expectedBalance, result);
        mockDynamicRepository.Verify(repo => repo.ExecuteAsync(It.IsAny<GetCashBalanceCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLimitAmtWithdrawAsync_ShouldReturnCorrectLimits()
    {
        // Arrange
        var param = new GetCashLimitAmt
        {
            CustId = 1,
            ProviderId = 1,
            UplineId = 1,
            CustDbGroupId = 1,
            PaymentMethodId = 1
        };

        var expectedMaxAmt = 1000m;
        var expectedMinAmt = 100m;

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string connectionString, CancellationToken token) =>
            {
                if (criteria is GetLimitAmtWithdrawCriteria laCriteria)
                {
                    laCriteria.MaxAmt = expectedMaxAmt;
                    laCriteria.MinAmt = expectedMinAmt;
                }
            })
            .ReturnsAsync(new CreditAmount { MaxAmt = expectedMaxAmt, MinAmt = expectedMinAmt });

        // Act
        var result = await service.GetLimitAmtWithdrawAsync(param);

        // Assert
        Assert.Equal(expectedMaxAmt, result.MaxAmt);
        Assert.Equal(expectedMinAmt, result.MinAmt);
        mockDynamicRepository.Verify(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMemberSettingWithdrawAsync_ShouldReturnCorrectSettings()
    {
        // Arrange
        var custId = 1;
        var expectedSettings = new MemberSettingWithdraw
        {
            WithdrawalLimitCnt = 5,
            MinLimitAmt = 100m,
            MaxLimitAmt = 1000m
        };

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<MemberSettingWithdraw>(It.IsAny<GetMemberSettingWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedSettings);

        // Act
        var result = await service.GetMemberSettingWithdrawAsync(custId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSettings.WithdrawalLimitCnt, result.WithdrawalLimitCnt);
        Assert.Equal(expectedSettings.MinLimitAmt, result.MinLimitAmt);
        Assert.Equal(expectedSettings.MaxLimitAmt, result.MaxLimitAmt);
        mockDynamicRepository.Verify(repo => repo.GetAsync<MemberSettingWithdraw>(It.IsAny<GetMemberSettingWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddManualWithdrawAsync_WithProviderId_ShouldProcessWithLimits()
    {
        // Arrange
        var param = new AddManualWithdrawalModel
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

        var expectedMaxAmt = 1000m;
        var expectedMinAmt = 100m;

        mockDynamicRepository
            .Setup(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string connectionString, CancellationToken token) =>
            {
                if (criteria is GetLimitAmtWithdrawCriteria laCriteria)
                {
                    laCriteria.MaxAmt = expectedMaxAmt;
                    laCriteria.MinAmt = expectedMinAmt;
                }
            })
            .ReturnsAsync(new CreditAmount { MaxAmt = expectedMaxAmt, MinAmt = expectedMinAmt });

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualWithdrawCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string connectionString, CancellationToken token) =>
            {
                if (criteria is CreateManualWithdrawCommand command)
                {
                    command.ErrCode = 0;
                    command.ErrMsg = "Success";
                    command.CashAccumCreditTbl = "TestTable";
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.AddManualWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        mockDynamicRepository.Verify(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualWithdrawCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
    }

    [Fact]
    public async Task AddManualWithdrawAsync_WithoutProviderId_ShouldProcessWithoutLimits()
    {
        // Arrange
        var param = new AddManualWithdrawalModel
        {
            CustId = 1,
            ProviderId = 0, // No provider
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

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualWithdrawCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria criteria, string connectionString, CancellationToken token) =>
            {
                if (criteria is CreateManualWithdrawCommand command)
                {
                    command.ErrCode = 0;
                    command.ErrMsg = "Success";
                    command.CashAccumCreditTbl = "TestTable";
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.AddManualWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        mockDynamicRepository.Verify(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualWithdrawCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
    }

    [Fact]
    public async Task ProcessWithdrawAsync_ShouldProcessTransaction()
    {
        // Arrange
        var param = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            PaymentFrom = "Test",
            Remark = "Test",
            StatusId = 1,
            Payslip = [1, 2]
        };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessWithdrawalTransactionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria command, string _, CancellationToken token) =>
            {
                if (command is ProcessWithdrawalTransactionCommand cmd)
                {
                    cmd.ErrCode = 0;
                    cmd.ErrMsg = "Success";
                }
            });

        // Act
        var result = await service.ProcessWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessWithdrawalTransactionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}