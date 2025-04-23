using Microsoft.AspNetCore.Http.HttpResults;
using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.API.Endpoints;

public static class MemberSiteManagementEndpoints
{
    public static void MapMemberSiteManagementEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/member-site-info")
            .WithTags("Member Site Management");

        // Get all settings
        group.MapGet("/", async (IMemberSiteManagementService service, [FromQuery] int custId) =>
        {
            var result = await service.GetAsync(custId);
            return Results.Ok(result);
        })
        .WithName("GetMemberSiteManagement")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get member site management settings",
            Description = "Returns all member site management settings including domains, general settings, and messages"
        });

        // Update settings
        group.MapPut("/", async (IMemberSiteManagementService service, [FromBody] MemberSiteManagement model) =>
        {
            var result = await service.UpdateAsync(model);
            return Results.Ok(result);
        })
        .WithName("UpdateMemberSiteManagement")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update member site management settings",
            Description = "Updates all member site management settings"
        });
    }
} 