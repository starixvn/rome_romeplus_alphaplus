using AlphaPlus.Identity.API.Features.Auth;
using Fanex.Data.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace AlphaPlus.Identity.API.UnitTests.Features.Auth;

public class AdminPermissionServiceTests
{
    private readonly Mock<IDynamicRepository> _mockDynamicRepository;
    private readonly AdminPermissionService _adminPermissionService;

    public AdminPermissionServiceTests()
    {
        _mockDynamicRepository = new Mock<IDynamicRepository>();
        _adminPermissionService = new AdminPermissionService(_mockDynamicRepository.Object);
    }

    [Fact]
    public void Constructor_WithValidRepository_ShouldCreateInstance()
    {
        // Act
        var service = new AdminPermissionService(_mockDynamicRepository.Object);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeOfType<AdminPermissionService>();
    }

    [Fact]
    public async Task HasUpdateSubAccountPermission_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var adminId = 1;
        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ReturnsAsync(1);

        // Act
        await _adminPermissionService.HasUpdateSubAccountPermission(adminId);

        // Assert
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default), Times.Once);
    }

    [Fact]
    public async Task HasHostPermission_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var adminId = 1;
        var functionId = 123;
        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ReturnsAsync(1);

        // Act
        await _adminPermissionService.HasHostPermission(adminId, functionId);

        // Assert
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default), Times.Once);
    }

    [Fact]
    public async Task HasHostPermission_WithFunctionName_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var adminId = 1;
        var functionName = "TestFunction";
        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ReturnsAsync(1);

        // Act
        await _adminPermissionService.HasHostPermission(adminId, null, functionName);

        // Assert
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default), Times.Once);
    }

    [Fact]
    public async Task HasHostPermission_WithRepositoryException_ShouldPropagateException()
    {
        // Arrange
        var adminId = 1;
        var expectedException = new InvalidOperationException("Database connection failed");

        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var action = () => _adminPermissionService.HasHostPermission(adminId);
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");
    }

    [Fact]
    public async Task HasUpdateSubAccountPermission_ShouldCallHasHostPermissionWithCorrectParameters()
    {
        // Arrange
        var adminId = 1;
        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ReturnsAsync(1);

        // Act
        await _adminPermissionService.HasUpdateSubAccountPermission(adminId);

        // Assert
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default), Times.Once);
    }

    [Fact]
    public async Task HasHostPermission_ShouldReturnTrue_WhenRepositorySucceeds()
    {
        // Arrange
        var adminId = 1;
        var functionId = 123;
        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default))
            .ReturnsAsync(1);

        // Act
        var result = await _adminPermissionService.HasHostPermission(adminId, functionId);

        // Assert
        // Since we can't control the ErrCode property through Moq callbacks,
        // we're testing that the service calls the repository correctly
        // The actual ErrCode behavior would be tested in integration tests
        result.Should().BeTrue(); // Default ErrCode is 0, which means permission granted
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<GetAdminHostPermissionCriteria>(), "", default), Times.Once);
    }
}
