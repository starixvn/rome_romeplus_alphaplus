namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class ManualBonus
{
    public string Username { get; init; }

    public string Nickname { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public decimal Amount { get; init; }

    public string Creator { get; init; }

    public DateTime CreatedTime { get; init; }

    public string SubAccName { get; init; }

    public byte StatusId { get; init; }

    public string Remark { get; init; }

    public long TransId { get; init; }

    public string RefNo => TransId.ToString();
}
