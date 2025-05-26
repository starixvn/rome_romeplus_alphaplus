using System.ComponentModel.DataAnnotations;
using PaymentService.API.Shared.Models;

namespace PaymentService.API.Shared.Helpers;

public class ModelValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var parameter = context.Arguments.OfType<T>().FirstOrDefault();

        if (parameter != null)
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(parameter, new ValidationContext(parameter), validationResults, validateAllProperties: true))
            {
                return Results.BadRequest(new Result(StatusCodes.Status400BadRequest, validationResults[0].ErrorMessage));
            }
        }

        return await next(context);
    }
}