using System.ComponentModel.DataAnnotations;

namespace WEB_voorbereiding.ModelValidation
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // als er geen @ in zit, is niet geldig
            if (value.ToString().IndexOf('@') != -1)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "Dit is geen e-mailadres");
            }
        }
    }
}
