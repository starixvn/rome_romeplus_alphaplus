using AlphaPlus.Reports.Features.Customer;
using AlphaPlus.Reports.Features.Shared.Constants;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Customer;

[Trait("Category", "Customer")]
public class CustomerServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly ICustomerService customerService;

    public CustomerServiceTests()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        customerService = new CustomerService(repositoryMock.Object);
    }

    [Fact]
    public async Task GetCustomerLevel_ShouldCallRepository_WithCorrectCriteria()
    {
        // Arrange
        const int viewerId = 123;
        const string username = "testUser";
        var expectedCustomerLevel = new CustomerLevel { CustId = 456 };

        repositoryMock
            .Setup(r => r.GetAsync<CustomerLevel>(It.IsAny<GetCustomerLevelCriteria>(), string.Empty))
            .ReturnsAsync(expectedCustomerLevel);

        // Act
        var result = await customerService.GetCustomerLevel(viewerId, username);

        // Assert
        repositoryMock.Verify(r => r.GetAsync<CustomerLevel>(It.Is<GetCustomerLevelCriteria>(
            c => c.UplineId == viewerId && c.Username == username), string.Empty),
            Times.Once);
        Assert.Equal(expectedCustomerLevel, result);
    }

    [Fact]
    public void BuildBreadcrumb_WhenCustomerIsMaster_ShouldReturnSuperBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Master,
            SuperId = 100,
            SuperName = "SuperTest"
        };

        // Act
        var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

        // Assert
        Assert.Equal(customerLevel.SuperId, breadcrumb.CustId);
        Assert.Equal(customerLevel.SuperName, breadcrumb.CustName);
        Assert.Equal(RoleIdConstants.Super, breadcrumb.CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumb_WhenCustomerIsAgent_ShouldReturnMasterBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Agent,
            MasterId = 200,
            MasterName = "MasterTest"
        };

        // Act
        var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

        // Assert
        Assert.Equal(customerLevel.MasterId, breadcrumb.CustId);
        Assert.Equal(customerLevel.MasterName, breadcrumb.CustName);
        Assert.Equal(RoleIdConstants.Master, breadcrumb.CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumb_WhenCustomerIsMemberWithSuperAsDirectUpline_ShouldReturnSuperBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Member,
            SuperId = 100,
            SuperName = "SuperTest",
            AgentId = DirectMemberAgentIds.Super
        };

        // Act
        var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

        // Assert
        Assert.Equal(customerLevel.SuperId, breadcrumb.CustId);
        Assert.Equal(customerLevel.SuperName, breadcrumb.CustName);
        Assert.Equal(RoleIdConstants.Super, breadcrumb.CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumb_WhenCustomerIsMemberWithMasterAsDirectUpline_ShouldReturnMasterBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Member,
            MasterId = 200,
            MasterName = "MasterTest",
            AgentId = DirectMemberAgentIds.Master
        };

        // Act
        var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

        // Assert
        Assert.Equal(customerLevel.MasterId, breadcrumb.CustId);
        Assert.Equal(customerLevel.MasterName, breadcrumb.CustName);
        Assert.Equal(RoleIdConstants.Master, breadcrumb.CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumb_WhenCustomerIsMemberWithRegularAgent_ShouldReturnAgentBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Member,
            AgentId = 300,
            AgentName = "AgentTest"
        };

        // Act
        var breadcrumb = customerService.BuildBreadcrumb(customerLevel);

        // Assert
        Assert.Equal(customerLevel.AgentId.Value, breadcrumb.CustId);
        Assert.Equal(customerLevel.AgentName, breadcrumb.CustName);
        Assert.Equal(RoleIdConstants.Agent, breadcrumb.CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumbs_WhenViewerIsSuper_AndCustomerIsMember_ShouldReturnFullHierarchy()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Member,
            SuperId = 100,
            SuperName = "SuperTest",
            MasterId = 200,
            MasterName = "MasterTest",
            AgentId = 300,
            AgentName = "AgentTest"
        };

        // Act
        var breadcrumbs = customerService.BuildBreadcrumbs(customerLevel, RoleIdConstants.Super).ToList();

        // Assert
        Assert.Equal(3, breadcrumbs.Count);
        Assert.Equal(customerLevel.SuperId, breadcrumbs[0].CustId);
        Assert.Equal(customerLevel.MasterId, breadcrumbs[1].CustId);
        Assert.Equal(customerLevel.AgentId.Value, breadcrumbs[2].CustId);
    }

    [Fact]
    public void BuildBreadcrumbs_WhenViewerIsMaster_AndCustomerIsMember_ShouldReturnMasterAndAgent()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Member,
            MasterId = 200,
            MasterName = "MasterTest",
            AgentId = 300,
            AgentName = "AgentTest"
        };

        // Act
        var breadcrumbs = customerService.BuildBreadcrumbs(customerLevel, RoleIdConstants.Master).ToList();

        // Assert
        Assert.Equal(2, breadcrumbs.Count);
        Assert.Equal(customerLevel.MasterId, breadcrumbs[0].CustId);
        Assert.Equal(customerLevel.AgentId.Value, breadcrumbs[1].CustId);
    }

    [Fact]
    public void BuildBreadcrumbs_WhenCustomerIsNotMember_ShouldNotIncludeAgentBreadcrumb()
    {
        // Arrange
        var customerLevel = new CustomerLevel
        {
            RoleId = RoleIdConstants.Agent,
            SuperId = 100,
            SuperName = "SuperTest",
            MasterId = 200,
            MasterName = "MasterTest"
        };

        // Act
        var breadcrumbs = customerService.BuildBreadcrumbs(customerLevel, RoleIdConstants.Super).ToList();

        // Assert
        Assert.Equal(2, breadcrumbs.Count);
        Assert.Equal(customerLevel.SuperId, breadcrumbs[0].CustId);
        Assert.Equal(customerLevel.MasterId, breadcrumbs[1].CustId);
    }
}