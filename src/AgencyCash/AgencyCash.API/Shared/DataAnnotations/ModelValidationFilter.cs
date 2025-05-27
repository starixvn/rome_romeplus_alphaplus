using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Shared.DataAnnotations;

public class ModelValidationFilter<T> : IEndpointFilter where T : class
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