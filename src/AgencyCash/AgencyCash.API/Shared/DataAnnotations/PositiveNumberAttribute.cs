using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AgencyCash.API.Shared.DataAnnotations;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PositiveNumberAttribute : ValidationAttribute
{
    public PositiveNumberAttribute() : base("{0} must be a positive number.")
    {
    }

    public override bool IsValid(object value)
    {
        if (value == null) // Not required
        {
            return true;
        }

        return value switch
        {
            byte b => b > 0,
            short s => s > 0,
            int i => i > 0,
            decimal m => m > 0,
            _ => false
        };
    }

    public override string FormatErrorMessage(string name) => string.Format(ErrorMessageString, name);
}