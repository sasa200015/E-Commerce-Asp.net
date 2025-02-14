using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20,ErrorMessage ="The maximum length is 20")]
        [MinLength(3)]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
