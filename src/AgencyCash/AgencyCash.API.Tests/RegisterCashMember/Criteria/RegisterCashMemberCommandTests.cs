using Xunit;
using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria
{
    public class RegisterCashMemberCommandTests
    {
        [Fact]
        public void IsValid_AllFieldsFilled_ReturnsTrue()
        {
            var cmd = new RegisterCashMemberCommand(1, "user", "first", "last", "123456789");
            Assert.True(cmd.IsValid());
        }

        [Fact]
        public void IsValid_MissingFields_ReturnsFalse()
        {
            var cmd = new RegisterCashMemberCommand(1, "", "", "", "");
            Assert.False(cmd.IsValid());
        }

        [Fact]
        public void GetSettingKey_ReturnsRegisterCashMember()
        {
            var cmd = new RegisterCashMemberCommand(1, "user", "first", "last", "123456789");
            Assert.Equal("RegisterCashMember", cmd.GetSettingKey());
        }
    }
}
