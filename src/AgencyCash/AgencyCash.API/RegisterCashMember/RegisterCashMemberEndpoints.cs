using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Responses;
using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.RegisterCashMember;

public static class RegisterCashMemberEndpoints
{
    public static void MapRegisterCashMemberEndpoints(this WebApplication app)
    {
        app.MapPost("/api/cash-members", HandleAsync)
            .AddEndpointFilter<ModelValidationFilter<RegisterCashMemberRequest>>()
            .WithName("Register Cash Member")
            .WithTags("Cash Service");

        app.MapGet("/api/cash-members/check-username", async (string username, IRegisterCashMemberService service) =>
        {
            var result = await service.CheckUsernameAvailable(new CheckUsernameCommand(username));
            return Results.Ok(result);
        })
        .WithName("Check Cash Member Username")
        .WithTags("Cash Service");
    }

    public static async Task<IResult> HandleAsync(RegisterCashMemberRequest request, IRegisterCashMemberService service)
    {
        // Fanex.Logging.LogService.Info(JsonSerializer.Serialize(request));
        // if (!service.ValidateRequest(request))
        // {
        //     return Results.Ok(Shared.Models.Result.CreateError(StaticResources.Get("InvalidParameterValue", request.Context.Language ?? LanguageUtils.DefaultLanguage)));
        // }

        return Results.Ok(await service.RegisterCashMember(new RegisterCashMemberCommand(
            request.AgentId,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.Mobile)));

    }
}