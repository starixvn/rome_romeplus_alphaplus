using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Tests.Shared.Models
{
    public class ResultTests
    {
        [Fact]
        public void IsSuccess_True_WhenCodeIsZero()
        {
            var result = new Result(0, "ok");
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void IsSuccess_False_WhenCodeIsNotZero()
        {
            var result = new Result(1, "fail");
            Assert.False(result.IsSuccess);
        }
    }
}
