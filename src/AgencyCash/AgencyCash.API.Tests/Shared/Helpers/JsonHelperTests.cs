using AgencyCash.API.Shared.Helpers;

namespace AgencyCash.API.Tests.Shared.Helpers
{
    public class JsonHelperTests
    {
        [Fact]
        public void ToJson_ReturnsJsonString()
        {
            var obj = new { Name = "Test", Value = 123 };
            var json = JsonHelper.ToJson(obj);
            Assert.Contains("Name", json);
            Assert.Contains("Value", json);
        }
    }
}
