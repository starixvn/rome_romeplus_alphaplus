namespace AlphaPlus.Identity.API.Features.Auth;

public class InternalLoginResponse
{
    public int AdminId { get; set; }

    public int CustId { get; set; }

    public string Username { get; set; }

    public byte RoleId { get; set; }

    public int On_UserId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}