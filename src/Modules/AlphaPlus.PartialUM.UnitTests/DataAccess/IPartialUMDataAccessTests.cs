using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Models;

namespace AlphaPlus.PartialUM.UnitTests.DataAccess;

public class IPartialUMDataAccessTests
{
    [Fact]
    public async Task GetUMEntities_ShouldReturnIEnumerableOfUMEntity()
    {
        // Arrange
        var mockDataAccess = new Mock<IPartialUMDataAccess>();
        var siteName = "TestSite";
        var expectedEntities = new List<UMEntity>
        {
            new() { FunctionTypeName = "Function1", BeginTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
            new() { FunctionTypeName = "Function2", BeginTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) }
        };

        mockDataAccess.Setup(x => x.GetUMEntities(siteName))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await mockDataAccess.Object.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntities.Count, result.Count());
        Assert.All(result, entity => Assert.IsType<UMEntity>(entity));
    }

    [Fact]
    public async Task GetUMEntities_WithEmptySiteName_ShouldHandleGracefully()
    {
        // Arrange
        var mockDataAccess = new Mock<IPartialUMDataAccess>();
        var siteName = "";
        var expectedEntities = Enumerable.Empty<UMEntity>();

        mockDataAccess.Setup(x => x.GetUMEntities(siteName))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await mockDataAccess.Object.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUMEntities_WithNullSiteName_ShouldHandleGracefully()
    {
        // Arrange
        var mockDataAccess = new Mock<IPartialUMDataAccess>();
        string siteName = null;
        var expectedEntities = Enumerable.Empty<UMEntity>();

        mockDataAccess.Setup(x => x.GetUMEntities(siteName))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await mockDataAccess.Object.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUMEntities_ShouldReturnEmptyCollectionWhenNoData()
    {
        // Arrange
        var mockDataAccess = new Mock<IPartialUMDataAccess>();
        var siteName = "TestSite";
        var expectedEntities = Enumerable.Empty<UMEntity>();

        mockDataAccess.Setup(x => x.GetUMEntities(siteName))
            .ReturnsAsync(expectedEntities);

        // Act
        var result = await mockDataAccess.Object.GetUMEntities(siteName);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUMEntities_ShouldBeCalledWithCorrectSiteName()
    {
        // Arrange
        var mockDataAccess = new Mock<IPartialUMDataAccess>();
        var siteName = "TestSite";
        var expectedEntities = Enumerable.Empty<UMEntity>();

        mockDataAccess.Setup(x => x.GetUMEntities(siteName))
            .ReturnsAsync(expectedEntities);

        // Act
        await mockDataAccess.Object.GetUMEntities(siteName);

        // Assert
        mockDataAccess.Verify(x => x.GetUMEntities(siteName), Times.Once);
    }
}
