using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class UniqueNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string email = value.ToString()!;

            // Resolve the UserManager service from the validation context
            var userManager = (UserManager<User>)validationContext.GetService(typeof(UserManager<User>));

            if (userManager == null)
            {
                throw new InvalidOperationException("UserManager service is not available in the validation context.");
            }

            // Check if the email is already in use
            var user = userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                return new ValidationResult("This email is already used.");
            }

            return ValidationResult.Success;
        }

    }
}
