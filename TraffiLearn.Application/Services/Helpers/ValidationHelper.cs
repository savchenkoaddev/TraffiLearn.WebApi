using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Application.Services.Helpers
{
    internal class ValidationHelper
    {
        internal async static Task ValidateObjects(params object?[]? objects)
        {
            if (objects is null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            foreach (var obj in objects)
            {
                if (obj is null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                ValidationContext validationContext = new ValidationContext(obj);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true))
                {
                    throw new ValidationException(validationResults.First().ErrorMessage);
                }
            }

            await Task.CompletedTask;
        }
    }
}
