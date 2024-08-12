using System.Text.RegularExpressions;
using System.Web;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Core;

public interface IRequestAnalyst
{
    int GetCustomerId(string parameterName);
}

public class RequestAnalyst : IRequestAnalyst
{
    public int GetCustomerId(string parameterName)
    {
        var customerId = 0;
        if (!string.IsNullOrEmpty(parameterName))
        {
            var jsonString = GetJsonDataInRequestBody(AlphaContext.Current.Request.Body);
            var pattern = string.Format("(\"{0}\":)(?<paramValue>[^,}}]+)", parameterName);
            var regexMatches = Regex.Matches(jsonString, pattern, RegexOptions.IgnoreCase);
            var matchLength = regexMatches.Count;
            if (matchLength > 0)
            {
                var lastMatch = regexMatches[matchLength - 1];
                customerId = Convert.ToInt32(lastMatch.Groups["paramValue"].ToString().Replace("\"", string.Empty));
            }
        }

        return customerId;
    }

    private static string GetJsonDataInRequestBody(Stream requestStream)
    {
        if (requestStream != null && requestStream.CanSeek)
        {
            requestStream.Seek(0, SeekOrigin.Begin);
            var bodyData = new StreamReader(requestStream).ReadToEnd();
            requestStream.Seek(0, SeekOrigin.Begin);
            if (!string.IsNullOrWhiteSpace(bodyData) && bodyData.Contains('{'))
            {
                return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
            }
        }

        return string.Empty;
    }
}
