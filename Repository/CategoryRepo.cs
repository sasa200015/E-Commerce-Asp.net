using E_Commerce.Interfaces;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CategoryRepo:ICategory
    {
        private readonly ProjectContext context;

        public CategoryRepo(ProjectContext context)
        {
            this.context = context;
        }

       public void Add(Category Category)
        {
            context.Category.Add(Category);
        }

        public void Delete(int id)
        {
           context.Category.Remove(GetById(id));
        }

        public List<Category> GetAll()
        {
           return context.Category.ToList();
        }

        public Category GetById(int id)
        {
           Category category=context.Category.Include(x=>x.Products).SingleOrDefault(x=>x.Id==id);
           return category;
        }


        public void Update(Category Category)
        {
            context.Category.Update(Category);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
