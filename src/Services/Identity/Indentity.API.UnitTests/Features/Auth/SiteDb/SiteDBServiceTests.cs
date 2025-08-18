namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.SiteDb;

public class SiteDBServiceTests
{
    private readonly Mock<IDynamicRepository> _mockDynamicRepository;
    private readonly SiteDBService _siteDBService;

    public SiteDBServiceTests()
    {
        _mockDynamicRepository = new Mock<IDynamicRepository>();
        _siteDBService = new SiteDBService(_mockDynamicRepository.Object);
    }

    [Fact]
    public async Task GetSiteDBConfig_WithValidCustId_ShouldReturnSiteDBConfig()
    {
        // Arrange
        var custId = 123;
        var expectedConfig = new SiteDBConfig
        {
            SiteDBGroupId = 1,
            LinkedServerName = "TestServer",
            ClientAccessName = "TestClient",
            AGIP = "TestAGIP",
            CustDBGroupId = 2
        };


        _mockDynamicRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _siteDBService.GetSiteDBConfig(custId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedConfig);

        _mockDynamicRepository.Verify(
            x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default),
            Times.Once);
    }

    [Fact]
    public async Task GetSiteDBConfig_WhenRepositoryReturnsNull_ShouldReturnNull()
    {
        // Arrange
        var custId = 789;
        _mockDynamicRepository
           .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default))
           .ReturnsAsync((SiteDBConfig)null);

        // Act
        var result = await _siteDBService.GetSiteDBConfig(custId);

        // Assert
        result.Should().BeNull();

        _mockDynamicRepository.Verify(
            x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default),
            Times.Once);
    }

    [Fact]
    public async Task GetSiteDBConfig_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var custId = 999;
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDynamicRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _siteDBService.GetSiteDBConfig(custId));

        exception.Should().Be(expectedException);
        exception.Message.Should().Be("Database connection failed");

        _mockDynamicRepository.Verify(
            x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default),
            Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(9999)]
    public async Task GetSiteDBConfig_WithVariousValidCustIds_ShouldWorkCorrectly(int custId)
    {
        // Arrange
        var expectedConfig = new SiteDBConfig
        {
            SiteDBGroupId = custId * 2,
            LinkedServerName = $"Server{custId}",
            ClientAccessName = $"Client{custId}",
            AGIP = $"AGIP{custId}",
            CustDBGroupId = custId * 3
        };

        _mockDynamicRepository
            .Setup(x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default))
            .ReturnsAsync(expectedConfig);

        // Act
        var result = await _siteDBService.GetSiteDBConfig(custId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedConfig);

        _mockDynamicRepository.Verify(
            x => x.GetAsync<SiteDBConfig>(It.IsAny<GetSiteDBConfigCriteria>(), "", default),
            Times.Once);
    }

    [Fact]
    public void SiteDBService_Constructor_ShouldAcceptDynamicRepository()
    {
        // Arrange & Act
        var service = new SiteDBService(_mockDynamicRepository.Object);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeOfType<SiteDBService>();
        service.Should().BeAssignableTo<ISiteDBService>();
    }

    [Fact]
    public void SiteDBService_Constructor_ShouldNotThrowWithNullRepository()
    {
        // Arrange & Act & Assert
        var action = () => new SiteDBService(null);
        action.Should().NotThrow();
    }
}
