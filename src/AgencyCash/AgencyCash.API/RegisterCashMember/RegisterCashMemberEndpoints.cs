using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.RegisterCashMember;

public static class RegisterCashMemberEndpoints
{
    public static void MapRegisterCashMemberEndpoints(this WebApplication app)
    {
        app.MapPost("/api/cash-members", RegisterCashMemberAsync)
            .AddEndpointFilter<ModelValidationFilter<RegisterCashMemberRequest>>()
            .WithName("Register Cash Member")
            .WithTags("Cash Service");

        app.MapGet("/api/cash-members/check-username", CheckUsernameAsync)
            .AddEndpointFilter<ModelValidationFilter<CheckUsernameRequest>>()
            .WithName("Check Cash Member Username")
            .WithTags("Cash Service");
    }

    public static async Task<IResult> RegisterCashMemberAsync(RegisterCashMemberRequest request, IRegisterCashMemberService service)
    {
        var result = await service.RegisterCashMember(request);
        return Results.Ok(result);
    }

    private static async Task<IResult> CheckUsernameAsync([AsParameters] CheckUsernameRequest request, IRegisterCashMemberService service)
    {
        var result = await service.CheckUsernameAvailable(new CheckUsernameCommand(request.Username));
        return Results.Ok(result);
    }
}