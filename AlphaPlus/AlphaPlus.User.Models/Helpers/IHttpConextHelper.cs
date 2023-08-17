namespace AlphaPlus.User.Models.Helpers
{
    public interface IHttpContextHelper
    {
        string SessionId { get; }

        string GetSession(string key);

        void SetSession(string key, string value);
    }
}