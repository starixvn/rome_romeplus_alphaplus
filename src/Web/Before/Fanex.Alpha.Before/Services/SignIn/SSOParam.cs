namespace Fanex.Alpha.Before.Services.SignIn;

public class SSOParam
{
    public int CustId { get; init; }

    public int SubAccId { get; init; }

    public long LoginTime { get; init; }

    public string EncryptPassword { get; init; }

    public string Nickname { get; init; }

    public bool IsNickname { get; init; }

    public string Language { get; init; }

    public bool NumberCrossCookie { get; init; }

    public string DeviceId { get; init; }

    public string BrowserSize { get; init; }

    public string EncryptCode { get; init; }

    public string DarkModeName { get; init; }

    public string UMToken { get; init; }
}
