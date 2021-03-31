using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HR_Management.Extensions.Validation
{
    public class UserNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string source = value.ToString().ToLower();
                if (Regex.IsMatch(source, @"\s", RegexOptions.Compiled))
                    return new ValidationResult("User name is not contain any space characters.");
                else
                    return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("Invalid User name field.");
            }
        }
    }
}
