using System.Net;
using System.Text;

namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class CustomHttpClientException : HttpRequestException
{
    public Uri RequestUri { get; }
    public string RequestContent { get; }

    public CustomHttpClientException(
        string message, 
        Exception innerException, 
        Uri requestUri, 
        string requestContent, 
        HttpStatusCode statusCode)
        : base(message, innerException, statusCode)
    {
        RequestUri = requestUri;
        RequestContent = requestContent;
    }

    public CustomHttpClientException(Uri requestUri, string requestContent, HttpStatusCode statusCode, string reasonPhrase)
        : base($"Response status code does not indicate success: {(int)statusCode} ({reasonPhrase ?? "Unknown reason"}).", null, statusCode)
    {
        RequestUri = requestUri;
        RequestContent = requestContent;
    }

    public override string Message
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine(base.Message);
            if (RequestUri != null)
            {
                sb.AppendLine($"Request URL: {RequestUri}");
            }
            if (!string.IsNullOrEmpty(RequestContent))
            {
                sb.AppendLine($"Request Data: {RequestContent}");
            }
            return sb.ToString();
        }
    }
}