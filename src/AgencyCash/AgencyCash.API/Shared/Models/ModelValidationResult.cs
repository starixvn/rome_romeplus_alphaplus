namespace AgencyCash.API.Shared.Models;

public record ModelValidationResult(IDictionary<string, string[]> Error)
    : Result(StatusCodes.Status400BadRequest, "Model validation failed.");