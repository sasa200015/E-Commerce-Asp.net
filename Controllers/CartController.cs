using E_Commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICart cartRepo;

        public CartController(ICart cartRepo)
        {
            this.cartRepo = cartRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await cartRepo.GetAllCartsAsync();

            var result = carts.Select(c => new
            {
                id = c.Id,
                userId = c.UserId,
                products = c.products.Select(p => new
                {
                    productId = p.ProductId,
                    quantity = p.Quantity
                }).ToList()
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, result);
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var cart = await cartRepo.GetCartByIdAsync(Id);

            if (cart == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "This Cart does not Exist");
            }

            var result = new
            {
                id = cart.Id,
                userId = cart.UserId,
                products = cart.products.Select(p => new
                {
                    productId = p.ProductId,
                    quantity = p.Quantity
                }).ToList()
            };


            return StatusCode(StatusCodes.Status200OK, result);
        }
        [HttpGet("User/{UserId:guid}")]
        public async Task<IActionResult> GetByUserId(string UserId)
        {
            var cartUser = cartRepo.GetByUserId(UserId);
            var bigResult = new List<dynamic>();
            if (cartUser != null)
            {
                foreach (var item in cartUser)
                {
                    var cart = await cartRepo.GetCartByIdAsync(item.Id);
                    var result = new
                    {
                        id = cart.Id,
                        userId = cart.UserId,
                        products = cart.products.Select(p => new
                        {
                            productId = p.ProductId,
                            quantity = p.Quantity
                        }).ToList()
                    };
                    bigResult.Add(result);
                }
                return StatusCode(StatusCodes.Status200OK, bigResult);
            }
            return StatusCode(StatusCodes.Status404NotFound, "This Cart does not Exist");
        }

    }
}
