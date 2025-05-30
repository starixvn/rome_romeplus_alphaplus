namespace AgencyCash.API.Shared.Models;

public record ModelValidationResult : Result
{
    public ModelValidationResult(IDictionary<string, string[]> error)
        : base(StatusCodes.Status400BadRequest, "Model validation failed.")
    {
        Error = error;
    }

    public ModelValidationResult(string key, string value)
        : base(StatusCodes.Status400BadRequest, "Model validation failed.")
    {
        Error = new Dictionary<string, string[]>
        {
            [key] = [value]
        };
    }

    public IDictionary<string, string[]> Error { get; }
}