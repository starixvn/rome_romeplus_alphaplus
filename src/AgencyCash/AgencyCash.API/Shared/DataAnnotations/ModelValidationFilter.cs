using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Shared.DataAnnotations;

[ExcludeFromCodeCoverage]
public class ModelValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (model != null)
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true))
            {
                return TypedResults.BadRequest(new ModelValidationResult(
                    validationResults.ToDictionary(
                        v => v.MemberNames.FirstOrDefault() ?? string.Empty,
                        v => new[] { v.ErrorMessage ?? string.Empty })));
            }
        }

        return await next(context);
    }
}