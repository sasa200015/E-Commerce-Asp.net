using E_Commerce.Interfaces;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CartRepo : ICart
    {
        private readonly ProjectContext context;

        public CartRepo(ProjectContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await context.Cart
                .Include(c => c.products)
                .ThenInclude(cp => cp.product)
                .ToListAsync();
        }


        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await context.Cart
                .Include(c => c.products)
                .ThenInclude(cp => cp.product)
                .Where(c => c.Id == cartId)
                .FirstOrDefaultAsync();
        }

        public List<Cart> GetByUserId(string userId)
        {
            return context.Cart.Where(c=>c.UserId==userId).ToList();
        }
    }
}
