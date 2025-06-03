using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.PartialUM.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Shared;

[Trait("Category", "BetsForecast")]
public class GetUMInfoListCriteriaTest
{
    [Fact]
    public void GetUMInfoListCriteria_ShouldReturnCorrectSettingKey()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria("TestSite");

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_CheckUM_ByReport", settingKey);
    }

    [Theory]
    [InlineData("TestSite", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    public void GetUMInfoListCriteria_IsValid_ShouldReturnCorrectResult(string siteName, bool expectedResult)
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria(siteName);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetUMInfoListCriteria_ShouldStoreSiteName()
    {
        // Arrange
        const string siteName = "TestSite";

        // Act
        var criteria = new GetUMInfoListCriteria(siteName);

        // Assert
        Assert.Equal(siteName, criteria.SiteName);
    }
}

[Trait("Category", "BetsForecast")]
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
            .Setup(r => r.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty, default))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await dataAccess.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntities.Count, result.Count());
        repositoryMock.Verify(r => r.FetchAsync<UMEntity>(It.Is<GetUMInfoListCriteria>(c => c.SiteName == siteName), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task GetUMEntities_ShouldReturnCorrectData()
    {
        // Arrange
        const string siteName = "TestSite";

        repositoryMock
            .Setup(r => r.FetchAsync<UMEntity>(It.IsAny<GetUMInfoListCriteria>(), string.Empty, default))
            .ReturnsAsync(Enumerable.Empty<UMEntity>());

        // Act
        var result = await dataAccess.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUMEntities_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        const string siteName = "TestSite";
        var expectedException = new Exception("Test exception");

        repositoryMock
            .Setup(r => r.FetchAsync<UMEntity>(It.IsAny<GetUMInfoListCriteria>(), string.Empty, default))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<Exception>(() => dataAccess.GetUMEntities(siteName));
        Assert.Same(expectedException, actualException);
    }
}