using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Payments;

[Trait("Category", "Withdrawal")]
public class WithdrawalServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ISiteDBService> _mockSiteDBService;
    private readonly Mock<Fanex.Logging.ILogger> _mockLogger;
    private readonly WithdrawalService _service;
    private readonly SiteDBConfig _siteDBConfig;

    public WithdrawalServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockSiteDBService = new Mock<ISiteDBService>();
        _mockLogger = new Mock<Fanex.Logging.ILogger>();
        _service = new WithdrawalService(_mockRepository.Object, _mockSiteDBService.Object, _mockLogger.Object);

        _siteDBConfig = new SiteDBConfig
        {
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(It.IsAny<int>()))
            .ReturnsAsync(_siteDBConfig);
    }

    [Fact]
    public async Task CreateManualWithdrawal_Success_ReturnsValidResult()
    {
        // Arrange
        var request = new ManualWithdrawalRequest
        {
            AgentId = 1,
            CustId = 100,
            Amount = 500,
            PaymentTo = "Bank ABC",
            IP = "192.168.1.1"
        };

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((criteria, s, t) =>
            {
                if (criteria is ValidatePaymentCriteria)
                {
                    var c = criteria as ValidatePaymentCriteria;
                    c.ErrCode = 0;
                    c.ErrMsg = "Ok";
                }
                else if (criteria is WithdrawalLimitAmountCriteria)
                {
                    var c = criteria as WithdrawalLimitAmountCriteria;
                    c.MinAmt = 100;
                    c.MaxAmt = 1000;
                    c.DailyWithdrawalCnt_Setting = 5;
                }
            })
            .Returns(Task.FromResult(1));

        var logs = new List<LogEntry> { new LogEntry() };
        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<ManualWithdrawalTransactionCriteria>(), string.Empty, default))
            .ReturnsAsync(logs);

        // Act
        var result = await _service.CreateManualWithdrawal(request);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Equal("Ok", result.Message);
    }

    [Fact]
    public async Task CreateManualWithdrawal_ValidationFails_ReturnsErrorResult()
    {
        // Arrange
        var request = new ManualWithdrawalRequest
        {
            AgentId = 1,
            CustId = 100,
            Amount = 500,
            PaymentTo = "Bank ABC",
            IP = "192.168.1.1"
        };

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as ValidatePaymentCriteria;
                c.ErrCode = 400;
                c.ErrMsg = "Validation failed";
            })
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.CreateManualWithdrawal(request);

        // Assert
        Assert.Equal(400, result.Code);
        Assert.Equal("Validation failed", result.Message);
    }

    [Fact]
    public async Task CreateManualWithdrawal_ExceptionThrown_ReturnsErrorResult()
    {
        // Arrange
        var request = new ManualWithdrawalRequest
        {
            AgentId = 1,
            CustId = 100,
            Amount = 500,
            PaymentTo = "Bank ABC",
            IP = "192.168.1.1"
        };

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _service.CreateManualWithdrawal(request);

        // Assert
        Assert.Equal(500, result.Code);
        Assert.Contains("An error occurred", result.Message);
        _mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetWithdrawalLimitAmount_Success_ReturnsLimitDetails()
    {
        // Arrange
        var request = new GetWithdrawalLimitRequest
        {
            AgentId = 1,
            CustId = 100
        };

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((criteria, s, t) =>
            {
                if (criteria is GetMemberCashBalanceCriteria)
                {
                    var c = criteria as GetMemberCashBalanceCriteria;
                    c.TotalCash = 1000;
                }
                else if (criteria is WithdrawalLimitAmountCriteria)
                {
                    var c = criteria as WithdrawalLimitAmountCriteria;
                    c.MinAmt = 100;
                    c.MaxAmt = 5000;
                    c.WithdrawalLimitCnt = 3;
                }
            })
            .Returns(Task.FromResult(1));

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<GetDailyWithdrawalCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.GetWithdrawalLimitAmount(request);

        // Assert
        Assert.Equal(100, result.MinAmount);
        Assert.Equal(5000, result.MaxAmount);
        Assert.Equal(3, result.DailyLimit);
        Assert.Equal(1000, result.MemberCashBalance);
    }

    [Fact]
    public async Task GetWithdrawalLimitAmount_ExceptionThrown_ReturnsEmptyResult()
    {
        // Arrange
        var request = new GetWithdrawalLimitRequest
        {
            AgentId = 1,
            CustId = 100
        };

        _mockRepository.Setup(r => r.ExecuteAsync(It.IsAny<GetMemberCashBalanceCriteria>(), string.Empty, default))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _service.GetWithdrawalLimitAmount(request);

        // Assert
        Assert.Equal(0, result.MinAmount);
        Assert.Equal(0, result.MaxAmount);
        Assert.Equal(0, result.DailyLimit);
        Assert.Equal(0, result.MemberCashBalance);
        _mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTransactionStatus_Should_Return_Success_Result_When_Valid_Request()
    {
        // Arrange
        long transId = 12345;
        var request = new UpdateTransactionStatusRequest
        {
            AgentId = 100,
            CustId = 200,
            StatusId = 1,
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig
        {
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        var logs = new List<LogEntry> { new LogEntry() };

        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as UpdateWithdrawalTransStatusCriteria;
                criteria.ErrCode = 0;
                criteria.ErrMsg = string.Empty;
                criteria.CashAccumCreditTbl = "SomeTable";
            });

        // Act
        var result = await _service.UpdateTransactionStatus(transId, request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Code);
        Assert.Equal(string.Empty, result.Message);
    }

    [Fact]
    public async Task UpdateTransactionStatus_Should_Return_Error_Result_When_Update_Fails()
    {
        // Arrange
        long transId = 12345;
        var request = new UpdateTransactionStatusRequest
        {
            AgentId = 100,
            CustId = 200,
            StatusId = 1,
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig
        {
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        var logs = new List<LogEntry> { new LogEntry() };

        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as UpdateWithdrawalTransStatusCriteria;
                criteria.ErrCode = 1001;
                criteria.ErrMsg = "Failed to update transaction status";
            });

        // Act
        var result = await _service.UpdateTransactionStatus(transId, request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(1001, result.Code);
        Assert.Equal("Failed to update transaction status", result.Message);
    }

    [Fact]
    public async Task UpdateTransactionStatus_Should_Log_Error_When_ErrExtraInfo_Is_Not_Empty()
    {
        // Arrange
        long transId = 12345;
        var request = new UpdateTransactionStatusRequest
        {
            AgentId = 100,
            CustId = 200,
            StatusId = 1,
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig
        {
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        var logs = new List<LogEntry> { new LogEntry() };

        _mockSiteDBService.Setup(s => s.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _mockRepository.Setup(r => r.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((c, s, t) =>
            {
                var criteria = c as UpdateWithdrawalTransStatusCriteria;
                criteria.ErrCode = 0;
                criteria.ErrMsg = string.Empty;
                criteria.ErrExtraInfo = "Extra error information";
            });

        // Act
        var result = await _service.UpdateTransactionStatus(transId, request);

        // Assert
        _mockLogger.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Once);
    }
}
