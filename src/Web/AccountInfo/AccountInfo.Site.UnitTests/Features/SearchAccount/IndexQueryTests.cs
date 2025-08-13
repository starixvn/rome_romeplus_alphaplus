using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.SearchAccount;

public class IndexQueryTests
{
    private readonly Fixture _fixture;

    public IndexQueryTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void IndexQuery_ShouldImplementIRequestOfIndexViewModel()
    {
        // Arrange & Act
        var queryType = typeof(IndexQuery);

        // Assert
        Assert.True(typeof(IRequest<IndexViewModel>).IsAssignableFrom(queryType));
    }

    [Fact]
    public void IndexQuery_ShouldInheritFromQueryBase()
    {
        // Arrange & Act
        var queryType = typeof(IndexQuery);

        // Assert
        Assert.True(typeof(QueryBase).IsAssignableFrom(queryType));
    }

    [Fact]
    public void IndexQuery_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var queryType = typeof(IndexQuery);

        // Assert
        Assert.NotNull(queryType.GetProperty("MobileView"));
        Assert.NotNull(queryType.GetProperty("ViewMemberInfo"));
    }


}
