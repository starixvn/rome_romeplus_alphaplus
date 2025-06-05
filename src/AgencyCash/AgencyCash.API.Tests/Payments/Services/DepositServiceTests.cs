using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgencyCash.API.Tests.Payments;

[Trait("Category", "Deposit")]
public class DepositServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ISiteDBService> _mockSiteDBService;
    private readonly Mock<Fanex.Logging.ILogger> _mockLogger;
    private readonly IDepositService _depositService;

    public DepositServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockSiteDBService = new Mock<ISiteDBService>();
        _mockLogger = new Mock<Fanex.Logging.ILogger>();
        _depositService = new DepositService(_mockRepository.Object, _mockSiteDBService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetLimit_ShouldReturnDepositLimit()
    {
        // Arrange
        var request = new GetDepositLimitRequest
        {
            AgentId = 123,
            CustId = 456,
            PaymentType = 1,
            ProviderId = 2,
            PaymentMethodId = 3
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, s, t) =>
            {
                if (c is GetAgentCashAccumCreditCriteria)
                {
                    var criteria = c as GetAgentCashAccumCreditCriteria;
                    criteria.AccumCreditReturn = 1000;
                }
                else if (c is GetDepositLimitCriteria)
                {
                    var criteria = c as GetDepositLimitCriteria;
                    criteria.MinAmt = 100;
                    criteria.MaxAmt = 10000;
                    criteria.AgentAvailableCredit = 5000;
                }
            })
            .Returns(Task.FromResult(1));

        // Act
        var result = await _depositService.GetLimit(request);

        // Assert
        Assert.Equal(100, result.MinAmount);
        Assert.Equal(10000, result.MaxAmount);
        Assert.Equal(5000, result.AgentAvailableCredit);
    }

    [Fact]
    public async Task Manual_WhenValidationFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new ManualDepositRequest
        {
            AgentId = 123,
            CustId = 456,
            PaymentMethodId = 1,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 1000,
            IP = "127.0.0.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        var failedResult = new PaymentResult(1, "Error message", 0);
        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as ValidatePaymentCriteria;
                criteria.ErrCode = failedResult.Code;
                criteria.ErrMsg = failedResult.Message;
            })
            .Returns(Task.FromResult(1));

        // Act
        var result = await _depositService.Manual(request);

        // Assert
        Assert.Equal(failedResult.Code, result.Code);
        Assert.Equal(failedResult.Message, result.Message);
    }

    [Fact]
    public async Task Manual_WhenValidationPasses_ShouldProcessDeposit()
    {
        // Arrange
        var request = new ManualDepositRequest
        {
            AgentId = 123,
            CustId = 456,
            PaymentMethodId = 1,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 1000,
            IP = "127.0.0.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<GetDepositLimitCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        var logEntries = new List<LogEntry> { new LogEntry() };
        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<AddTransactionDepositCriteria>(), string.Empty, default))
            .ReturnsAsync(logEntries);

        // Act
        var result = await _depositService.Manual(request);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<LogEntry>(It.IsAny<AddTransactionDepositCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task Auto_WhenValidationFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new AutoDepositRequest
        {
            AgentId = 123,
            CustId = 456,
            DepositMethod = 1,
            ProviderId = 2,
            PaymentMethodId = 3,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 1000,
            IP = "127.0.0.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        var failedResult = new PaymentResult(1, "Error message", 0);
        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as ValidatePaymentCriteria;
                criteria.ErrCode = failedResult.Code;
                criteria.ErrMsg = failedResult.Message;
            })
            .Returns(Task.FromResult(1));

        // Act
        var result = await _depositService.Auto(request);

        // Assert
        Assert.Equal(failedResult.Code, result.Code);
        Assert.Equal(failedResult.Message, result.Message);
    }

    [Fact]
    public async Task Auto_WhenValidationPasses_ShouldProcessDeposit()
    {
        // Arrange
        var request = new AutoDepositRequest
        {
            AgentId = 123,
            CustId = 456,
            DepositMethod = 1,
            ProviderId = 2,
            PaymentMethodId = 3,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 1000,
            IP = "127.0.0.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<GetDepositLimitCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        var logEntries = new List<LogEntry> { new LogEntry() };
        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<AddTransactionDepositCriteria>(), string.Empty, default))
            .ReturnsAsync(logEntries);

        // Act
        var result = await _depositService.Auto(request);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<LogEntry>(It.IsAny<AddTransactionDepositCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task AddTransactionAndWriteLogs_ShouldLogErrorWhenErrExtraInfoExists()
    {
        // Arrange
        var request = new AutoDepositRequest
        {
            AgentId = 123,
            CustId = 456,
            DepositMethod = 1,
            ProviderId = 2,
            PaymentMethodId = 3,
            PaymentFrom = "From",
            PaymentTo = "To",
            Amount = 1000,
            IP = "127.0.0.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId)).ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<GetDepositLimitCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        var logEntries = new List<LogEntry> { new LogEntry() };
        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .Callback<ICriteria, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as AddTransactionDepositCriteria;
                criteria.ErrCode = 0;
                criteria.ErrMsg = null;
                criteria.ErrExtraInfo = "Extra info";
            })
            .ReturnsAsync(logEntries);

        // Act
        var result = await _depositService.Auto(request);

        // Assert
        _mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Once);
    }
}
