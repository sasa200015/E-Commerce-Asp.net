using System.ComponentModel.DataAnnotations;
using System.IO;
namespace E_Commerce.Models
{
    public class AllowedExtinstions : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtinstions(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return ValidationResult.Success; // No file, validation passes (optional depending on requirements)
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"This file extension is not allowed. Allowed extensions are: {string.Join(", ", _extensions)}");
            }

            return ValidationResult.Success;
        }
    }
}
