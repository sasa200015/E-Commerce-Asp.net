using System.ComponentModel.DataAnnotations;
using E_Commerce.Models;

namespace E_Commerce.DTO
{
    public class Register
    {
        [Required]
        [MaxLength(20,ErrorMessage ="The max length is 20 ")]
        [MinLength(3,ErrorMessage ="The min length is 3 ")]
        public string User_Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._]+@gmail\.com$",ErrorMessage = "Please enter a valid email address like \"expample.exe@gmail.com\"")]
        [UniqueName]
        public  string  Email { get; set; }
        [Required]
        [MinLength(8,ErrorMessage ="The min length is 8")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).+$", ErrorMessage = "Password must be start by upper letter and use special character $#@%^&* and numbers 0-9.")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^(011|010|012|015)[0-9]{8}$",ErrorMessage ="Please enter valid phone number")]
        public string Phone_Number { get; set; }
    }
}
