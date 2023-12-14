using AlphaPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Helpers;

public interface ISessionManager
{
    void Add<T>(string key, T item);

    T Get<T>(string key);

    void Remove(string key);
}

public class SessionManager : ISessionManager
{
    private readonly ISession httpSession;

    public SessionManager() : this(null) { }

    internal SessionManager(ISession httpSession)
    {
        this.httpSession = httpSession ?? AlphaContext.Current.Session;
    }

    public void Add<T>(string key, T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        ValidateKey(key);

        httpSession.Set(key, item);
    }

    public T Get<T>(string key)
    {
        ValidateKey(key);

        return httpSession.Get<T>(key);
    }

    public void Remove(string key) => httpSession.Remove(key);

    private static void ValidateKey(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentOutOfRangeException(nameof(key), @"Cache keys cannot be empty or whitespace");
        }
    }
}