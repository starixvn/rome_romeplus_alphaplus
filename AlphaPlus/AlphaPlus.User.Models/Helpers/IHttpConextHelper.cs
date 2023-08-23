namespace AlphaPlus.User.Models.Helpers
{
    public interface IHttpContextHelper
    {
        string SessionId { get; }

        object GetSession(string key);

        void SetSession(string key, object value);
    }
}