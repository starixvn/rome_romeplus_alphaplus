using AlphaPlus.PartialUM.Models;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class PartialUMDataAccessTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly PartialUMDataAccess dataAccess;

    public PartialUMDataAccessTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        dataAccess = new PartialUMDataAccess(repositoryMock.Object);
    }

    [Fact]
    public async Task GetUMEntities_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        const string siteName = "TestSite";
        var expectedEntities = new List<UMEntity>
            {
                new UMEntity { FunctionTypeName = "Func1", BeginTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                new UMEntity { FunctionTypeName = "Func2", BeginTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) }
            };

        repositoryMock
            .Setup(r => r.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await dataAccess.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntities.Count, result.Count());
        Assert.Equal(expectedEntities, result);

        repositoryMock.Verify(
            x => x.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetUMEntities_ShouldReturnEmptyList_WhenRepositoryReturnsNull()
    {
        // Arrange
        const string siteName = "TestSite";
        IEnumerable<UMEntity> nullEntities = null;

        repositoryMock
            .Setup(r => r.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty))
            .ReturnsAsync(nullEntities);

        // Act
        var result = await dataAccess.GetUMEntities(siteName);

        // Assert
        Assert.Null(result);

        repositoryMock.Verify(
            x => x.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetUMEntities_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        const string siteName = "TestSite";
        var expectedException = new Exception("Repository error");

        repositoryMock
            .Setup(r => r.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await dataAccess.GetUMEntities(siteName));

        Assert.Same(expectedException, exception);

        repositoryMock.Verify(
            x => x.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty),
            Times.Once);
    }
}
