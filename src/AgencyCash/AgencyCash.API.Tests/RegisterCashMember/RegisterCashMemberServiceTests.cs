using AgencyCash.API.RegisterCashMember;
using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Requests;
using Fanex.Data.Repository;
using Moq;

namespace AgencyCash.API.Tests.RegisterCashMember
{
    public class RegisterCashMemberServiceTests
    {
        [Fact]
        public async Task RegisterCashMember_Success_ReturnsSuccessResult()
        {
            // Arrange
            var mockRepo = new Mock<IDynamicRepository>();
            var service = new RegisterCashMemberService(mockRepo.Object);
            var request = new RegisterCashMemberRequest
            {
                AgentId = 1,
                UserName = "user",
                Password = "password",
                FirstName = "first",
                LastName = "last",
                Mobile = "123456789",
                Domain = "domain.com",
                Ip = "127.0.0.1"
            };

            // Act
            var result = await service.RegisterCashMember(request);

            // Assert
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);
        }

        // [Fact]
        // public async Task RegisterCashMember_RegisterCustomerFails_ReturnsError()
        // {
        //    // Arrange
        //    var mockRepo = new Mock<IDynamicRepository>();
        //    var service = new RegisterCashMemberService(mockRepo.Object);
        //    var command = new RegisterCashMemberCommand(1, "user", "first", "last", "123456789");

        //    // Act
        //    var result = await service.RegisterCashMember(command);

        //    // Assert
        //    Assert.Equal(1, result.Code);
        //    Assert.Equal("Error", result.Message);
        // }

        [Fact]
        public async Task CheckUsernameAvailable_ReturnsAvailableTrue_WhenUsernameIsNotTaken()
        {
            // Arrange
            var service = new RegisterCashMemberService(new Mock<IDynamicRepository>().Object);
            var criteria = new CheckUsernameCommand("uniqueuser");

            // Act
            var result = await service.CheckUsernameAvailable(criteria);

            // Assert
            Assert.True(result.Available);
        }

        // [Fact]
        // public async Task CheckUsernameAvailable_ReturnsAvailableFalse_WhenUsernameIsTaken()
        // {
        //     // Arrange
        //     var service = new RegisterCashMemberService(new Mock<IDynamicRepository>().Object);
        //     var criteria = new CheckUsernameCommand("taken");

        //     // Act
        //     var result = await service.CheckUsernameAvailable(criteria);

        //     // Assert
        //     Assert.False(result.Available);
        // }
    }
}
