using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.UnitTests.Shared.Services;

[Trait("Category", "BetLists")]
public class AutoFollowBetServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly AutoFollowBetService service;

    public AutoFollowBetServiceTests()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        service = new AutoFollowBetService(repositoryMock.Object);
    }

    [Fact]
    public async Task GetRealCustId_WhenFollowBetAccountExists_ShouldReturnRealCustId()
    {
        // Arrange
        const int agentId = 123;
        const int dummyCustId = 456;
        const int expectedRealCustId = 789;

        var followBetAccounts = new List<FollowBetAccount>
        {
            new FollowBetAccount
            {
                AgentId = agentId,
                CustId = expectedRealCustId,
                DummyCustId = dummyCustId,
                Username = "realUser",
                DummyUsername = "dummyUser"
            },
            new FollowBetAccount
            {
                AgentId = agentId,
                CustId = 999,
                DummyCustId = 888,
                Username = "anotherUser",
                DummyUsername = "anotherDummy"
            }
        };

        repositoryMock
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(followBetAccounts);

        // Act
        var result = await service.GetRealCustId(agentId, dummyCustId);

        // Assert
        Assert.Equal(expectedRealCustId, result);
        repositoryMock.Verify(r => r.FetchAsync<FollowBetAccount>(
            It.Is<GetFollowBetAccountsCriteria>(c => c.DummyAgentId == agentId),
            string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetRealCustId_WhenFollowBetAccountDoesNotExist_ShouldReturnZero()
    {
        // Arrange
        const int agentId = 123;
        const int dummyCustId = 456;

        var followBetAccounts = new List<FollowBetAccount>
        {
            new FollowBetAccount
            {
                AgentId = agentId,
                CustId = 789,
                DummyCustId = 999,
                Username = "realUser",
                DummyUsername = "dummyUser"
            }
        };

        repositoryMock
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(followBetAccounts);

        // Act
        var result = await service.GetRealCustId(agentId, dummyCustId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetRealCustId_WhenEmptyResults_ShouldReturnZero()
    {
        // Arrange
        const int agentId = 123;
        const int dummyCustId = 456;

        repositoryMock
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(new List<FollowBetAccount>());

        // Act
        var result = await service.GetRealCustId(agentId, dummyCustId);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetRealCustId_RepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        const int agentId = 123;
        const int dummyCustId = 456;

        var expectedException = new Exception("Database error");

        repositoryMock
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => service.GetRealCustId(agentId, dummyCustId));
        Assert.Same(expectedException, exception);
    }
}