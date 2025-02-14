using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class CartProduct
    {
        public int Id { get; set; }

        [ForeignKey("cart")]
        public int CartId { get; set; }
        public Cart cart { get; set; }

        [ForeignKey("product")]
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int Quantity { get; set; }
    }
}
