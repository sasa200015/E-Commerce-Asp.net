using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }
        public User? user { get; set; }
        public ICollection<CartProduct>? products { get; set; }
    }
}
