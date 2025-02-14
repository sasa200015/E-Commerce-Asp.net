using E_Commerce.Models;

namespace E_Commerce.Interfaces
{
    public interface ICategory
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public void Add(Category Category);
        public void Update(Category Category);
        public void Delete(int id);
        public void Save();
    }
}
