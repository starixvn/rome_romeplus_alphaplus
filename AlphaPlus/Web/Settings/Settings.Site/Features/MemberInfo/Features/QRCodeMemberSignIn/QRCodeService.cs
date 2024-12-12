using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn;

public interface IQRCodeService
{
    Task<string> CreateMobileLoginUrl(
      string domain,
      int siteid,
      string username,
      int TokenExpireSecond);
}

public class QRCodeService(HttpClient httpClient) : IQRCodeService
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<string> CreateMobileLoginUrl(string domain, int siteid, string username, int TokenExpireSecond)
    {
        string token = await CreateToken(siteid, username, TokenExpireSecond);
        return domain + "/Login/UrlLogin?token=" + token;
    }

    private async Task<string> CreateToken(int siteid, string username, int TokenExpireSecond)
    {
        string data = await GetData(string.Format("GenerateQrLoginToken?siteId={0}&userName={1}&tokenExpireSecond={2}", siteid, username, TokenExpireSecond));
        return !string.IsNullOrWhiteSpace(data) ? data : throw new Exception("CreateToken Error , token is null");
    }

    private Task<string> GetData(string url) => httpClient.GetAsStringAsync(url);
}