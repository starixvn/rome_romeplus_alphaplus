using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.Core.UnitTests.Helpers;

public class StaticResourcesTests
{
    private class HttpContextAccessorStub : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }
    }

    private static DefaultHttpContext CreateHttpContext(string pathBase = "/app")
    {
        var context = new DefaultHttpContext();
        context.Request.PathBase = pathBase;
        context.Session = new DummySession();
        return context;
    }

    private class DummySession : ISession
    {
        private readonly Dictionary<string, byte[]> store = new();

        public IEnumerable<string> Keys => store.Keys;
        public string Id { get; } = Guid.NewGuid().ToString();
        public bool IsAvailable { get; } = true;
        public void Clear() => store.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => store.Remove(key);
        public void Set(string key, byte[] value) => store[key] = value;
        public bool TryGetValue(string key, out byte[] value) => store.TryGetValue(key, out value);
    }

    public StaticResourcesTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ForcedHideProductIds"] = "1,2",
                ["MarketsSupportedNewHandbook"] = "*",
                ["AssetsApp:StaticResourcesVersion"] = "1.2.3",
                ["AssetAppPath"] = "/assets",
                ["SiteLabel"] = "mbc"
            })
            .Build();
        AlphaPlus.Core.Configurations.ConfigurationManager.Init(config);

        var accessor = new HttpContextAccessorStub { HttpContext = CreateHttpContext("/app") };
        AlphaContext.Configure(accessor);

        // default setup
        StaticResources.Init("1.2.3");
        var userSession = UserSession.Current;
        userSession.RoleId = UserLevel.Agent.Value;
        userSession.DarkModeName = ".dark";
        userSession.AssetAppPath = string.Empty;
    }

    [Fact]
    public void RenderScripts_Should_Return_Script_Tags_With_Version()
    {
        var html = StaticResources.RenderScripts("~/assets/js/app");
        Assert.IsType<HtmlString>(html);
        Assert.Contains("/app/assets/js/app.js?v=", html.ToString());
    }

    [Fact]
    public void RenderStyles_Should_Return_Link_Tags_With_Version()
    {
        var html = StaticResources.RenderStyles("~/assets/css/style");
        Assert.IsType<HtmlString>(html);
        Assert.Contains("/app/assets/css/style.css?v=", html.ToString());
    }

    [Fact]
    public void RenderStylesWithRoleAndDarkMode_Should_Include_Role_And_DarkMode()
    {
        var html = StaticResources.RenderStylesWithRoleAndDarkMode("~/assets/css/site");
        Assert.Contains("/app/assets/css/siteagent.dark.css?v=", html.ToString());
    }

    [Fact]
    public void AssetScripts_Render_Should_Use_AssetAppPath()
    {
        var html = StaticResources.AssetScripts.Render("/common");
        Assert.Contains("/assets/common.js?v=1.2.3", html.ToString());
    }
}


