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
            Assert.Equal(0, result.ErrCode);
            Assert.Equal(string.Empty, result.ErrMsg);
        }

        [Fact]
        public void Can_Set_Properties()
        {
            var result = new RegisterCashMemberResult { ErrCode = 2, ErrMsg = "error" };
            Assert.Equal(2, result.ErrCode);
            Assert.Equal("error", result.ErrMsg);
        }
    }
}
