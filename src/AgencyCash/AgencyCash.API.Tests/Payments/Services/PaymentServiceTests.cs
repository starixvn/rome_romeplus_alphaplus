using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Payments.Services;

[Trait("Category", "Payments")]
public class PaymentServiceTests
{
    private class TestPaymentService : PaymentService
    {
        public TestPaymentService(
            IDynamicRepository repository,
            ISiteDBService siteDBService,
            Fanex.Logging.ILogger logger)
            : base(repository, siteDBService, logger)
        {
        }
    }

    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ISiteDBService> _siteDBServiceMock;
    private readonly Mock<Fanex.Logging.ILogger> _loggerMock;
    private readonly TestPaymentService _service;

    public PaymentServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _siteDBServiceMock = new Mock<ISiteDBService>();
        _loggerMock = new Mock<Fanex.Logging.ILogger>();
        _service = new TestPaymentService(_repositoryMock.Object, _siteDBServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ValidatePayment_Should_Return_PaymentResult_With_Error_Code_And_Message()
    {
        // Arrange
        var criteria = new ValidatePaymentCriteria(
            agentId: 123,
            custId: 456,
            transType: 1,
            custDBGroupId: 0)
        {
            ErrCode = 999,
            ErrMsg = "Test Error",
            ErrExtraInfo = "Test Error"
        };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.ValidatePayment(criteria);

        // Assert
        Assert.Equal(999, result.Code);
        Assert.Equal("Test Error", result.Message);
        _repositoryMock.Verify(r => r.ExecuteAsync(criteria, string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task ValidatePayment_Should_Log_Error_When_ErrExtraInfo_Is_Not_Empty()
    {
        // Arrange
        var criteria = new ValidatePaymentCriteria(
            agentId: 123,
            custId: 456,
            transType: 1,
            custDBGroupId: 0)
        {
            ErrCode = 999,
            ErrMsg = "Test Error",
            ErrExtraInfo = "Extra Error Info"
        };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<ValidatePaymentCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        _loggerMock
            .Setup(l => l.ErrorAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.ValidatePayment(criteria);

        // Assert
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task WriteLogsTransferAndRunSP_Should_Call_WriteLogTransfer_For_LogTypeId_1()
    {
        // Arrange
        var logs = new List<LogEntry>
        {
            new LogEntry { LogTypeId = 1, UserId = 1, FromCustId = 100, ToCustId = 200, IP = "127.0.0.1", Doing = "Transfer" }
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 10, CustDBGroupId = 20 };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<WriteLogTransferCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        // Act
        await _service.WriteLogsTransferAndRunSP(logs, siteDBConfig);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<WriteLogTransferCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task WriteLogsTransferAndRunSP_Should_Call_WriteLogRunSP_For_LogTypeId_Not_1()
    {
        // Arrange
        var logs = new List<LogEntry>
        {
            new LogEntry { LogTypeId = 2, UserId = 1, CustId = 100, ActionId = 5, TrackingValueMask = "1000", TrackingValue = "Tracking" }
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 10, CustDBGroupId = 20 };

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<WriteLogRunSPCriteria>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        // Act
        await _service.WriteLogsTransferAndRunSP(logs, siteDBConfig);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<WriteLogRunSPCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task WriteLogRunSP_Should_Try_With_CustDBGroupId_When_First_Attempt_Fails()
    {
        // Arrange
        var log = new LogEntry
        {
            UserId = 1,
            CustId = 100,
            ActionId = 5,
            TrackingValueMask = "1000",
            TrackingValue = "Tracking"
        };

        _repositoryMock
            .SetupSequence(r => r.ExecuteAsync(It.IsAny<WriteLogRunSPCriteria>(), string.Empty, default))
            .Throws(new Exception("DB Error"))
            .Returns(Task.FromResult(1));

        _loggerMock
            .Setup(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.WriteLogRunSP(log, 20);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<WriteLogRunSPCriteria>(), string.Empty, default), Times.Exactly(2));
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task WriteLogTransfer_Should_Try_With_SiteDBGroupId_When_First_Attempt_Fails()
    {
        // Arrange
        var log = new LogEntry
        {
            UserId = 1,
            FromCustId = 100,
            ToCustId = 200,
            IP = "127.0.0.1",
            Doing = "Transfer"
        };

        _repositoryMock
            .SetupSequence(r => r.ExecuteAsync(It.IsAny<WriteLogTransferCriteria>(), string.Empty, default))
            .Throws(new Exception("DB Error"))
            .Returns(Task.FromResult(1));

        _loggerMock
            .Setup(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.WriteLogTransfer(log, 10);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<WriteLogTransferCriteria>(), string.Empty, default), Times.Exactly(2));
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task SyncCashAccumCredit_Should_Return_Result_With_Error_Details()
    {
        // Arrange
        var cashAccumCreditTbl = "CashAccumCredit";
        short custDBGroupId = 20;

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Callback<INonQueryCommand, string, CancellationToken>((c, ip, t) =>
            {
                var criteria = c as SyncCashAccumCreditCriteria;
                criteria.ErrCode = 999;
                criteria.ErrMsg = "Sync Error";
            })
            .Returns(Task.FromResult(1));

        _loggerMock
            .Setup(l => l.ErrorAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.SyncCashAccumCredit(cashAccumCreditTbl, custDBGroupId);

        // Assert
        Assert.Equal(999, result.Code);
        Assert.Equal("Sync Error", result.Message);
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SyncCashAccumCredit_Should_Not_Log_When_Success()
    {
        // Arrange
        var cashAccumCreditTbl = "CashAccumCredit";
        short custDBGroupId = 20;

        _repositoryMock
            .Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.SyncCashAccumCredit(cashAccumCreditTbl, custDBGroupId);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Null(result.Message);
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Never);
    }
}
