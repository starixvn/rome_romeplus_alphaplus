namespace AlphaPlus.CashService.API.UnitTests.Features.PendingWithdrawal.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Models;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests.PaymentGateway;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

[Trait("Category", "CashService")]
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
                    command.TransId = 123;
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Mock ApproveByPaymentGatewayAsync to return success
        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 500m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 0,
            Message = "Success"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<short>()))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), It.IsAny<short>()))
            .ReturnsAsync(paymentGatewayResponse);

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.AddManualWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        mockDynamicRepository.Verify(repo => repo.GetAsync<CreditAmount>(It.IsAny<GetLimitAmtWithdrawCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<CreateManualWithdrawCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<short>()), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), It.IsAny<short>()), Times.Once);
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
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
    }

    [Fact]
    public async Task AddManualWithdrawAsync_WithProviderIdAndError_ShouldNotCallPaymentGateway()
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
                    command.ErrCode = 1; // Error
                    command.ErrMsg = "Error occurred";
                    command.CashAccumCreditTbl = "TestTable";
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.AddManualWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Code);
        Assert.Equal("Error occurred", result.Message);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
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
                    cmd.CashAccumCreditTbl = "TestTable";
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.ProcessWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessWithdrawalTransactionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
    }

    [Fact]
    public async Task ProcessWithdrawAsync_WithRemovePayslip_ShouldProcessWithoutPayslip()
    {
        // Arrange
        var param = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            PaymentFrom = "Test",
            Remark = "Test",
            StatusId = 1,
            Payslip = [1, 2],
            IsRemovePayslip = true
        };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessWithdrawalTransactionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback((ICriteria command, string _, CancellationToken token) =>
            {
                if (command is ProcessWithdrawalTransactionCommand cmd)
                {
                    cmd.ErrCode = 0;
                    cmd.ErrMsg = "Success";
                    cmd.CashAccumCreditTbl = "TestTable";
                }
            })
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.ProcessWithdrawAsync(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<ProcessWithdrawalTransactionCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Once);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithValidTransaction_ShouldProcessSuccessfully()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 1000m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 0,
            Message = "Success"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId))
            .ReturnsAsync(paymentGatewayResponse);

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        Assert.Equal(string.Empty, result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId), Times.Once);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithNullTransaction_ShouldReturnError()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync((TransactionModel)null);

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(405, result.Code);
        Assert.Equal("Invalid parameters", result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), It.IsAny<short>()), Times.Never);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithInvalidPaymentTo_ShouldReturnError()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "InvalidFormat", // Invalid format
            Amount = 1000m,
            Username = "TestUser"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(405, result.Code);
        Assert.Equal("Invalid parameters", result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), It.IsAny<short>()), Times.Never);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithEmptyPaymentTo_ShouldReturnError()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1
        };

        var transaction = new TransactionModel
        {
            PaymentTo = null, // Null payment to
            Amount = 1000m,
            Username = "TestUser"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(405, result.Code);
        Assert.Equal("Invalid parameters", result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), It.IsAny<short>()), Times.Never);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithFailedPaymentGateway_ShouldUpdateStatusAndReturnError()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1,
            IP = "127.0.0.1"
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 1000m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 4, // Provider rejected
            Message = "Provider rejected"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId))
            .ReturnsAsync(paymentGatewayResponse);

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(410, result.Code); // Provider rejected code
        Assert.Equal("Provider rejected", result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId), Times.Once);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithOtherFailedPaymentGateway_ShouldUpdateStatusAndReturnError()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1,
            IP = "127.0.0.1"
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 1000m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 5, // Other error
            Message = "Other error"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId))
            .ReturnsAsync(paymentGatewayResponse);

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WriteLogDBCommandModel>());

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(411, result.Code); // Other error code
        Assert.Equal("Other error", result.Message);
        mockTransactionService.Verify(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId), Times.Once);
        mockPaymentGatewayService.Verify(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId), Times.Once);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithEmptyResponseLogs_ShouldNotCallTransactionService()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1,
            IP = "127.0.0.1"
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 1000m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 4,
            Message = "Provider rejected"
        };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId))
            .ReturnsAsync(paymentGatewayResponse);

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WriteLogDBCommandModel>()); // Empty list

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(410, result.Code);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<List<WriteLogDBCommandModel>>()), Times.Never);
    }

    [Fact]
    public async Task ApproveByPaymentGatewayAsync_WithValidResponseLogs_ShouldCallTransactionService()
    {
        // Arrange
        var command = new ProcessWithdrawalTransaction
        {
            TransId = 123,
            CustId = 1,
            ProviderId = 1,
            PaymentFrom = "Test",
            PaymentMethodId = 1,
            ActorId = 1,
            SubAccId = 1,
            SiteDBGroupId = 1,
            CustDBGroupId = 1,
            IP = "127.0.0.1"
        };

        var transaction = new TransactionModel
        {
            PaymentTo = "BANK-123456789",
            Amount = 1000m,
            Username = "TestUser"
        };

        var paymentGatewayResponse = new PaymentResponse
        {
            Code = 4,
            Message = "Provider rejected"
        };

        var responseLogs = new List<WriteLogDBCommandModel> { new WriteLogDBCommandModel() };

        mockTransactionService
            .Setup(s => s.GetTransactionByIdAsync(command.TransId, command.ActorId, command.SiteDBGroupId))
            .ReturnsAsync(transaction);

        mockPaymentGatewayService
            .Setup(s => s.WithdrawalViaPaymentGateway(It.IsAny<WithdrawalBodyRequest>(), command.CustDBGroupId))
            .ReturnsAsync(paymentGatewayResponse);

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<WriteLogDBCommandModel>(It.IsAny<UpdateWithdrawalTransactionByApiCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(responseLogs);

        // Act
        var result = await service.ApproveByPaymentGatewayAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(410, result.Code);
        mockTransactionService.Verify(s => s.OnAfterUpdateTransaction(string.Empty, command.SiteDBGroupId, command.CustDBGroupId, responseLogs), Times.Once);
    }
}
