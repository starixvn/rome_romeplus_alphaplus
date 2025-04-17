using AlphaPlus.Identity.API.Features.Authorization.Requests;
using AlphaPlus.Identity.Features.Authorization.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlphaPlus.Identity.Features.Authorization;

public static class AuthorizationEndpoints
{
    public static IEndpointRouteBuilder MapAuthorizeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var auth = endpoints.MapGroup("/api/authorize").WithTags("Authorize").WithOpenApi();

        auth.MapGet("/customer-status/{custId}/{subAccId}", GetCustomerStatus);
        auth.MapGet("/belonging", CheckBelonging);
        auth.MapGet("/belonging-consolidatedgroup", CheckBelongingConsolidatedGroup);
        auth.MapGet("/belonging-multiple", CheckBelongingMultipleCustomers);

        return endpoints;
    }

    private static async Task<Results<Ok<CustomerStatus>, BadRequest>> GetCustomerStatus(int custId, int subAccId, IAuthorizationService service)
    {
        var customerStatus = await service.GetCustomerStatus(custId, subAccId);

        return TypedResults.Ok(customerStatus);
    }

    private static async Task<Results<Ok<BelongingInfo>, BadRequest>> CheckBelonging([AsParameters] BelongingQuery message, IAuthorizationService service)
    {
        var belongingInfo = await service.CheckBelonging(message.CustId, message.SubAccId, message.TransId, message.DownlineCustId);

        return TypedResults.Ok(belongingInfo);
    }

    private static async Task<Results<Ok<BelongingInfo>, BadRequest>> CheckBelongingConsolidatedGroup([AsParameters] BelongingQuery message, IAuthorizationService service)
    {
        var belongingInfo = await service.CheckBelongingConsolidatedGroup(message.CustId, message.SubAccId, message.TransId, message.DownlineCustId);

        return TypedResults.Ok(belongingInfo);
    }

    private static async Task<Results<Ok<BelongingInfo>, BadRequest>> CheckBelongingMultipleCustomers([AsParameters] BelongingMultipleQuery message, IAuthorizationService service)
    {
        var belongingInfo = await service.CheckBelongingMultipleCustomers(message.CustId, message.SubAccId, message.CustIds);

        return TypedResults.Ok(belongingInfo);
    }
}