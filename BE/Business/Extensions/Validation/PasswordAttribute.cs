using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Business.Extensions.Validation
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string source = value.ToString().ToLower();
                if (!Regex.IsMatch(source, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled))
                    return new ValidationResult("Password must MD5 format.");
                else
                    return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("Password must MD5 format.");
            }
        }
    }
}
