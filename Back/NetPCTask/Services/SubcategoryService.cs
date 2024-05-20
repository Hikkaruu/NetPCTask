using Microsoft.EntityFrameworkCore;
using NetPCTask.Data;
using NetPCTask.Dto;
using NetPCTask.Models;

namespace NetPCTask.Services
{
    public class SubcategoryService
    {
        private readonly AppDbContext _appDbContext;

        public SubcategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Subcategory GetSubcategory(int id)
        {
            return _appDbContext.Subcategories.FirstOrDefault(s => s.Id == id);
        }

        public Subcategory GetSubcategoryByName(string name)
        {
            return _appDbContext.Subcategories.FirstOrDefault(s => s.Name == name);
        }

        public ICollection<Subcategory> GetSubcategories()
        {
            return _appDbContext.Subcategories.ToList();
        }

        public bool CreateSubcategory(Subcategory subcategory)
        {
            _appDbContext.Add(subcategory);
            return Save();
        }

        public bool UpdateSubcategory(Subcategory subcategory)
        {
            _appDbContext.Update(subcategory);
            return Save();
        }

        public bool DeleteSubcategory(Subcategory subcategory)
        {
            _appDbContext.Remove(subcategory);
            return Save();
        }

        private bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SubcategoryExists(int id)
        {
            return _appDbContext.Subcategories.Any(s => s.Id == id);
        }

        public async Task<List<Subcategory>> GetSubcategoriesByCategory(int categoryId)
        {
            try
            {
                var subcategories = await _appDbContext.Subcategories
                    .Where(s => s.CategoryId == categoryId)
                    .ToListAsync();
                return subcategories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching subcategories: {ex.Message}");
            }
        }

        
    }
}
