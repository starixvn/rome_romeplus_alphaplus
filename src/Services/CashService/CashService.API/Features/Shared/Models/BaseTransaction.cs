namespace AlphaPlus.CashService.API.Features.Shared.Models;

using System.Numerics;
using System.Text.Json.Serialization;

public class BaseTransaction
{
    [JsonIgnore]
    public BigInteger TransId { get; set; }

    [JsonPropertyName("transId")]
    public string TransIdString => TransId.ToString();
}