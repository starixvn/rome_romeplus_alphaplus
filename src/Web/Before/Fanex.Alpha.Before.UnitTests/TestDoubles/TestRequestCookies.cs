using Microsoft.AspNetCore.Http;

namespace Fanex.Alpha.Before.UnitTests.TestDoubles;

public sealed class TestRequestCookies : IRequestCookieCollection
{
    private readonly Dictionary<string, string> cookies;

    public TestRequestCookies()
        : this(new Dictionary<string, string>(StringComparer.Ordinal))
    {
    }

    public TestRequestCookies(IDictionary<string, string> initial)
    {
        cookies = new Dictionary<string, string>(initial, StringComparer.Ordinal);
    }

    public int Count => cookies.Count;
    public ICollection<string> Keys => cookies.Keys;

    public bool ContainsKey(string key) => cookies.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => cookies.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => cookies.GetEnumerator();

    public bool TryGetValue(string key, out string value)
        => cookies.TryGetValue(key, out value!);

    public string this[string key]
        => cookies.TryGetValue(key, out var value) ? value : string.Empty;
}


