namespace AlphaPlus.CashService.API.Features.Shared.Models;

using System.Text.Json.Serialization;

public class BaseTransaction
{
    [JsonIgnore]
    public long TransId { get; set; }

    [JsonPropertyName("transId")]
    public string TransactionId => TransId.ToString();
}