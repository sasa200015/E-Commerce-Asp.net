using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage ="The maximum is 30 ")]
        [MinLength(3,ErrorMessage ="The minimum length is 3")]
        public string Title { get; set; }

        [Required]
        [Range(20,10000,ErrorMessage ="The range should be between 20 to 10000")]
        public double Price { get; set; }

        [Required]
        [MaxLength(20,ErrorMessage ="The maximum length is 20")]
        [MinLength(5,ErrorMessage ="The minimum length should be 5")]
        public string Category { get; set; }

        [Required]
        [MaxLength(200,ErrorMessage ="The maximum length is 100")]
        [MinLength(10,ErrorMessage ="The minimum length is 10")]
        public string Description { get; set; }
        public string ImageUrl { get; set; }


        [ForeignKey("CategoryNavigation")]
        public int CategoryId { get; set; }
        public Category? CategoryNavigation { get; set; }
        public ICollection<CartProduct>? carts { get; set; }
    }
}
