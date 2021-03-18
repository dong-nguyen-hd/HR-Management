using HR_Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Extensions.Validation
{
    public class ComponentOrderAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                List<int> tempList = new List<int>((List<int>)value);
                
                foreach (var item in tempList)
                    if (!ValidateElement(item))
                        return new ValidationResult("Invalid Component field.");

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("Invalid Component field.");
            }

            bool ValidateElement(int temp)
                => Enum.IsDefined(typeof(eOrder), temp);
        }
    }
}
