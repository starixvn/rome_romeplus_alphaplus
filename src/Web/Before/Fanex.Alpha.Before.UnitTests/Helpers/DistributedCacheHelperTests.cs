using Fanex.Alpha.Before.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace Fanex.Alpha.Before.UnitTests.Helpers;

public class DistributedCacheHelperTests
{
    private sealed class InMemoryDistributedCache : IDistributedCache
    {
        private readonly Dictionary<string, byte[]> store = new();
        public byte[]? Get(string key) => store.TryGetValue(key, out var v) ? v : null;
        public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => Task.FromResult(Get(key));
        public void Refresh(string key) { }
        public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;
        public void Remove(string key) => store.Remove(key);
        public Task RemoveAsync(string key, CancellationToken token = default) { Remove(key); return Task.CompletedTask; }
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => store[key] = value;
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) { Set(key, value, options); return Task.CompletedTask; }
    }

    private sealed class DummyLogger : Fanex.Logging.ILogger
    {
        // Sync
        public void Debug(string message) { }
        public void Debug(string format, string category) { }
        public void Debug(string format, params object[] args) { }
        public void Debug(string format, string category, params object[] args) { }
        public void Info(string message) { }
        public void Info(string format, string category) { }
        public void Info(string format, params object[] args) { }
        public void Info(string format, string category, params object[] args) { }
        public void Warn(string message) { }
        public void Warn(string format, string category) { }
        public void Warn(string format, params object[] args) { }
        public void Warn(string format, string category, params object[] args) { }
        public void Error(string message) { }
        public void Error(string format, string category) { }
        public void Error(string message, Exception exception) { }
        public void Error(string format, string category, Exception exception) { }
        public void Critical(string message) { }
        public void Critical(string format, string category) { }
        public void Critical(string format, params object[] args) { }
        public void Critical(string format, string category, params object[] args) { }

        // Async
        public Task DebugAsync(string message) => Task.CompletedTask;
        public Task DebugAsync(string format, string category) => Task.CompletedTask;
        public Task DebugAsync(string format, params object[] args) => Task.CompletedTask;
        public Task DebugAsync(string format, string category, params object[] args) => Task.CompletedTask;
        public Task InfoAsync(string message) => Task.CompletedTask;
        public Task InfoAsync(string format, string category) => Task.CompletedTask;
        public Task InfoAsync(string format, params object[] args) => Task.CompletedTask;
        public Task InfoAsync(string format, string category, params object[] args) => Task.CompletedTask;
        public Task WarnAsync(string message) => Task.CompletedTask;
        public Task WarnAsync(string format, string category) => Task.CompletedTask;
        public Task WarnAsync(string format, params object[] args) => Task.CompletedTask;
        public Task WarnAsync(string format, string category, params object[] args) => Task.CompletedTask;
        public Task ErrorAsync(string message) => Task.CompletedTask;
        public Task ErrorAsync(string format, string category) => Task.CompletedTask;
        public Task ErrorAsync(string message, Exception exception) => Task.CompletedTask;
        public Task ErrorAsync(string format, string category, Exception exception) => Task.CompletedTask;
        public Task CriticalAsync(string message) => Task.CompletedTask;
        public Task CriticalAsync(string format, string category) => Task.CompletedTask;
        public Task CriticalAsync(string format, params object[] args) => Task.CompletedTask;
        public Task CriticalAsync(string format, string category, params object[] args) => Task.CompletedTask;
    }

    [Fact]
    public async Task SetGetContainRemove_Works()
    {
        var cache = new InMemoryDistributedCache();
        var helper = new DistributedCacheHelper(cache, new DummyLogger());

        var contained = await helper.Contain("k1");
        Assert.False(contained);

        await helper.Set("k1", new { A = 1 });

        contained = await helper.Contain("k1");
        Assert.True(contained);

        var value = await helper.Get("k1", new { A = 0 });
        Assert.Equal(1, value.A);

        await helper.Remove("k1");
        contained = await helper.Contain("k1");
        Assert.False(contained);
    }
}


