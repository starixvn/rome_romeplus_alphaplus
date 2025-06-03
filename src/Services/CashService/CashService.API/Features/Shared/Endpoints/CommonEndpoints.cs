namespace AlphaPlus.CashService.API.Features.Shared.Endpoints;

using Services;

public static class CommonEndpoints
{
    public static void MapCommonEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/common")
            .WithTags("Common Get APIs");

        group.MapGet("country-codes/{siteName}", async (string siteName, ICommonService service) =>
            {
                var prefixes = await service.GetCountryCodes(siteName);
                return Results.Ok(prefixes);
            })
            .WithName("GetCountryCodes")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get country codes",
                Description = "Returns a list of all available country codes"
            });
    }
}