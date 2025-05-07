using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.RegisterCashMember.Criteria;
namespace AgencyCash.API.RegisterCashMember;

public static class RegisterCashMemberEndpoint 
{
    public static void MapRegisterCashMemberEndpoint(this WebApplication app)
    {
        app.MapPost("/api/cash-member/register", async (RegisterCashMemberRequest request, IRegisterCashMemberService service) 
        => HandleAsync(request, service))
        .WithName("RegisterCashMember")
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
            request.PhoneNumber)));
    }
}