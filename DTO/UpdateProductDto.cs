using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public double ? Price { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int ? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
