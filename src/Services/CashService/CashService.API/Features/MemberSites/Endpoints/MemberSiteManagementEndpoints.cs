using AlphaPlus.CashService.API.Features.MemberSites.Services;
using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Features.MemberSites.Endpoints;

public static class MemberSiteManagementEndpoints
{
    public static void MapMemberSiteManagementEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/member-site-info")
            .WithTags("Member Site Management");

        // Get all settings
        group.MapGet("/", async (IMemberSiteManagementService service, [FromQuery] int custId, [FromQuery] string site) =>
        {
            var result = await service.GetAsync(custId, site);
            return Results.Ok(result);
        })
        .WithName("GetMemberSiteManagement")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member site management settings",
            Description = "Returns all member site management settings including domains, general settings, and messages"
        });

        // Update settings
        group.MapPut("/", async (IMemberSiteManagementService service, [FromBody] UpdateMemberSiteModel model) =>
        {
           return await service.UpdateAsync(model);
        })
        .WithName("UpdateMemberSiteManagement")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update member site management settings",
            Description = "Updates all member site management settings"
        });
    }
}