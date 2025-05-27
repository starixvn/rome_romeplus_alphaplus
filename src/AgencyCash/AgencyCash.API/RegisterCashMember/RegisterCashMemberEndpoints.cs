using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Responses;
using System.ComponentModel.DataAnnotations;

namespace AgencyCash.API.RegisterCashMember;

public static class RegisterCashMemberEndpoints
{
    public static void MapRegisterCashMemberEndpoints(this WebApplication app)
    {
        app.MapPost("/api/cash-member/register", HandleAsync)
            .AddEndpointFilter<ValidationFilter<RegisterCashMemberRequest>>()
            .WithName("Register Cash Member")
            .WithTags("Cash Service");

        app.MapGet("/api/cash-member/check-username", async (string username, IRegisterCashMemberService service) =>
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

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();
        if (model != null)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                return Results.ValidationProblem(validationResults.ToDictionary(
                    v => v.MemberNames.FirstOrDefault() ?? string.Empty,
                    v => new[] { v.ErrorMessage ?? "Invalid" }
                ));
            }
        }
        return await next(context);
    }
}