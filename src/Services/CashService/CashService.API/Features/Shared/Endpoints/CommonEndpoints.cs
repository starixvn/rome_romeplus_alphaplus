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

        group.MapGet("setup-checklist/{custId}", async (int custId, ICommonService service) =>
            {
                var setupCheckList = await service.GetSetupCheckList(custId);
                return Results.Ok(setupCheckList);
            })
            .WithName("GetSetupCheckList")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get setup checklist",
                Description = "Returns a setup checklist"
            });

        group.MapGet("banks", async (string site, int providerId, string languageCode, ICommonService service) =>
        {
            var banks = await service.GetProviderBanks(site, providerId, languageCode);
            return Results.Ok(banks);
        })
        .WithName("GetProviderBanks")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get provider banks",
            Description = "Returns a list of all available banks for a provider"
        });
    }
}