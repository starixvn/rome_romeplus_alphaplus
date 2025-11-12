using AlphaPlus.Settings.MemberInfo.API.Features.CreateAccount;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Features.CreateAccount;

public class ProductSettingInfoServiceTests
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly ProductSettingInfoService _service;
    private readonly Fixture _fixture;

    public ProductSettingInfoServiceTests()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _service = new ProductSettingInfoService(_mockRepository.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void Can_Create_ProductSettingInfoService_Instance()
    {
        // Arrange & Act
        var service = new ProductSettingInfoService(_mockRepository.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void ProductSettingInfoService_Should_Implement_IProductSettingInfoService()
    {
        // Arrange & Act
        var service = new ProductSettingInfoService(_mockRepository.Object);

        // Assert
        Assert.IsAssignableFrom<IProductSettingInfoService>(service);
    }

    [Fact]
    public async Task GetProductsSettingInfo_Should_Return_NotNull()
    {
        // Arrange
        var cusId = _fixture.Create<int>();

        // Act
        var result = await _service.GetProductsSettingInfo(cusId);

        // Assert
        Assert.NotNull(result);
    }
}
