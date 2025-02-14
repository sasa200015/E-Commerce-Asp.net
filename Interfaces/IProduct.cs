using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface  IProduct
    {
        public List<Product> GetAll();
        public Product GetById(int id);
        public void Add(Product product);  
        public void Update(Product product);
        public void Delete(int id);
        public void Save();
        public Task Saveasync();

    }
}
