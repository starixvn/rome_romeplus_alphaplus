using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Services;
using CashService.API.DTOs.Payments;
using Fanex.Data.Repository;
using Moq;
using Xunit;
using System.Threading;

namespace AlphaPlus.CashService.API.UnitTests.Services;

public class PaymentsServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly PaymentsService _service;

    public PaymentsServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _service = new PaymentsService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetProviders_WhenValidCustomerId_ReturnsProviders()
    {
        // Arrange
        var custId = 1;
        var expectedProviders = new List<ProviderModel>
        {
            new ProviderModel { Id = 1, CustId = custId, Name = "Provider 1" },
            new ProviderModel { Id = 2, CustId = custId, Name = "Provider 2" }
        };

        _repositoryMock
            .Setup(r => r.FetchAsync<ProviderModel>(
                It.Is<GetProvidersCriteria>(c => c.CustId == custId),
                string.Empty,
                CancellationToken.None))
            .ReturnsAsync(expectedProviders);

        // Act
        var result = await _service.GetProviders(custId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProviders.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<ProviderModel>(
            It.Is<GetProvidersCriteria>(c => c.CustId == custId),
            string.Empty,
            CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task GetDailyWithdrawalCount_WhenValidCustomerId_ReturnsCount()
    {
        // Arrange
        var custId = 1;
        var expectedCount = 5;

        _repositoryMock.Setup(r => r.FetchAsync<int>(
            It.Is<GetDailyLimitCriteria>(c => c.CustId == custId && c.ModeView == ModeViewConstants.PaymentGateway),
            string.Empty,
            CancellationToken.None))
            .ReturnsAsync(new List<int> { expectedCount });

        // Act
        var result = await _service.GetDailyWithdrawalCount(custId);

        // Assert
        Assert.Equal(expectedCount, result);
        _repositoryMock.Verify(r => r.FetchAsync<int>(
            It.Is<GetDailyLimitCriteria>(c => 
                c.CustId == custId && c.ModeView == ModeViewConstants.PaymentGateway),
            string.Empty,
            CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task SetDailyWithdrawalCount_WhenValidInput_ReturnsSuccess()
    {
        // Arrange
        var custId = 1;
        var count = 5;

        _repositoryMock.Setup(r => r.ExecuteAsync(
            It.IsAny<SetDailyWithdrawalCountCriteria>(),
            string.Empty,
            CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.SetDailyWithdrawalCount(custId, count);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Equal(string.Empty, result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(
            It.IsAny<SetDailyWithdrawalCountCriteria>(),
            string.Empty,
            CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task GetProviderFees_WhenValidProviderId_ReturnsGroupedFees()
    {
        // Arrange
        var providerId = 1;
        var fees = new List<ProviderFeeModel>
        {
            new() { ProviderId = providerId, TransactionType = "1", Method = "Method1", Rate = 10.5m },
            new() { ProviderId = providerId, TransactionType = "1", Method = "Method2", Rate = 20.5m },
            new() { ProviderId = providerId, TransactionType = "2", Method = "Method1", Rate = 30.5m }
        };

        _repositoryMock.Setup(r => r.FetchAsync<ProviderFeeModel>(
            It.Is<GetProvidersFeesCriteria>(c => c.ProviderId == providerId),
            string.Empty,
            CancellationToken.None))
            .ReturnsAsync(fees);

        // Act
        var result = await _service.GetProviderFees(providerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, g => g.TransactionType == "1" && g.Fees.Count == 2);
        Assert.Contains(result, g => g.TransactionType == "2" && g.Fees.Count == 1);
        _repositoryMock.Verify(r => r.FetchAsync<ProviderFeeModel>(
            It.Is<GetProvidersFeesCriteria>(c => 
                c.ProviderId == providerId),
            string.Empty,
            CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task GetProviderById_WhenValidIds_ReturnsProvider()
    {
        // Arrange
        var custId = 1;
        var providerId = 2;
        var expectedProvider = new ProviderModel { Id = providerId, CustId = custId, Name = "Provider 1" };
        var providers = new List<ProviderModel> { expectedProvider };

        _repositoryMock.Setup(r => r.FetchAsync<ProviderModel>(
            It.Is<GetProvidersCriteria>(c => 
                c.CustId == custId),
            string.Empty,
            CancellationToken.None))
            .ReturnsAsync(providers);

        // Act
        var result = await _service.GetProviderById(custId, providerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(providerId, result.Id);
        Assert.Equal(custId, result.CustId);
        _repositoryMock.Verify(r => r.FetchAsync<ProviderModel>(
            It.Is<GetProvidersCriteria>(c => 
                c.CustId == custId),
            string.Empty,
            CancellationToken.None), 
            Times.Once);
    }

    [Fact]
    public async Task GetPaymentMethods_WhenValidInput_ReturnsMethods()
    {
        // Arrange
        var custId = 1;
        var providerId = 2;
        var transactionType = 1;
        var expectedMethods = new List<PaymentMethodModel>
        {
            new() { Id = 1, Name = "Method 1" },
            new() { Id = 2, Name = "Method 2" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<PaymentMethodModel>(It.IsAny<GetPaymentMethodsCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedMethods);

        // Act
        var result = await _service.GetPaymentMethods(custId, providerId, transactionType);

        // Assert
        Assert.NotNull(result); 
        Assert.Equal(expectedMethods.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<PaymentMethodModel>(It.Is<GetPaymentMethodsCriteria>(c => 
            c.CustId == custId && c.ProviderId == providerId && c.TransactionType == transactionType), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetProviderSettings_WhenValidInput_ReturnsSettings()
    {
        // Arrange
        var providerId = 1;
        var method = 1;
        var status = 1;
        var transactionType = 1;
        var expectedSettings = new List<ProviderSettingsModel>
        {
            new() { ProviderId = providerId, Method = method, Status = status }
        };

        _repositoryMock.Setup(r => r.FetchAsync<ProviderSettingsModel>(It.IsAny<GetProviderSettingsCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedSettings);

        // Act
        var result = await _service.GetProviderSettings(providerId, method, status, transactionType);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _repositoryMock.Verify(r => r.FetchAsync<ProviderSettingsModel>(It.Is<GetProviderSettingsCriteria>(c => 
            c.ProviderId == providerId && c.Method == method && c.Status == status && c.TransactionType == transactionType), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task InsertPaymentMethod_WhenValidMethod_ReturnsSuccess()
    {
        // Arrange
        var paymentMethod = new PaymentMethodModel { Id = 1, Name = "Test Method" };

        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<InsertPaymentMethodCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.InsertPaymentMethod(paymentMethod);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<InsertPaymentMethodCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteBankAccount_WhenInvalidId_ReturnsError()
    {
        // Arrange
        var accountId = 0;

        // Act
        var result = await _service.DeleteBankAccount(accountId);

        // Assert
        Assert.Equal(1, result.Code);
        Assert.Equal("Invalid account ID", result.Message);
    }

    [Fact]
    public async Task GetMobilePrefixes_WhenCalled_ReturnsPrefixes()
    {
        // Act
        var result = await _service.GetMobilePrefixes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(15, result.Count());
        Assert.Contains(result, p => p.CountryCode == "US" && p.Prefix == "+1");
        Assert.Contains(result, p => p.CountryCode == "GB" && p.Prefix == "+44");
    }

    [Fact]
    public async Task GetBanks_WhenValidTransactionType_ReturnsBanks()
    {
        // Arrange
        var transactionType = 1;
        var expectedBanks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank 1" },
            new() { Id = 2, Bank = "Bank 2" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<BankModel>(It.IsAny<GetBanksByTransactionTypeCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedBanks);

        // Act
        var result = await _service.GetBanks(transactionType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBanks.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<BankModel>(It.Is<GetBanksByTransactionTypeCriteria>(c => 
            c.TransactionType == transactionType), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateProvider_WhenValidProvider_ReturnsSuccess()
    {
        // Arrange
        var provider = new ProviderModel 
        { 
            Id = 1, 
            CustId = 1,
            Status = 1,
            ProviderSettings = new List<ProviderSettingsModel>
            {
                new() { Method = 1, Status = 1, MinAmount = 10, MaxAmount = 100, IsEnabled = true }
            }
        };

        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<UpdateProviderStatusCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<UpdateProviderSettingsCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.UpdateProvider(provider);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<UpdateProviderStatusCriteria>(), string.Empty, CancellationToken.None), Times.Once);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<UpdateProviderSettingsCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetBanksByCust_WhenValidCustomerId_ReturnsBanks()
    {
        // Arrange
        var custId = 1;
        var expectedBanks = new List<BankModel>
        {
            new() { Id = 1, Bank = "Bank 1" },
            new() { Id = 2, Bank = "Bank 2" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<BankModel>(It.IsAny<GetBanksByCustCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedBanks);

        // Act
        var result = await _service.GetBanksByCust(custId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBanks.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<BankModel>(It.Is<GetBanksByCustCriteria>(c => 
            c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetTransactionTypes_WhenCalled_ReturnsTypes()
    {
        // Arrange
        var expectedTypes = new List<TransactionTypeModel>
        {
            new() { Id = 1, Name = "Type 1" },
            new() { Id = 2, Name = "Type 2" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<TransactionTypeModel>(It.IsAny<GetTransactionTypesCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedTypes);

        // Act
        var result = await _service.GetTransactionTypes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTypes.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<TransactionTypeModel>(It.IsAny<GetTransactionTypesCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetBankAccounts_WhenValidInput_ReturnsAccounts()
    {
        // Arrange
        var custId = 1;
        var bankId = 1;
        var transactionType = 1;
        var status = 1;
        var searchText = "test";
        var expectedAccounts = new List<BankAccountModel>
        {
            new() { Id = 1, BankId = bankId, AccountNickname = "Account 1" },
            new() { Id = 2, BankId = bankId, AccountNickname = "Account 2" }
        };

        _repositoryMock.Setup(r => r.FetchAsync<BankAccountModel>(It.IsAny<GetBankAccountsCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedAccounts);

        // Act
        var result = await _service.GetBankAccounts(custId, bankId, transactionType, status, searchText);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedAccounts.Count, result.Count());
        _repositoryMock.Verify(r => r.FetchAsync<BankAccountModel>(It.Is<GetBankAccountsCriteria>(c => 
            c.CustId == custId && c.BankId == bankId && c.TransactionType.ToString() == transactionType.ToString() && 
            c.Status == status && c.SearchText == searchText), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetTransactionLimit_WhenValidCustomerId_ReturnsLimit()
    {
        // Arrange
        var custId = 1;
        var limits = new List<DailyLimitDTO>
        {
            new() { IsDeposit = true, MinPer = 10, MaxPer = 1000 },
            new() { IsDeposit = false, MinPer = 20, MaxPer = 2000 }
        };
        var count = new List<int> { 5 };

        _repositoryMock.Setup(r => r.FetchMultipleAsync<DailyLimitDTO, int>(It.IsAny<GetDailyLimitCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync((limits, count));

        // Act
        var result = await _service.GetTransactionLimit(custId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.DailyWithdrawalCount);
        Assert.Equal(10, result.MinPerDepositTransaction);
        Assert.Equal(1000, result.MaxPerDepositTransaction);
        Assert.Equal(20, result.MinPerWithdrawalTransaction);
        Assert.Equal(2000, result.MaxPerWithdrawalTransaction);
        _repositoryMock.Verify(r => r.FetchMultipleAsync<DailyLimitDTO, int>(It.Is<GetDailyLimitCriteria>(c => 
            c.CustId == custId && c.ModeView == ModeViewConstants.BankAccount), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SetTransactionLimit_WhenValidInput_ReturnsSuccess()
    {
        // Arrange
        var custId = 1;
        var limit = new TransactionLimitModel
        {
            DailyWithdrawalCount = 5,
            MinPerDepositTransaction = 10,
            MaxPerDepositTransaction = 1000,
            MinPerWithdrawalTransaction = 20,
            MaxPerWithdrawalTransaction = 2000
        };

        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<SetTransactionLimitCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.SetTransactionLimit(custId, limit);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<SetTransactionLimitCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AddBankAccount_WhenValidAccount_ReturnsSuccess()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            Id = 1,
            BankId = 1,
            AccountHolderName = "Test Account",
            AccountNumber = "123456789"
        };

        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<AddBankAccountCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.AddBankAccount(bankAccount);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<AddBankAccountCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetBankAccountById_WhenValidId_ReturnsAccount()
    {
        // Arrange
        var accountId = 1;
        var expectedAccount = new BankAccountModel
        {
            Id = accountId,
            BankId = 1,
            AccountHolderName = "Test Account",
            AccountNumber = "123456789"
        };

        _repositoryMock.Setup(r => r.FetchAsync<BankAccountModel>(It.IsAny<GetBankAccountByIdCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(new List<BankAccountModel> { expectedAccount });

        // Act
        var result = await _service.GetBankAccountById(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accountId, result.Id);
        _repositoryMock.Verify(r => r.FetchAsync<BankAccountModel>(It.Is<GetBankAccountByIdCriteria>(c => 
            c.BankAccountId == accountId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateBankAccount_WhenValidAccount_ReturnsSuccess()
    {
        // Arrange
        var bankAccount = new BankAccountModel
        {
            Id = 1,
            BankId = 1,
            AccountHolderName = "Test Account",
            AccountNumber = "987654321"
        };

        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<UpdateBankAccountCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        var result = await _service.UpdateBankAccount(bankAccount);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<UpdateBankAccountCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }
} 