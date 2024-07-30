namespace RDS.Core.Common.Extensions;

public class GreaterThanZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("O valor é obrigatório.");
        }

        if (value is decimal decimalValue)
        {
            if (decimalValue > 0)
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("O valor deve ser maior do que zero.");
        }

        return new ValidationResult("O valor deve ser um número decimal.");
    }
}