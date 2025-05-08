using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Services;
using CashService.API.DTOs.Payments;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Services;

public class PaymentsServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ICacheRefService> _mockCacheRefService;
    private readonly PaymentsService _paymentsService;

    public PaymentsServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockCacheRefService = new Mock<ICacheRefService>();
        _paymentsService = new PaymentsService(_mockRepository.Object, _mockCacheRefService.Object);
    }

    [Fact]
    public async Task GetProviders_WhenCalled_ReturnsProviderList()
    {
        // Arrange
        var custId = 1;
        var expectedProviders = new List<ProviderModel>
        {
            new ProviderModel { Id = 1, Name = "Provider1" },
            new ProviderModel { Id = 2, Name = "Provider2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<ProviderModel>(It.IsAny<GetProvidersCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedProviders);

        _mockCacheRefService.Setup(x => x.GetProviderName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviders(custId);

        // Assert
        Assert.Equal(expectedProviders.Count(), result.Count());
        _mockRepository.Verify(x => x.FetchAsync<ProviderModel>(It.IsAny<GetProvidersCriteria>(),
                string.Empty,
                CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetDailyWithdrawalCount_WhenCalled_ReturnsCount()
    {
        // Arrange
        var custId = 1;
        var expectedCount = 5;
        var counts = new List<int> { expectedCount };

        _mockRepository.Setup(x => x.FetchAsync<int>(It.IsAny<GetDailyLimitCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(counts);

        // Act
        var result = await _paymentsService.GetDailyWithdrawalCount(custId);

        // Assert
        Assert.Equal(expectedCount, result);
        _mockRepository.Verify(x => x.FetchAsync<int>(It.IsAny<GetDailyLimitCriteria>(),
                string.Empty,
                CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SetDailyWithdrawalCount_WhenValidCount_ReturnsSuccess()
    {
        // Arrange
        var custId = 1;
        var count = 5;

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<SetDailyWithdrawalCountCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.SetDailyWithdrawalCount(custId, count);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<SetDailyWithdrawalCountCriteria>(),
                string.Empty,
                CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetTransactionLimit_WhenCalled_ReturnsTransactionLimits()
    {
        // Arrange
        var custId = 1;
        var expectedLimits = new List<DailyLimitDTO>
        {
            new() { IsDeposit = true, MinPer = 100, MaxPer = 1000 },
            new() { IsDeposit = false, MinPer = 200, MaxPer = 2000 }
        };
        var counts = new List<Int16> { 5 };

        _mockRepository.Setup(x => x.FetchMultipleAsync<DailyLimitDTO, Int16>(It.IsAny<GetDailyLimitCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync((expectedLimits, counts));

        // Act
        var result = await _paymentsService.GetTransactionLimit(custId);

        // Assert
        Assert.Equal(5, result.DailyWithdrawalCount);
        Assert.Equal(100, result.MinPerDepositTransaction);
        Assert.Equal(1000, result.MaxPerDepositTransaction);
        Assert.Equal(200, result.MinPerWithdrawalTransaction);
        Assert.Equal(2000, result.MaxPerWithdrawalTransaction);
    }

    [Fact]
    public async Task DeleteBankAccount_WhenInvalidAccountId_ReturnsError()
    {
        // Arrange
        var invalidAccountId = 0;

        // Act
        var result = await _paymentsService.DeleteBankAccount(invalidAccountId);

        // Assert
        Assert.Equal(1, result.Code);
        Assert.Equal("Invalid account ID", result.Message);
    }

    [Fact]
    public async Task GetBankAccountById_WhenAccountExists_ReturnsBankAccount()
    {
        // Arrange
        var accountId = 1;
        var expectedAccount = new BankAccountModel
        {
            Id = accountId,
            BankName = "Test Bank"
        };
        var accounts = new List<BankAccountModel> { expectedAccount };

        _mockRepository.Setup(x => x.FetchAsync<BankAccountModel>(It.IsAny<GetBankAccountByIdCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(accounts);

        _mockCacheRefService.Setup(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetBankAccountById(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accountId, result.Id);
        Assert.Equal("Test Bank", result.BankName);
    }

    [Fact]
    public async Task GetProviderFees_WhenCalled_ReturnsGroupedFees()
    {
        // Arrange
        var providerId = 1;
        var expectedFees = new List<ProviderFeeModel>
        {
            new() { MethodId = 1, Method = "1", TransactionType = "1" },
            new() { MethodId = 2, Method = "2", TransactionType = "1" },
            new() { MethodId = 3, Method = "3", TransactionType = "2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<ProviderFeeModel>(It.IsAny<GetProvidersFeesCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedFees);

        _mockCacheRefService.Setup(x => x.GetPaymentMethodName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviderFees(providerId);

        // Assert
        Assert.Equal(2, result.Count()); // Should have 2 groups (TransactionType 1 and 2)
        var group1 = result.First(g => g.TransactionType == "1");
        Assert.Equal(2, group1.Fees.Count);
        var group2 = result.First(g => g.TransactionType == "2");
        Assert.Single(group2.Fees);
    }

    [Fact]
    public async Task GetProviderById_WhenProviderExists_ReturnsProvider()
    {
        // Arrange
        var custId = 1;
        var providerId = 1;
        var expectedProviders = new List<ProviderModel>
        {
            new() { Id = 1, Name = "Provider1" },
            new() { Id = 2, Name = "Provider2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<ProviderModel>(It.IsAny<GetProvidersCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedProviders);

        _mockCacheRefService.Setup(x => x.GetProviderName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviderById(custId, providerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(providerId, result.Id);
        Assert.Equal("Provider1", result.Name);
    }

    [Fact]
    public async Task GetProviderById_WhenProviderDoesNotExist_ReturnsNull()
    {
        // Arrange
        var custId = 1;
        var providerId = 999;
        var expectedProviders = new List<ProviderModel>
        {
            new() { Id = 1, Name = "Provider1" },
            new() { Id = 2, Name = "Provider2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<ProviderModel>(It.IsAny<GetProvidersCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedProviders);

        // Act
        var result = await _paymentsService.GetProviderById(custId, providerId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPaymentMethods_WhenCalled_ReturnsMethods()
    {
        // Arrange
        var custId = 1;
        var providerId = 1;
        var transactionType = 1;
        var expectedMethods = new List<PaymentMethodModel>
        {
            new() { Id = 1, Name = "Method1", CustId = custId, ProviderId = providerId, TransactionType = transactionType },
            new() { Id = 2, Name = "Method2", CustId = custId, ProviderId = providerId, TransactionType = transactionType }
        };

        _mockRepository.Setup(x => x.FetchAsync<PaymentMethodModel>(It.IsAny<GetPaymentMethodsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedMethods);

        _mockCacheRefService.Setup(x => x.GetPaymentMethodName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetPaymentMethods(custId, providerId, transactionType);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, method => 
        {
            Assert.Equal(custId, method.CustId);
            Assert.Equal(providerId, method.ProviderId);
            Assert.Equal(transactionType, method.TransactionType);
        });
    }

    [Fact]
    public async Task GetProviderSettings_WhenCalled_ReturnsSettings()
    {
        // Arrange
        var providerId = 1;
        var method = 1;
        var status = 1;
        var transactionType = 1;
        var expectedSettings = new List<ProviderSettingsModel>
        {
            new() 
            { 
                ProviderId = providerId,
                Method = method,
                MethodName = "Method1",
                Status = status,
                TransactionType = transactionType,
                MinAmount = 100,
                MaxAmount = 1000,
                IsEnabled = true
            }
        };

        _mockRepository.Setup(x => x.FetchAsync<ProviderSettingsModel>(It.IsAny<GetProviderSettingsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedSettings);

        _mockCacheRefService.Setup(x => x.GetPaymentMethodName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetProviderSettings(providerId, method, status, transactionType);

        // Assert
        Assert.Single(result);
        var setting = result.First();
        Assert.Equal(providerId, setting.ProviderId);
        Assert.Equal(method, setting.Method);
        Assert.Equal(status, setting.Status);
        Assert.Equal(transactionType, setting.TransactionType);
        Assert.Equal(100, setting.MinAmount);
        Assert.Equal(1000, setting.MaxAmount);
        Assert.True(setting.IsEnabled);
    }

    [Fact]
    public async Task GetProviders_WhenRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var custId = 1;
        _mockRepository.Setup(x => x.FetchAsync<ProviderModel>(It.IsAny<GetProvidersCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _paymentsService.GetProviders(custId));
    }

    [Fact]
    public async Task GetDailyWithdrawalCount_WhenNoResults_ReturnsZero()
    {
        // Arrange
        var custId = 1;
        var emptyCounts = new List<int>();

        _mockRepository.Setup(x => x.FetchAsync<int>(It.IsAny<GetDailyLimitCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(emptyCounts);

        // Act
        var result = await _paymentsService.GetDailyWithdrawalCount(custId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetTransactionLimit_WhenNoLimitsFound_ReturnsDefaultValues()
    {
        // Arrange
        var custId = 1;
        var emptyLimits = new List<DailyLimitDTO>();
        var emptyCounts = new List<Int16>();

        _mockRepository.Setup(x => x.FetchMultipleAsync<DailyLimitDTO, Int16>(It.IsAny<GetDailyLimitCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync((emptyLimits, emptyCounts));

        // Act
        var result = await _paymentsService.GetTransactionLimit(custId);

        // Assert
        Assert.Equal(0, result.DailyWithdrawalCount);
        Assert.Equal(0, result.MinPerDepositTransaction);
        Assert.Equal(0, result.MaxPerDepositTransaction);
        Assert.Equal(0, result.MinPerWithdrawalTransaction);
        Assert.Equal(0, result.MaxPerWithdrawalTransaction);
    }

    [Fact]
    public async Task GetBankAccountById_WhenAccountNotFound_ReturnsNull()
    {
        // Arrange
        var accountId = 999;
        var emptyAccounts = new List<BankAccountModel>();

        _mockRepository.Setup(x => x.FetchAsync<BankAccountModel>(It.IsAny<GetBankAccountByIdCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(emptyAccounts);

        // Act
        var result = await _paymentsService.GetBankAccountById(accountId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProviderFees_WhenNoFeesFound_ReturnsEmptyList()
    {
        // Arrange
        var providerId = 1;
        var emptyFees = new List<ProviderFeeModel>();

        _mockRepository.Setup(x => x.FetchAsync<ProviderFeeModel>(It.IsAny<GetProvidersFeesCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(emptyFees);

        // Act
        var result = await _paymentsService.GetProviderFees(providerId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPaymentMethods_WhenNoMethodsFound_ReturnsEmptyList()
    {
        // Arrange
        var custId = 1;
        var providerId = 1;
        var transactionType = 1;
        var emptyMethods = new List<PaymentMethodModel>();

        _mockRepository.Setup(x => x.FetchAsync<PaymentMethodModel>(It.IsAny<GetPaymentMethodsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(emptyMethods);

        // Act
        var result = await _paymentsService.GetPaymentMethods(custId, providerId, transactionType);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProviderSettings_WhenNoSettingsFound_ReturnsEmptyList()
    {
        // Arrange
        var providerId = 1;
        var method = 1;
        var status = 1;
        var transactionType = 1;
        var emptySettings = new List<ProviderSettingsModel>();

        _mockRepository.Setup(x => x.FetchAsync<ProviderSettingsModel>(It.IsAny<GetProviderSettingsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(emptySettings);

        // Act
        var result = await _paymentsService.GetProviderSettings(providerId, method, status, transactionType);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var paymentMethod = new PaymentMethodModel
        {
            CustId = 1,
            ProviderId = 1,
            TransactionType = 1,
            Method = 1,
            APIMethodKey = "key",
            MerchantID = "merchant",
            AppSecret = "secret"
        };

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<InsertPaymentMethodCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.InsertPaymentMethod(paymentMethod);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<InsertPaymentMethodCriteria>(),
                string.Empty,
                CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenInvalidData_ReturnsError()
    {
        // Arrange
        var paymentMethod = new PaymentMethodModel
        {
            CustId = 0,
            ProviderId = 0,
            TransactionType = 1,
            Method = 1
        };

        // Act
        var result = await _paymentsService.InsertPaymentMethod(paymentMethod);

        // Assert
        Assert.Equal(1, result.Code);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public async Task GetBanks_WhenCalled_ReturnsBankList()
    {
        // Arrange
        var transactionType = 1;
        var expectedBanks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank1" },
            new() { Id = 2, Bank = "Bank2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetBanksByTransactionTypeCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedBanks);

        _mockCacheRefService.Setup(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetBanks(transactionType);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, bank => Assert.NotNull(bank.Name));
    }

    [Fact]
    public async Task UpdateProvider_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var provider = new ProviderModel
        {
            Id = 1,
            Name = "Updated Provider"
        };

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<UpdateProviderSettingsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.UpdateProvider(provider);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
    }

    [Fact]
    public async Task GetBanksByCust_WhenCalled_ReturnsBankList()
    {
        // Arrange
        var custId = 1;
        var expectedBanks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank1" },
            new() { Id = 2, Bank = "Bank2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetBanksByCustCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedBanks);

        _mockCacheRefService.Setup(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetBanksByCust(custId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, bank => Assert.NotNull(bank.Name));
    }

    [Fact]
    public async Task GetTransactionTypes_WhenCalled_ReturnsTypes()
    {
        // Arrange
        var expectedTypes = new List<TransactionTypeModel>
        {
            new() { Id = 1, Name = "Type1" },
            new() { Id = 2, Name = "Type2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<TransactionTypeModel>(It.IsAny<GetTransactionTypesCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedTypes);

        // Act
        var result = await _paymentsService.GetTransactionTypes();

        // Assert
        Assert.Equal(2, result.Count());
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
        var expectedAccounts = new List<BankAccountModel>
        {
            new() { Id = 1, BankName = "Test Bank" },
            new() { Id = 2, BankName = "Test Bank 2" }
        };

        _mockRepository.Setup(x => x.FetchAsync<BankAccountModel>(It.IsAny<GetBankAccountsCriteria>(),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedAccounts);

        _mockCacheRefService.Setup(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns((int id, string name) => name);

        // Act
        var result = await _paymentsService.GetBankAccounts(custId, bankId, transactionType, status, searchText);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, account => Assert.NotNull(account.BankName));
    }

    [Fact]
    public async Task SetTransactionLimit_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var custId = 1;
        var limit = new TransactionLimitModel
        {
            MinPerDepositTransaction = 100,
            MaxPerDepositTransaction = 1000,
            MinPerWithdrawalTransaction = 200,
            MaxPerWithdrawalTransaction = 2000
        };

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<SetTransactionLimitCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.SetTransactionLimit(custId, limit);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
    }

    [Fact]
    public async Task AddBankAccount_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            CreatorId = 1,
            BankId = 1,
            AccountNumber = "1234567890",
            AccountHolderName = "Test Account",
            AccountNickname = "Test Account",
            TransactionTypeID = "1,2,3",
            Status = 1
        };

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<AddBankAccountCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.AddBankAccount(bankAccount);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
    }

    [Fact]
    public async Task UpdateBankAccount_WhenValidData_ReturnsSuccess()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            Id = 1,
            CreatorId = 1,
            BankId = 1,
            AccountNumber = "1234567890",
            AccountHolderName = "Updated Account",
            AccountNickname = "Updated Account",
            TransactionTypeID = "1,2,3",
            Status = 1
        };

        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<UpdateBankAccountCriteria>(),
                string.Empty,
                CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        var result = await _paymentsService.UpdateBankAccount(bankAccount);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
    }
} 