using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.DTOs;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Data.Repository;
using Fanex.Logging;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CommonServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ILogger> _mockLogger;
    private readonly ICommonService _commonService;

    public CommonServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockLogger = new Mock<ILogger>();
        _commonService = new CommonService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCountryCodes_WhenValidSiteName_ReturnsCountryCodes()
    {
        // Arrange
        var siteName = "TestSite";
        var expectedCountryCodes = new List<string> { "US", "UK", "CA" };
        _mockRepository.Setup(x => x.FetchAsync<string>(It.IsAny<GetCountryCodesCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedCountryCodes);

        // Act
        var result = await _commonService.GetCountryCodes(siteName);

        // Assert
        Assert.Equal(expectedCountryCodes, result);
        _mockRepository.Verify(x => x.FetchAsync<string>(It.Is<GetCountryCodesCriteria>(c => c.SiteName == siteName), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetServerDBConfigByCustIdAsync_WhenValidCustId_ReturnsSiteDbConfig()
    {
        // Arrange
        var custId = 123;
        var expectedConfig = new SiteDbConfig();
        _mockRepository.Setup(x => x.GetAsync<SiteDbConfig>(It.IsAny<GetServerDBConfigByCustIdCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _commonService.GetServerDBConfigByCustIdAsync(custId);

        // Assert
        Assert.Equal(expectedConfig, result);
        _mockRepository.Verify(x => x.GetAsync<SiteDbConfig>(It.Is<GetServerDBConfigByCustIdCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task WriteLogUserRunSP_WhenErrCodeIsNotZero_DoesNotWriteLog()
    {
        // Arrange
        var actionId = 1;
        var request = new BaseActionRequest();
        var command = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US")
        {
            ErrCode = 1
        };

        // Act
        await _commonService.WriteLogUserRunSP(actionId, request, command);

        // Assert
        _mockRepository.Verify(x => x.ExecuteAsync(It.IsAny<WriteLogUserRunSPCriteria>(), string.Empty, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task WriteLogUserRunSP_WhenFirstAttemptSucceeds_DoesNotTrySecondAttempt()
    {
        // Arrange
        var actionId = 1;
        var request = new BaseActionRequest();
        var command = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US")
        {
            ErrCode = 0
        };
        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<WriteLogUserRunSPCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(0);

        // Act
        await _commonService.WriteLogUserRunSP(actionId, request, command);

        // Assert
        _mockRepository.Verify(x => x.ExecuteAsync(It.Is<WriteLogUserRunSPCriteria>(c => c.SiteDB == 0), string.Empty, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task WriteLogUserRunSP_WhenFirstAttemptFails_TriesSecondAttempt()
    {
        // Arrange
        var actionId = 1;
        var request = new BaseActionRequest();
        var command = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US")
        {
            ErrCode = 0
        };
        _mockRepository.Setup(x => x.ExecuteAsync(It.IsAny<WriteLogUserRunSPCriteria>(), string.Empty, CancellationToken.None))
            .ThrowsAsync(new Exception("First attempt fails"));

        // Act
        await _commonService.WriteLogUserRunSP(actionId, request, command);

        // Assert
        _mockRepository.Verify(x => x.ExecuteAsync(It.Is<WriteLogUserRunSPCriteria>(c => c.SiteDB == 0), string.Empty, CancellationToken.None), Times.Once);
    }
} 