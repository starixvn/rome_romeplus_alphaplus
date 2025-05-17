using Xunit;
using Moq;
using AgencyCash.API.RegisterCashMember;
using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Responses;
using Fanex.Data.Repository;
using System.Threading.Tasks;

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
            var command = new RegisterCashMemberCommand(1, "user", "first", "last", "123456789");
            
            // Act
            var result = await service.RegisterCashMember(command);

            // Assert
            Assert.Equal(0, result.ErrCode);
            Assert.Equal("Success", result.ErrMsg);
        }

        //[Fact]
        //public async Task RegisterCashMember_RegisterCustomerFails_ReturnsError()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IDynamicRepository>();
        //    var service = new RegisterCashMemberService(mockRepo.Object);
        //    var command = new RegisterCashMemberCommand(1, "user", "first", "last", "123456789");
            
        //    // Act
        //    var result = await service.RegisterCashMember(command);

        //    // Assert
        //    Assert.Equal(1, result.ErrCode);
        //    Assert.Equal("Error", result.ErrMsg);
        //}
    }
}
