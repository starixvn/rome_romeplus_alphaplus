using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace Fanex.Alpha.Before.UnitTests.TestDoubles;

public sealed class TestSession : ISession
{
    private readonly ConcurrentDictionary<string, byte[]> store = new();

    public bool IsAvailable => true;
    public string Id { get; } = Guid.NewGuid().ToString();
    public IEnumerable<string> Keys => store.Keys;

    public void Clear() => store.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Remove(string key) => store.TryRemove(key, out _);
    public void Set(string key, byte[] value) => store[key] = value;
    public bool TryGetValue(string key, out byte[] value) => store.TryGetValue(key, out value!);
}


