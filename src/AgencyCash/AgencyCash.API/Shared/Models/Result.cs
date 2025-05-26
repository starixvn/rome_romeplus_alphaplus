using System.Text.Json.Serialization;

namespace AgencyCash.API.Shared.Models;

public record Result(int Code, string Message)
{
    [JsonIgnore]
    public bool IsSuccess { get; } = Code == 0;
}
