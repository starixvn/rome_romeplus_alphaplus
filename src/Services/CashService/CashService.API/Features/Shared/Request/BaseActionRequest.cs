namespace AlphaPlus.CashService.API.Features.Shared.Request;

public class BaseActionRequest
{
    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
} 