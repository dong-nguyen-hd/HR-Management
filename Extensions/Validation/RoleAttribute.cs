using HR_Management.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Extensions.Validation
{
    public class RoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (Enum.IsDefined(typeof(eRole), value))
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Invalid Role field.");
            }
            catch (Exception)
            {
                return new ValidationResult("Invalid Role field.");
            }
        }
    }
}
