using System.ComponentModel.DataAnnotations;
using E_Commerce.Models;

namespace E_Commerce.DTO
{
    public class ProductsAtCategory
    {
        public int categoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

    }
}
