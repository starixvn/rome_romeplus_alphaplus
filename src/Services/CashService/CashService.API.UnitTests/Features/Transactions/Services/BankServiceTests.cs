using AlphaPlus.CashService.API.Features.Shared.DTOs;
using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace CashService.API.UnitTests.Features.Transactions.Services;

public class BankServiceTests
{
    private readonly Mock<IDynamicRepository> mockDynamicRepository;
    private readonly IBankService bankService;

    public BankServiceTests()
    {
        mockDynamicRepository = new Mock<IDynamicRepository>();
        bankService = new BankService(mockDynamicRepository.Object);
    }

    [Fact]
    public async Task GetCashAgentBankInfoAsync_ShouldReturnBankInfoList()
    {
        // Arrange
        var agentId = 1;
        var transactionType = 1;
        var custDbGroupId = 1;
        var expectedBankInfo = new List<BankInfo> { new() { BankAccountNumber = "Test Bank" } };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<BankInfo>(It.IsAny<GetCashAgentBankInfoCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBankInfo);

        // Act
        var result = await bankService.GetCashAgentBankInfoAsync(agentId, transactionType, custDbGroupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBankInfo.Count, result.Count());
        Assert.Equal(expectedBankInfo.First().BankAccountNumber, result.First().BankAccountNumber);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<BankInfo>(
            It.Is<GetCashAgentBankInfoCriteria>(c =>
                c.AgentId == agentId &&
                c.TransType == transactionType &&
                c.CustDbGroupId == custDbGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMemberBankInfoAsync_ShouldReturnBankInfoList()
    {
        // Arrange
        var custId = 1;
        var custDbGroupId = 1;
        var expectedBankInfo = new List<BankInfo> { new() { BankAccountNumber = "Test Member" } };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<BankInfo>(It.IsAny<GetCashMemberBankInfoCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBankInfo);

        // Act
        var result = await bankService.GetMemberBankInfoAsync(custId, custDbGroupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBankInfo.Count, result.Count());
        Assert.Equal(expectedBankInfo.First().BankAccountNumber, result.First().BankAccountNumber);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<BankInfo>(
            It.Is<GetCashMemberBankInfoCriteria>(c =>
                c.CustId == custId &&
                c.CustDbGroupId == custDbGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCashAgentProviderAsync_ShouldReturnProviderList()
    {
        // Arrange
        var agentId = 1;
        var transactionType = 1;
        var custDbGroupId = 1;
        var expectedProviders = new List<PaymentProvider> { new() { ProviderName = "Test Provider" } };

        mockDynamicRepository
            .Setup(repo => repo.FetchAsync<PaymentProvider>(It.IsAny<GetCashAgentProviderCriteria>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProviders);

        // Act
        var result = await bankService.GetCashAgentProviderAsync(agentId, transactionType, custDbGroupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProviders.Count, result.Count());
        Assert.Equal(expectedProviders.First().ProviderName, result.First().ProviderName);
        mockDynamicRepository.Verify(repo => repo.FetchAsync<PaymentProvider>(
            It.Is<GetCashAgentProviderCriteria>(c =>
                c.AgentId == agentId &&
                c.TransType == transactionType &&
                c.CustDbGroupId == custDbGroupId),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}