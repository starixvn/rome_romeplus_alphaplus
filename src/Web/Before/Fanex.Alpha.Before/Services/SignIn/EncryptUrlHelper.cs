using System.Text.Json;
using Fanex.Alpha.Before.Configurations;
using Fanex.Alpha.Before.Models;
using Fanex.Cryptography.Aes;

namespace Fanex.Alpha.Before.Services.SignIn
{
    public interface IEncryptUrlHelper
    {
        string BuildAfterURL(
            HttpContext httpContext,
            string encryptPassword,
            VerifySignInResult verifySignInResult,
            string language,
            string deviceId,
            string browserSize,
            string encryptCode,
            string darkModeName);
    }

    public class EncryptUrlHelper : IEncryptUrlHelper
    {
        public const string UTMS = "__utms";
        private const string SecretKey = "!@#$";

        public string BuildAfterURL(
            HttpContext httpContext,
            string encryptPassword,
            VerifySignInResult verifySignInResult,
            string language,
            string deviceId,
            string browserSize,
            string encryptCode,
            string darkModeName)
        {
            var parameter = new SSOParam
            {
                CustId = verifySignInResult.CustId,
                SubAccId = verifySignInResult.SubAccId,
                LoginTime = verifySignInResult.LoginTime.Ticks,
                EncryptPassword = encryptPassword,
                Nickname = verifySignInResult.Nickname,
                IsNickname = verifySignInResult.IsNickname,
                Language = language,
                NumberCrossCookie = NumberCrossCookie(httpContext),
                DeviceId = deviceId,
                BrowserSize = browserSize,
                EncryptCode = encryptCode,
                DarkModeName = darkModeName,
                UMToken = GetUMToken(httpContext)
            };

            var host = httpContext.Request.Host.Value.TrimEnd('.');
            var afterUrl = AppSetting.DomainMappings[host];

            var url = $"{afterUrl}/Account/SSO?parameter={EncryptParameter(JsonSerializer.Serialize(parameter))}";

            return AddUMToken(httpContext, url);
        }

        private static bool NumberCrossCookie(HttpContext httpContext)
        {
            var utmsCookie = httpContext.Request.Cookies[UTMS];

            return string.IsNullOrWhiteSpace(utmsCookie) || utmsCookie == AppSession.CookieCode;
        }

        private static string EncryptParameter(string parameter)
            => AesService.Default.Encrypt(parameter, SecretKey);

        private static string AddUMToken(HttpContext httpContext, string url)
        {
            var umToken = GetUMToken(httpContext);

            return umToken == null
                ? url
                : $"{url}&UMToken={umToken}";
        }

        private static string GetUMToken(HttpContext httpContext)
            => httpContext.Session.GetString("UMToken");
    }
}
