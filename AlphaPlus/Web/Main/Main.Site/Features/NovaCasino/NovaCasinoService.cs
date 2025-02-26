using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AlphaPlus.Main.Site.Features.NovaCasino.Models;

namespace AlphaPlus.Main.Site.Features.NovaCasino
{
    using AlphaPlus.Shared.AppResources.Main;

    public interface INovaCasinoService
    {
        Task<NovaCasinoSSOResult> SSO(NovaCasinoSSORequest request);
    }

    public class NovaCasinoService(Fanex.Logging.ILogger logger) : INovaCasinoService
    {
        private readonly Fanex.Logging.ILogger logger = logger;

        public async Task<NovaCasinoSSOResult> SSO(NovaCasinoSSORequest request)
        {
            try
            {
                using var httpClient = new HttpClient();
                var message = JsonSerializer.Serialize(request);
                httpClient.DefaultRequestHeaders.Add("Signature", GenerateSignature(message, NovaCasinoSettings.SecretKey));
                using var stringContent = new StringContent(message, Encoding.UTF8, "application/json");
                using var httpResponseMessage = await httpClient.PostAsync($"{NovaCasinoSettings.ServiceUrl}/api/NovaCasino/SingleSignOn", stringContent).ConfigureAwait(false);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<NovaCasinoSSOResponse<SSOSuccess>>(responseString);
                    return new NovaCasinoSSOResult { LaunchUrl = response.Data.LaunchUrl, CustIds = response.Data.CustIds, Message = response.ErrorMessage };
                }

                if (httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<NovaCasinoSSOResponse<UnderMaintenance>>(responseString);
                    return new NovaCasinoSSOResult
                    {
                        Code = (int)httpResponseMessage.StatusCode,
                        Message = string.Format(Resources.SystemUnderMaintenance, response.Data.StartTime, response.Data.EndTime)
                    };
                }

                await logger.ErrorAsync(await httpResponseMessage.Content.ReadAsStringAsync());

                return new NovaCasinoSSOResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Resources.InternalServerError
                };
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);

                return new NovaCasinoSSOResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Resources.InternalServerError
                };
            }
        }

        private static string GenerateSignature(string message, string secretKey)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(message);

            using var hmacsha256 = new HMACSHA256(keyBytes);
            byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashMessage);
        }
    }
}