using Microsoft.EntityFrameworkCore;
using NetPCTask.Data;
using NetPCTask.Models;

namespace NetPCTask.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _appDbContext;

        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Category GetCategory(int id)
        {
            return _appDbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _appDbContext.Categories.ToList();
        }

        public bool CreateCategory(Category category)
        {
            _appDbContext.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _appDbContext.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
           _appDbContext.Remove(category);
            return Save();
        }

        private bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CategoryExists(int id)
        {
            return _appDbContext.Categories.Any(c => c.Id == id);
        }
    }
}
