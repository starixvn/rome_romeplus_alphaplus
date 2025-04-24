using Newtonsoft.Json;
using System.Text;

namespace AlphaPlus.Settings.MemberInfo.Domain.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object payload = null)
        {
            var response = await httpClient.PostAsStringAsync(requestUri, payload ?? string.Empty);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public static async Task<T> GetAsJsonAsync<T>(this HttpClient httpClient, string requestUri)
        {
            var content = await httpClient.GetAsStringAsync(requestUri);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<string> GetAsStringAsync(this HttpClient httpClient, string requestUri)
        {
            using var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostAsStringAsync(this HttpClient httpClient, string requestUri, object payload = null)
        {
            using var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = stringContent };
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> PutAsStringAsync(this HttpClient httpClient, string requestUri, object payload = null)
        {
            using var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = stringContent };
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
