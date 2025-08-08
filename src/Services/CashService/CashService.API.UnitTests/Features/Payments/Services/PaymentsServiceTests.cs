using AlphaPlus.CashService.API.Features.Payments.DTOs;
using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Payments.Services;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Models.PaymentGateway;
using AlphaPlus.CashService.API.Features.Shared.Requests.PaymentGateway;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Data.Repository;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Services;

public class PaymentsServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ICacheRefService> _mockCacheRefService;
    private readonly Mock<ICommonService> _mockCommonService;
    private readonly Mock<IPaymentGatewayService> _mockPaymentGatewayService;
    private readonly PaymentsService _paymentsService;  

    public PaymentsServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockCacheRefService = new Mock<ICacheRefService>();
        _mockCommonService = new Mock<ICommonService>();
        _mockPaymentGatewayService = new Mock<IPaymentGatewayService>();
        _paymentsService = new PaymentsService(_mockRepository.Object, _mockCacheRefService.Object, _mockCommonService.Object, _mockPaymentGatewayService.Object);
    }

    [Fact]
    public async Task GetProviders_WhenCalled_ReturnsTranslatedProviders()
    {
        // Arrange
        var custId = 1;
        var languageCode = "en";
        var providers = new List<ProviderModel>
        {
            new() { Id = 1, Name = "Provider1" },
            new() { Id = 2, Name = "Provider2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<ProviderModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(providers);

        _mockCacheRefService.Setup(c => c.GetProviderName(1, "Provider1", languageCode))
            .Returns("Translated Provider1");
        _mockCacheRefService.Setup(c => c.GetProviderName(2, "Provider2", languageCode))
            .Returns("Translated Provider2");

        // Act
        var result = await _paymentsService.GetProviders(custId, languageCode);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Translated Provider1", result.First().Name);
        Assert.Equal("Translated Provider2", result.Last().Name);
    }

    [Fact]
    public async Task GetDailyWithdrawalCount_WhenCalled_ReturnsCount()
    {
        // Arrange
        var custId = 1;
        var expectedCount = 5;
        _mockRepository.Setup(r => r.FetchAsync<int>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<int> { expectedCount });

        // Act
        var result = await _paymentsService.GetDailyWithdrawalCount(custId);

        // Assert
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task GetTransactionLimit_WhenCalled_ReturnsCorrectLimits()
    {
        // Arrange
        var custId = 1;
        var depositLimit = new DailyLimitDTO { TransType = 0, MinPer = 100, MaxPer = 1000 };
        var withdrawalLimit = new DailyLimitDTO { TransType = 1, MinPer = 200, MaxPer = 2000 };
        var dailyCount = new List<short> { 3 };

        _mockRepository.Setup(r => r.FetchMultipleAsync<DailyLimitDTO, short>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<DailyLimitDTO> { depositLimit, withdrawalLimit }, dailyCount));

        // Act
        var result = await _paymentsService.GetTransactionLimit(custId);

        // Assert
        Assert.Equal(100, result.MinPerDepositTransaction);
        Assert.Equal(1000, result.MaxPerDepositTransaction);
        Assert.Equal(200, result.MinPerWithdrawalTransaction);
        Assert.Equal(2000, result.MaxPerWithdrawalTransaction);
        Assert.Equal(3, result.DailyWithdrawalCount);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenInvalidRequest_ReturnsErrorResult()
    {
        // Arrange
        var request = new InsertPaymentMethodRequest
        {
            ActorId = 0, // Invalid actor ID
            ProviderId = 0  // Invalid provider ID
        };

        // Act
        var result = await _paymentsService.InsertPaymentMethod(request);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, result.Code);
        Assert.Equal("Invalid customer ID or provider ID", result.Message);
    }

    [Fact]
    public async Task UpdateProvider_WhenStatusUpdateFails_ReturnsErrorResult()
    {
        // Arrange
        var request = new UpdateProviderRequest
        {
            Status = 1,
            ProviderSettings = []
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(1, "Update failed"));

        // Act
        var result = await _paymentsService.UpdateProvider(request);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, result.Code);
        Assert.Equal("Update failed", result.Message);
    }

    [Fact]
    public async Task UpdateProvider_WhenStatusUpdateSucceedsAndNoProviderSettings_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateProviderRequest
        {
            Status = 1,
            ProviderSettings = []
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.UpdateProvider(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("", result.Message);
    }

    [Fact]
    public async Task UpdateProvider_WhenStatusUpdateSucceedsAndHasProviderSettings_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateProviderRequest
        {
            Status = 1,
            ProviderSettings = new List<ProviderSettingsModel> { new ProviderSettingsModel() } // Non-empty settings
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.UpdateProvider(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task GetProviderFees_WhenCalled_ReturnsTranslatedFees()
    {
        // Arrange
        var providerId = 1;
        var languageCode = "en";
        var fees = new List<ProviderFeeModel>
        {
            new() { MethodId = 1, Method = "Method1", TransactionType = "1" },
            new() { MethodId = 2, Method = "Method2", TransactionType = "1" }
        };

        _mockRepository.Setup(r => r.FetchAsync<ProviderFeeModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fees);

        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(1, "Method1", languageCode))
            .Returns("Translated Method1");
        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(2, "Method2", languageCode))
            .Returns("Translated Method2");

        // Act
        var result = await _paymentsService.GetProviderFees(providerId, languageCode);

        // Assert
        Assert.Single(result);
        var group = result.First();
        Assert.Equal("1", group.TransactionType);
        Assert.Equal(2, group.Fees.Count);
        Assert.Equal("Translated Method1", group.Fees[0].Method);
        Assert.Equal("Translated Method2", group.Fees[1].Method);
    }

    [Fact]
    public async Task GetProviderById_WhenProviderExists_ReturnsProvider()
    {
        // Arrange
        var custId = 1;
        var providerId = 1;
        var languageCode = "en";
        var providers = new List<ProviderModel>
        {
            new() { Id = 1, Name = "Provider1" },
            new() { Id = 2, Name = "Provider2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<ProviderModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(providers);

        _mockCacheRefService.Setup(c => c.GetProviderName(1, "Provider1", languageCode))
            .Returns("Translated Provider1");

        // Act
        var result = await _paymentsService.GetProviderById(custId, providerId, languageCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Translated Provider1", result.Name);
    }

    [Fact]
    public async Task GetAddPaymentMethodInfo_WhenCalled_ReturnsPaymentMethodsAndChannelContact()
    {
        // Arrange
        var custId = 1;
        var providerId = 1;
        var transactionType = 1;
        var languageCode = "en";
        var methods = new List<PaymentMethodModel>
        {
            new() { Id = 1, Name = "Method1" },
            new() { Id = 2, Name = "Method2" }
        };

        var channelContact = new ChannelContactModel { Tele = "test@example.com" };

        _mockRepository.Setup(r => r.FetchAsync<PaymentMethodModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(methods);

        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(1, "Method1", languageCode))
            .Returns("Translated Method1");
        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(2, "Method2", languageCode))
            .Returns("Translated Method2");

        // Act
        var result = await _paymentsService.GetAddPaymentMethodInfo(custId, providerId, transactionType, languageCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.PaymentMethods.Count());
        Assert.Equal("Translated Method1", result.PaymentMethods.First().Name);
        Assert.Equal("Translated Method2", result.PaymentMethods.Last().Name);
    }

    [Fact]
    public async Task GetBanks_WhenCalled_ReturnsTranslatedBanks()
    {
        // Arrange
        var site = "test";
        var transactionType = 1;
        var languageCode = "en";
        var banks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank1", DepositMode = JsonConvert.SerializeObject(new DepositMatchingModeModel { SourceType = "1" }) },
            new() { Id = 2, Bank = "Bank2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<BankModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(banks);

        _mockCacheRefService.Setup(c => c.GetBankName(1, "Bank1", languageCode))
            .Returns("Translated Bank1");
        _mockCacheRefService.Setup(c => c.GetBankName(2, "Bank2", languageCode))
            .Returns("Translated Bank2");
        _mockCacheRefService.Setup(c => c.GetDepositAutoModeName("1", "1", languageCode))
            .Returns("Translated Source Type");

        // Act
        var result = await _paymentsService.GetBanks(site, transactionType, languageCode);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Translated Bank1", result.First().Name);
        Assert.Equal("Translated Bank2", result.Last().Name);
        Assert.NotNull(result.First().DepositMatchingMode);
        Assert.Equal("Translated Source Type", result.First().DepositMatchingMode.SourceTypeName);
    }

    [Fact]
    public async Task UpdatePaymentMethod_WhenInvalidRequest_ReturnsErrorResult()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 0, // Invalid actor ID
            ProviderId = 0  // Invalid provider ID
        };

        // Act
        var result = await _paymentsService.UpdatePaymentMethod(request);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, result.Code);
        Assert.Equal("Invalid customer ID or provider ID", result.Message);
    }

    [Fact]
    public async Task UpdatePaymentMethod_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdatePaymentMethodRequest
        {
            ActorId = 1,
            ProviderId = 1,
            APIMethodKey = "key",
            MerchantID = "merchant",
            AppSecret = "secret"
        };

        _mockPaymentGatewayService.Setup(p => p.TestConnection(It.IsAny<TestConnectionRequest>(), It.IsAny<UpdatePaymentMethodRequest>()))
            .ReturnsAsync(new PaymentResponse { Code = 0, Message = "Success" });

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.UpdatePaymentMethod(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task SetDailyWithdrawalCount_WhenCalled_ReturnsResult()
    {
        // Arrange
        var request = new SetDailyWithdrawalCountRequest
        {
            ActorId = 1,
            Count = 5
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.SetDailyWithdrawalCount(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task GetProviderSettings_WhenCalled_ReturnsTranslatedSettings()
    {
        // Arrange
        var providerId = 1;
        var custId = 1;
        var method = 1;
        var status = 1;
        var transactionType = 1;
        var languageCode = "en";
        var settings = new List<ProviderSettingsModel>
        {
            new() { Method = 1, MethodName = "Method1", TransactionType = 1 },
            new() { Method = 2, MethodName = "Method2", TransactionType = 1 }
        };

        _mockRepository.Setup(r => r.FetchAsync<ProviderSettingsModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(1, "Method1", languageCode))
            .Returns("Translated Method1");
        _mockCacheRefService.Setup(c => c.GetPaymentMethodName(2, "Method2", languageCode))
            .Returns("Translated Method2");

        // Act
        var result = await _paymentsService.GetProviderSettings(providerId, custId, method, status, transactionType, languageCode);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Translated Method1", result.First().MethodName);
        Assert.Equal("Translated Method2", result.Last().MethodName);
    }

    [Fact]
    public async Task GetBanksByCust_WhenCalled_ReturnsTranslatedBanks()
    {
        // Arrange
        var custId = 1;
        var languageCode = "en";
        var banks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank1", DepositMode = JsonConvert.SerializeObject(new DepositMatchingModeModel { SourceType = "1" }) },
            new() { Id = 2, Bank = "Bank2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<BankModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(banks);

        _mockCacheRefService.Setup(c => c.GetBankName(1, "Bank1", languageCode))
            .Returns("Translated Bank1");
        _mockCacheRefService.Setup(c => c.GetBankName(2, "Bank2", languageCode))
            .Returns("Translated Bank2");
        _mockCacheRefService.Setup(c => c.GetDepositAutoModeName("1", "1", languageCode))
            .Returns("Translated Source Type");

        // Act
        var result = await _paymentsService.GetBanksByCust(custId, languageCode);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Translated Bank1", result.First().Name);
        Assert.Equal("Translated Bank2", result.Last().Name);
        Assert.NotNull(result.First().DepositMatchingMode);
        Assert.Equal("Translated Source Type", result.First().DepositMatchingMode.SourceTypeName);
    }

    [Fact]
    public async Task GetTransactionTypes_WhenCalled_ReturnsTypes()
    {
        // Arrange
        var types = new List<TransactionTypeModel>
        {
            new() { Id = 1, Name = "Type1" },
            new() { Id = 2, Name = "Type2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<TransactionTypeModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(types);

        // Act
        var result = await _paymentsService.GetTransactionTypes();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Type1", result.First().Name);
        Assert.Equal("Type2", result.Last().Name);
    }

    [Fact]
    public async Task GetBankAccounts_WhenCalled_ReturnsAccounts()
    {
        // Arrange
        var custId = 1;
        var bankId = 1;
        var transactionType = 1;
        var status = 1;
        var searchText = "test";
        var languageCode = "en";
        var accounts = new List<BankAccountModel>
        {
            new() { Id = 1, AccountNumber = "123", AccountNickname = "Test Account 1", BankId = 1, BankName = "Bank1", Bank_DepositMatchingMode = JsonConvert.SerializeObject(new DepositMatchingModeModel { SourceType = "1" }) },
            new() { Id = 2, AccountNumber = "456", AccountNickname = "Test Account 2", BankId = 2, BankName = "Bank2" }
        };

        _mockRepository.Setup(r => r.FetchAsync<BankAccountModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts);

        _mockCacheRefService.Setup(c => c.GetBankName(1, "Bank1", languageCode))
            .Returns("Translated Bank1");
        _mockCacheRefService.Setup(c => c.GetBankName(2, "Bank2", languageCode))
            .Returns("Translated Bank2");
        _mockCacheRefService.Setup(c => c.GetDepositAutoModeName("1", "1", languageCode))
            .Returns("Translated Source Type");

        // Act
        var result = await _paymentsService.GetBankAccounts(custId, bankId, transactionType, status, searchText, languageCode);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("123", result.First().AccountNumber);
        Assert.Equal("456", result.Last().AccountNumber);
        Assert.Equal("Translated Bank1", result.First().BankName);
        Assert.Equal("Translated Bank2", result.Last().BankName);
        Assert.NotNull(result.First().BankDepositMatchingMode);
        Assert.Equal("Translated Source Type", result.First().BankDepositMatchingMode.SourceTypeName);
    }

    [Fact]
    public async Task GetBankAccountById_WhenAccountExists_ReturnsAccount()
    {
        // Arrange
        var accountId = 1;
        var languageCode = "en";
        var account = new BankAccountModel
        {
            Id = 1,
            AccountNumber = "123",
            AccountNickname = "Test Account",
            BankId = 1,
            BankName = "Bank1",
            Bank_DepositMatchingMode = JsonConvert.SerializeObject(new DepositMatchingModeModel { SourceType = "1" })
        };

        _mockRepository.Setup(r => r.FetchAsync<BankAccountModel>(It.IsAny<ICriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BankAccountModel> { account });

        _mockCacheRefService.Setup(c => c.GetBankName(1, "Bank1", languageCode))
            .Returns("Translated Bank1");
        _mockCacheRefService.Setup(c => c.GetDepositAutoModeName("1", "1", languageCode))
            .Returns("Translated Source Type");

        // Act
        var result = await _paymentsService.GetBankAccountById(accountId, languageCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("123", result.AccountNumber);
        Assert.Equal("Test Account", result.AccountNickname);
        Assert.Equal("Translated Bank1", result.BankName);
        Assert.NotNull(result.BankDepositMatchingMode);
        Assert.Equal("Translated Source Type", result.BankDepositMatchingMode.SourceTypeName);
    }

    [Fact]
    public async Task AddBankAccount_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new AddBankAccountRequest
        {
            ActorId = 1,
            BankId = 1,
            AccountNumber = "123",
            AccountNickname = "Test Account"
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.AddBankAccount(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task UpdateBankAccount_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateBankAccountRequest
        {
            ActorId = 1,
            Id = 1,
            AccountNickname = "Updated Account",
            Status = 1,
            BankId = 1,
            BankName = "Test Bank",
            TransactionTypeID = "1",
            Agent_TransTypeList = "1",
            Agent_DepositMatchingMode = 1
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.UpdateBankAccount(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task DeleteBankAccount_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new DeleteBankAccountRequest
        {
            ActorId = 1,
            BankAccountId = 1
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.DeleteBankAccount(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }

    [Fact]
    public async Task SetTransactionLimit_WhenValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new SetTransactionLimitRequest
        {
            ActorId = 1,
            MinPerDepositTransaction = 100,
            MaxPerDepositTransaction = 1000,
            MinPerWithdrawalTransaction = 200,
            MaxPerWithdrawalTransaction = 2000
        };

        _mockCommonService.Setup(c => c.RunDBCommand(request, It.IsAny<BaseDBCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _paymentsService.SetTransactionLimit(request);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Code);
        Assert.Equal("Success", result.Message);
    }
}