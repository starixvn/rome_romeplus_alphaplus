using AlphaPlus.Reports.Features.Outstanding;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class AutoFollowBetServiceTest
{
    private readonly Mock<IDynamicRepository> mockRepository;
    private readonly IAutoFollowBetService service;

    public AutoFollowBetServiceTest()
    {
        mockRepository = new Mock<IDynamicRepository>();
        service = new AutoFollowBetService(mockRepository.Object);
    }

    [Fact]
    public async Task GetFollowBetAccounts_ShouldReturnGroupedAccounts()
    {
        // Arrange
        int agentId = 123;
        var followBetAccounts = new List<FollowBetAccount>
        {
            new()
            {
                CustId = 1001,
                Username = "player123",
                AgentId = 5001,
                DummyCustId = 2001,
                DummyUsername = "dummy_user1"
            },
            new()
            {
                CustId = 1003,
                Username = "betmaster789",
                AgentId = 5001,
                DummyCustId = 2003,
                DummyUsername = "dummy_user3"
            },
            new()
            {
                CustId = 1002,
                Username = "gambler456",
                AgentId = 5002,
                DummyCustId = 2002,
                DummyUsername = "dummy_user2"
            }
        };

        mockRepository
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(followBetAccounts);

        // Act
        var result = await service.GetFollowBetAccounts(agentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        var group1 = result.First();
        Assert.Equal(5001, group1.Item1);
        Assert.Equal(2, group1.Item2.Count());

        var group2 = result.Last();
        Assert.Equal(5002, group2.Item1);
        Assert.Single(group2.Item2);
    }

    [Fact]
    public async Task GetFollowBetAccounts_ShouldPassCorrectCriteriaToRepository()
    {
        // Arrange
        int agentId = 123;

        mockRepository
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(new List<FollowBetAccount>());

        // Act
        var result = await service.GetFollowBetAccounts(agentId);

        // Assert
        mockRepository.Verify(r => r.FetchAsync<FollowBetAccount>(It.Is<GetFollowBetAccountsCriteria>(c => c.DummyAgentId == agentId), string.Empty), Times.Once);
        Assert.NotNull(result);
        // Note: This assertion assumes GetFollowBetAccountsCriteria has a property or way to check the agentId
        // You may need to modify this based on actual implementation
    }

    [Fact]
    public async Task GetFollowBetAccounts_WithEmptyResults_ShouldReturnEmptyCollection()
    {
        // Arrange
        int agentId = 123;
        mockRepository
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ReturnsAsync(new List<FollowBetAccount>());

        // Act
        var result = await service.GetFollowBetAccounts(agentId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetFollowBetAccounts_RepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        int agentId = 123;
        mockRepository
            .Setup(r => r.FetchAsync<FollowBetAccount>(It.IsAny<GetFollowBetAccountsCriteria>(), string.Empty))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetFollowBetAccounts(agentId));
    }
}