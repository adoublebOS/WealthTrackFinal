using System.ComponentModel.DataAnnotations;

namespace WealthTrack.Domain.Attributes;

public class NonNegativeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is decimal and < 0)
        {
            return new ValidationResult("The field must be a non-negative value.");
        }
        
        return ValidationResult.Success!;
    }
}