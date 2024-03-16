using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GotoFreight.IATA.X;

public class ValidationX
{
    public static List<ValidationResult> RecurveValidate(object validatingObject)
    {
        var validationErrors = new List<ValidationResult>();

        if (validatingObject == null)
        {
            return validationErrors;
        }

        validationErrors = Validate(validatingObject);

        //Validate items of enumerable
        if (validatingObject is IEnumerable && !(validatingObject is IQueryable))
        {
            foreach (var item in (validatingObject as IEnumerable))
            {
                validationErrors.AddRange(RecurveValidate(item) ?? new List<ValidationResult>());
            }
        }

        //Do not recursively validate for enumerable objects
        if (validatingObject is IEnumerable)
        {
            return validationErrors;
        }

        var validatingObjectType = validatingObject.GetType();

        //Do not recursively validate for primitive objects
        if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObjectType))
        {
            return validationErrors;
        }

        var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
        foreach (var property in properties)
        {
            validationErrors.AddRange(RecurveValidate(property.GetValue(validatingObject)) ??
                                      new List<ValidationResult>());
        }

        return validationErrors;
    }

    public static List<ValidationResult> Validate(object validatingObject)
    {
        var validationErrors = new List<ValidationResult>();

        var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
        foreach (var property in properties)
        {
            var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
            if (validationAttributes.Length == 0)
            {
                continue;
            }

            var validationContext = new ValidationContext(validatingObject)
            {
                DisplayName = property.DisplayName,
                MemberName = property.Name
            };

            foreach (var attribute in validationAttributes)
            {
                var result = attribute.GetValidationResult(property.GetValue(validatingObject), validationContext);
                if (result != null)
                {
                    validationErrors.Add(result);
                }
            }
        }

        if (validatingObject is IValidatableObject)
        {
            var results = (validatingObject as IValidatableObject).Validate(new ValidationContext(validatingObject));
            validationErrors.AddRange(results);
        }

        return validationErrors;
    }
}