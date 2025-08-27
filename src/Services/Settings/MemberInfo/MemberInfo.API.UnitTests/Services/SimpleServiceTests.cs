using AlphaPlus.Settings.MemberInfo.API.Services;

namespace AlphaPlus.Settings.MemberInfo.API.UnitTests.Services;

public class SimpleServiceTests
{
    [Fact]
    public void Can_Create_AccountService_Instance()
    {
        // This test verifies that we can create the service with a mock repository
        // Arrange
        var mockRepository = new Moq.Mock<Fanex.Data.Repository.IDynamicRepository>();

        // Act
        var service = new AccountService(mockRepository.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void AccountService_Should_Implement_IAccountService()
    {
        // Arrange
        var mockRepository = new Moq.Mock<Fanex.Data.Repository.IDynamicRepository>();

        // Act
        var service = new AccountService(mockRepository.Object);

        // Assert
        Assert.IsAssignableFrom<IAccountService>(service);
    }

    [Fact]
    public void Can_Call_GetTransferOption_With_Valid_Inputs()
    {
        // Arrange
        var mockRepository = new Moq.Mock<Fanex.Data.Repository.IDynamicRepository>();
        var service = new AccountService(mockRepository.Object);
        var custId = 123;
        var roleId = 1;

        // Act
        var result = service.GetTransferOption(custId, roleId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
}
