using E_Commerce.DTO;
using E_Commerce.Interfaces;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory categoryRepo;

        public CategoryController(ICategory categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories =categoryRepo.GetAll();
            var result = categories.Select(c => new
            {
                id = c.Id,
                name = c.Name,
            }).ToList();
            return StatusCode(StatusCodes.Status200OK, result);
        }
        [HttpGet("{Id:int}")]
        public IActionResult Get(int Id)
        {
            Category category = categoryRepo.GetById(Id);
            if (category != null)
            {
                List<ProductsAtCategory> products = new List<ProductsAtCategory>();
                foreach(var item in category.Products)
                {
                    ProductsAtCategory product1=new ProductsAtCategory();
                    product1.Title = item.Title;
                    product1.Price = item.Price;
                    product1.Description= item.Description;
                    product1.categoryId = item.CategoryId;
                    product1.ProductId = item.Id;
                    product1.CategoryName = category.Name;
                    products.Add(product1);
                }
                return StatusCode(StatusCodes.Status200OK, products);
            }
            return StatusCode(StatusCodes.Status404NotFound, "This category does not exist ");

        }
    }
}
