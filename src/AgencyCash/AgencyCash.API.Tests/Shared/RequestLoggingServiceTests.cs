using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Criteria;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.Shared;

public class RequestLoggingServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ISiteDBService> _siteDBServiceMock;
    private readonly Mock<Fanex.Logging.ILogger> _loggerMock;
    private readonly RequestLoggingService _service;
    private readonly AddRequestLoggingCriteria _criteria;

    public RequestLoggingServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _siteDBServiceMock = new Mock<ISiteDBService>();
        _loggerMock = new Mock<Fanex.Logging.ILogger>();
        _service = new RequestLoggingService(
            _repositoryMock.Object,
            _siteDBServiceMock.Object,
            _loggerMock.Object);

        _criteria = new AddRequestLoggingCriteria(
            456,
            789,
            "TestFunctionName",
            "TestRequestUrl",
            "TestRequestBody");
    }

    [Fact]
    public async Task WriteLog_SuccessfulFirstAttempt_LogIsWritten()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(_criteria, string.Empty, default))
            .ReturnsAsync(1);

        // Act
        await _service.WriteLog(_criteria);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(_criteria, string.Empty, default), Times.Once);
        _siteDBServiceMock.Verify(s => s.GetCustDBGroupId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task WriteLog_FailedFirstAttemptWithNonZeroUserId_RetryWithCustDBGroupId()
    {
        // Arrange
        const short custDBGroupId = 5;
        _repositoryMock.SetupSequence(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .ThrowsAsync(new Exception("Test exception"))
            .ReturnsAsync(1);

        _siteDBServiceMock.Setup(s => s.GetCustDBGroupId(_criteria.UserId))
            .ReturnsAsync(custDBGroupId);

        // Act
        await _service.WriteLog(_criteria);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default), Times.Exactly(2));
        _siteDBServiceMock.Verify(s => s.GetCustDBGroupId(_criteria.UserId), Times.Once);
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        Assert.Equal(custDBGroupId, _criteria.CustDBGroupId);
    }

    [Fact]
    public async Task WriteLog_BothAttemptsFailure_LogsErrors()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default))
            .ThrowsAsync(new Exception("Test exception"));

        _siteDBServiceMock.Setup(s => s.GetCustDBGroupId(_criteria.UserId))
            .ReturnsAsync((short)5);

        // Act
        await _service.WriteLog(_criteria);

        // Assert
        _repositoryMock.Verify(r => r.ExecuteAsync(It.IsAny<INonQueryCommand>(), string.Empty, default), Times.Exactly(2));
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(2));
    }
}
