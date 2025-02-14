using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class Roles
    {
        [Required]
        public string RoleName { get; set; }
    }
}
