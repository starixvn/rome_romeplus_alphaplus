using System;
using System.IO;
using System.Net;
using System.Text;
using AlphaPlus.Configurations;

namespace ONEbook.Common.Auth
{
  public class Authentication
  {
    private static string _ApiUrl = ConfigurationManager.GetValue("ONEbook.Common.Auth.API", "http://10.40.40.146");
    private static Authentication _auth = new Authentication();

    public static Authentication Instance => Authentication._auth;

    public string CreateMobileLoginUrl(
      string domain,
      int siteid,
      string username,
      int TokenExpireSecond)
    {
      string token = this.CreateToken(siteid, username, TokenExpireSecond);
      return domain + "/Login/UrlLogin?token=" + token;
    }

    public string CreateToken(int siteid, string username, int TokenExpireSecond)
    {
      string data = Authentication.GetData(string.Format("{0}/GenerateQrLoginToken?siteId={1}&userName={2}&tokenExpireSecond={3}", (object)Authentication._ApiUrl, (object)siteid, (object)username, (object)TokenExpireSecond));
      return !string.IsNullOrEmpty(data) ? data : throw new Exception("CreateToken Erron , token is null");
    }

    public string CheckToken(string Token) => Authentication.GetData(Authentication._ApiUrl + "/GetQrLoginData?token=" + Token);

    private static string GetData(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
      httpWebRequest.Method = "GET";
      StringBuilder stringBuilder = new StringBuilder();
      using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
      {
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
          stringBuilder.Append(streamReader.ReadToEnd());
      }
      return stringBuilder.ToString();
    }
  }
}
