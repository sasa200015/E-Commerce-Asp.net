using E_Commerce.Interfaces;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class ProductRepo : IProduct
    {
        private readonly ProjectContext context;

        public ProductRepo(ProjectContext context)
        {
            this.context = context;
        }

        public void Add(Product product)
        {
            context.Product.Add(product);
        }

        public void Delete(int id)
        {
            Product product = GetById(id);
            context.Product.Remove(product);
        }

        public List<Product> GetAll()
        {
            return context.Product.ToList();
        }

        public Product GetById(int id)
        {
            return context.Product.FirstOrDefault(x => x.Id == id);
        }


        public void Update(Product product)
        {
            context.Product.Update(product);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public async Task Saveasync()
        {
           await context.SaveChangesAsync();
        }
    }
}
