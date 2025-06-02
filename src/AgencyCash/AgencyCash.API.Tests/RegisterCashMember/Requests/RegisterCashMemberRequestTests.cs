using AgencyCash.API.RegisterCashMember.Requests;

namespace AgencyCash.API.Tests.RegisterCashMember.Requests
{
    public class RegisterCashMemberRequestTests
    {
        [Fact]
        public void Can_Set_Properties()
        {
            var req = new RegisterCashMemberRequest
            {
                AgentId = 1,
                UserName = "user",
                Password = "pass",
                FirstName = "first",
                LastName = "last",
                Mobile = "123456789",
                Domain = "domain",
                Ip = "127.0.0.1"
            };
            Assert.Equal(1, req.AgentId);
            Assert.Equal("user", req.UserName);
            Assert.Equal("pass", req.Password);
            Assert.Equal("first", req.FirstName);
            Assert.Equal("last", req.LastName);
            Assert.Equal("123456789", req.Mobile);
            Assert.Equal("domain", req.Domain);
            Assert.Equal("127.0.0.1", req.Ip);
        }
    }
}
