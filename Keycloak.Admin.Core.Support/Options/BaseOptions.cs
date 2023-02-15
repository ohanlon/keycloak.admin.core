using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Keycloak.Admin.Core.Support.Options;

/// <summary>
/// Base object for validating complex objects.
/// </summary>
/// <typeparam name="T">The type of the object being validated</typeparam>
/// <remarks>The implementation of this is sourced from <see href="https://www.ariank.dev/simple-solution-for-complex-validation-in-asp-net-core/"/></remarks>
public abstract class BaseOptions<T> : IValidatableObject where T : BaseOptions<T>, new()
{
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        foreach (var propertyInfo in GetType().GetProperties())
        {
            var option = propertyInfo.GetValue(this, null);
            switch (option)
            {
                case null:
                case string _:
                    continue;
                case IEnumerable list:
                {
                    ValidateList(list, propertyInfo.Name, validationContext, results);
                    continue;
                }
                default:
                    ValidateOption(option, validationContext, results);
                    continue;
            }
        }

        return results;
    }

    private static void ValidateList(IEnumerable list, string propertyInfoName, ValidationContext validationContext,
        List<ValidationResult> results)
    {
        foreach (var optionItem in list)
        {
            if (optionItem == null)
            {
                results.Add(new ValidationResult($"{propertyInfoName} contains null in the list."));
            }
            else
            {
                ValidateOption(optionItem, validationContext, results);
            }
        }
    }

    private static void ValidateOption(object option, ValidationContext validationContext,
        List<ValidationResult> results)
    {
        Validator.TryValidateObject(option,
            new ValidationContext(option, validationContext, validationContext.Items),
            results, true);
    }
}