namespace AlphaPlus.User.Models.Helpers
{
    public interface IHttpContextHelper
    {
        string GetSession(string key);

        void SetSession(string key, string value);
    }
}