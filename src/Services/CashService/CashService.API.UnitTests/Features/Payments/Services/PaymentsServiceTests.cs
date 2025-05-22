using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Payments.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Payments.DTOs;
using Fanex.Data.Repository;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Services;

public class PaymentsServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ICacheRefService> _cacheRefServiceMock;
    private readonly Mock<ICommonService> _commonServiceMock;
    private readonly PaymentsService _paymentsService;

    public PaymentsServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _cacheRefServiceMock = new Mock<ICacheRefService>();
        _commonServiceMock = new Mock<ICommonService>();
        _paymentsService = new PaymentsService(_repositoryMock.Object, _cacheRefServiceMock.Object, _commonServiceMock.Object);
    }

    [Fact]
    public async Task GetProviders_WhenCalled_ReturnsProviderList()
    {
        // Arrange
        var custId = 1;
        var providers = new List<ProviderModel>
        {
            new ProviderModel { Id = 1, Name = "Provider1" },
            new ProviderModel { Id = 2, Name = "Provider2" }
        };

        _repositoryMock.Setup(x => x.FetchAsync<ProviderModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(providers);
        _cacheRefServiceMock.Setup(x => x.GetProviderName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviders(custId);

        // Assert
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(x => x.FetchAsync<ProviderModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDailyWithdrawalCount_WhenCalled_ReturnsCount()
    {
        // Arrange
        var custId = 1;
        var expectedCount = 5;
        _repositoryMock.Setup(x => x.FetchAsync<int>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<int> { expectedCount });

        // Act
        var result = await _paymentsService.GetDailyWithdrawalCount(custId);

        // Assert
        Assert.Equal(expectedCount, result);
        _repositoryMock.Verify(x => x.FetchAsync<int>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProvider_WhenStatusIsValid_UpdatesSuccessfully()
    {
        // Arrange
        var request = new UpdateProviderRequest
        {
            ActorId = 1,
            SubAccId = 1,
            IP = "127.0.0.1",
            Language = "en",
            ProviderId = 1,
            Status = 1
        };

        _commonServiceMock.Setup(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, string.Empty));

        // Act
        var result = await _paymentsService.UpdateProvider(request);

        // Assert
        Assert.True(result.IsSuccessful);
        _commonServiceMock.Verify(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()), Times.Exactly(1));
    }

    [Fact]
    public async Task UpdateProvider_WhenStatusIsInvalid_ReturnsSuccessWithoutUpdate()
    {
        // Arrange
        var request = new UpdateProviderRequest
        {
            ProviderId = 1,
            Status = -1
        };

        // Act
        var result = await _paymentsService.UpdateProvider(request);

        // Assert
        Assert.True(result.IsSuccessful);
        _commonServiceMock.Verify(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()), Times.Never);
    }

    [Fact]
    public async Task GetProviderFees_WhenCalled_ReturnsGroupedFees()
    {
        // Arrange
        var providerId = 1;
        var fees = new List<ProviderFeeModel>
        {
            new ProviderFeeModel { MethodId = 1, Method = "Method1", TransactionType = "1" },
            new ProviderFeeModel { MethodId = 2, Method = "Method2", TransactionType = "1" }
        };

        _repositoryMock.Setup(x => x.FetchAsync<ProviderFeeModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(fees);
        _cacheRefServiceMock.Setup(x => x.GetPaymentMethodName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviderFees(providerId);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result.First().Fees.Count);
        _repositoryMock.Verify(x => x.FetchAsync<ProviderFeeModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenRequestIsValid_InsertsSuccessfully()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 1,
            SubAccId = 1,
            IP = "127.0.0.1",
            Language = "en",
            ProviderId = 1,
            APIMethodKey = "key",
            MerchantID = "merchant",
            AppSecret = "secret"
        };

        _commonServiceMock.Setup(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, string.Empty));

        // Act
        var result = await _paymentsService.InsertPaymentMethod(request);

        // Assert
        Assert.True(result.IsSuccessful);
        _commonServiceMock.Verify(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()), Times.Once);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenRequestIsInvalid_ReturnsError()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ProviderId = 0
        };

        // Act
        var result = await _paymentsService.InsertPaymentMethod(request);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, result.Code);
        _commonServiceMock.Verify(x => x.RunDBCommand(It.IsAny<BaseActionRequest>(), It.IsAny<BaseDBCommand>()), Times.Never);
    }

    [Fact]
    public async Task GetBanks_WhenCalled_ReturnsBanksWithNames()
    {
        // Arrange
        var transactionType = 1;
        var banks = new List<BankModel>
        {
            new BankModel { Id = 1, Bank = "Bank1" },
            new BankModel { Id = 2, Bank = "Bank2" }
        };

        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(banks);
        _cacheRefServiceMock.Setup(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetBanks(transactionType);

        // Assert
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(x => x.FetchAsync<BankModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTransactionLimit_WhenCalled_ReturnsLimits()
    {
        // Arrange
        var custId = 1;
        var limits = new List<DailyLimitDTO>
        {
            new DailyLimitDTO { TransType = 0, MinPer = 100, MaxPer = 1000 },
            new DailyLimitDTO { TransType = 1, MinPer = 200, MaxPer = 2000 }
        };
        var count = new List<short> { 5 };

        _repositoryMock.Setup(x => x.FetchMultipleAsync<DailyLimitDTO, short>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync((limits, count));

        // Act
        var result = await _paymentsService.GetTransactionLimit(custId);

        // Assert
        Assert.Equal(100, result.MinPerDepositTransaction);
        Assert.Equal(1000, result.MaxPerDepositTransaction);
        Assert.Equal(200, result.MinPerWithdrawalTransaction);
        Assert.Equal(2000, result.MaxPerWithdrawalTransaction);
        Assert.Equal(5, result.DailyWithdrawalCount);
        _repositoryMock.Verify(x => x.FetchMultipleAsync<DailyLimitDTO, short>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }
} 