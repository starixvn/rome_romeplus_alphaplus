using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AgencyCash.API.Shared.DataAnnotations;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class RequiredIfAttribute(string dependentProperty, object targetValue) : ValidationAttribute("{0} is required.")
{
    private readonly string _dependentProperty = dependentProperty;
    private readonly object _targetValue = targetValue;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dependentProperty = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (dependentProperty == null)
        {
            return new ValidationResult($"Unknown property: {_dependentProperty}");
        }

        var dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance);
        var conditionMet = Equals(dependentValue, _targetValue);

        if (conditionMet)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name) => string.Format(ErrorMessageString, name);
}