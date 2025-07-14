using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Payments;

public class TransactionServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ISiteDBService> _siteDBServiceMock;
    private readonly Mock<Fanex.Logging.ILogger> _loggerMock;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _siteDBServiceMock = new Mock<ISiteDBService>();
        _loggerMock = new Mock<Fanex.Logging.ILogger>();
        _transactionService = new TransactionService(_repositoryMock.Object, _siteDBServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateTransProviderInfo_ShouldReturnResultWithCorrectErrorCode_WhenExecuted()
    {
        // Arrange
        const long transId = 123;
        var request = new UpdateTransProviderInfoRequest
        {
            AgentId = 456,
            CustId = 789,
            TransType = 1,
            ProviderTransId = "PT123",
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        var logs = new List<LogEntry> { new LogEntry() };
        const int expectedErrorCode = 0;
        const string expectedErrorMessage = "Success";

        _siteDBServiceMock.Setup(x => x.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _repositoryMock.Setup(x => x.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as UpdateTransProviderInfoCriteria;
                c.ErrCode = expectedErrorCode;
                c.ErrMsg = expectedErrorMessage;
                c.ErrExtraInfo = string.Empty;
            });

        // Act
        var result = await _transactionService.UpdateTransProviderInfo(transId, request);

        // Assert
        Assert.Equal(expectedErrorCode, result.Code);
        Assert.Equal(expectedErrorMessage, result.Message);
        _repositoryMock.Verify(x => x.FetchAsync<LogEntry>(
            It.Is<UpdateTransProviderInfoCriteria>(c =>
                c.AgentId == request.AgentId &&
                c.CustId == request.CustId &&
                c.TransId == transId &&
                c.TransType == request.TransType &&
                c.ProviderTransId == request.ProviderTransId &&
                c.IP == request.IP),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateTransProviderInfo_ShouldLogError_WhenErrExtraInfoIsNotEmpty()
    {
        // Arrange
        const long transId = 123;
        var request = new UpdateTransProviderInfoRequest
        {
            AgentId = 456,
            CustId = 789,
            TransType = 1,
            ProviderTransId = "PT123",
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        var logs = new List<LogEntry> { new LogEntry() };
        const string errorExtraInfo = "Some extra error info";

        _siteDBServiceMock.Setup(x => x.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _repositoryMock.Setup(x => x.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as UpdateTransProviderInfoCriteria;
                c.ErrExtraInfo = errorExtraInfo;
            });

        // Act
        await _transactionService.UpdateTransProviderInfo(transId, request);

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(s => s.Contains(nameof(TransactionService.UpdateTransProviderInfo)))), Times.Once);
    }

    [Fact]
    public async Task UpdateTransProviderInfo_ShouldNotLogError_WhenErrExtraInfoIsEmpty()
    {
        // Arrange
        const long transId = 123;
        var request = new UpdateTransProviderInfoRequest
        {
            AgentId = 456,
            CustId = 789,
            TransType = 2,
            ProviderTransId = "PT123",
            IP = "192.168.1.1"
        };

        var siteDBConfig = new SiteDBConfig { SiteDBGroupId = 1, CustDBGroupId = 2 };
        var logs = new List<LogEntry> { new LogEntry() };

        _siteDBServiceMock.Setup(x => x.GetSiteDBConfig(request.AgentId))
            .ReturnsAsync(siteDBConfig);

        _repositoryMock.Setup(x => x.FetchAsync<LogEntry>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(logs)
            .Callback<ICriteria, string, CancellationToken>((criteria, s, t) =>
            {
                var c = criteria as UpdateTransProviderInfoCriteria;
                c.ErrExtraInfo = string.Empty;
            });

        // Act
        await _transactionService.UpdateTransProviderInfo(transId, request);

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Never);
    }
}