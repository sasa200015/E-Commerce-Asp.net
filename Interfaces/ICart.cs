using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface ICart
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByIdAsync(int cartId);
        public List<Cart> GetByUserId(string userId);

    }
}
