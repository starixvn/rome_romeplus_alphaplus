using AgencyCash.API.RegisterCashMember.Criteria;

namespace AgencyCash.API.Tests.RegisterCashMember.Criteria
{
    public class RegisterCashMemberCommandTests
    {
        [Fact]
        public void IsValid_AllFieldsFilled_ReturnsTrue()
        {
            var cmd = new RegisterCashMemberCommand(1, "first", "last", "user", "user", "pass", "123456789", 1, "127.0.0.1");
            Assert.True(cmd.IsValid());
        }

        [Fact]
        public void IsValid_MissingFields_ReturnsFalse()
        {
            var cmd = new RegisterCashMemberCommand(1, "", "", "", "", "", "", 1, "");
            Assert.False(cmd.IsValid());
        }

        [Fact]
        public void GetSettingKey_ReturnsRegisterCashMember()
        {
            var cmd = new RegisterCashMemberCommand(1, "first", "last", "user", "user", "pass", "123456789", 1, "127.0.0.1");
            Assert.Equal("API_CustomerInsMember_CashMember", cmd.GetSettingKey());
        }
    }
}
