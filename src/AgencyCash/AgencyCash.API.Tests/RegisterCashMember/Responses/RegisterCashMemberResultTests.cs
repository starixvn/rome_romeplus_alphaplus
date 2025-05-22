using Xunit;
using AgencyCash.API.RegisterCashMember.Responses;

namespace AgencyCash.API.Tests.RegisterCashMember.Responses
{
    public class RegisterCashMemberResultTests
    {
        [Fact]
        public void Default_Values_AreSet()
        {
            var result = new RegisterCashMemberResult();
            Assert.Equal(0, result.Code);
            Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public void Can_Set_Properties()
        {
            var result = new RegisterCashMemberResult { Code = 2, Message = "error" };
            Assert.Equal(2, result.Code);
            Assert.Equal("error", result.Message);
        }
    }
}
