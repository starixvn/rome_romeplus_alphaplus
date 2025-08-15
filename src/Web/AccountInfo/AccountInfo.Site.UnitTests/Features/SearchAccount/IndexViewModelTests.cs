using AlphaPlus.AccountInfo.Site.Features.SearchAccount;

namespace AlphaPlus.AccountInfo.Site.UnitTests.Features.SearchAccount;

public class IndexViewModelTests
{
    private readonly Fixture _fixture;

    public IndexViewModelTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void IndexViewModel_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var viewModelType = typeof(IndexViewModel);

        // Assert
        Assert.NotNull(viewModelType.GetProperty("ActorLevel"));
        Assert.NotNull(viewModelType.GetProperty("MobileView"));
        Assert.NotNull(viewModelType.GetProperty("ActorName"));
        Assert.NotNull(viewModelType.GetProperty("ShowLoginInfo"));
    }

    [Fact]
    public void IndexViewModel_ShouldHaveConstructor()
    {
        // Arrange & Act
        var viewModelType = typeof(IndexViewModel);
        var constructors = viewModelType.GetConstructors();

        // Assert
        Assert.NotEmpty(constructors);
    }

    [Fact]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange & Act
        var viewModelType = typeof(IndexViewModel);
        var actorLevelProperty = viewModelType.GetProperty(nameof(IndexViewModel.ActorLevel));
        var mobileViewProperty = viewModelType.GetProperty(nameof(IndexViewModel.MobileView));
        var actorNameProperty = viewModelType.GetProperty(nameof(IndexViewModel.ActorName));
        var showLoginInfoProperty = viewModelType.GetProperty(nameof(IndexViewModel.ShowLoginInfo));

        // Assert
        Assert.True(actorLevelProperty?.CanWrite == false, "ActorLevel should be read-only");
        Assert.True(mobileViewProperty?.CanWrite == false, "MobileView should be read-only");
        Assert.True(actorNameProperty?.CanWrite == false, "ActorName should be read-only");
        Assert.True(showLoginInfoProperty?.CanWrite == false, "ShowLoginInfo should be read-only");
    }


}
