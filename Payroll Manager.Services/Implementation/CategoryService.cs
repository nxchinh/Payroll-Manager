using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services.Implementation
{
    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<Category> GetCategories() => _context.Categories;

        public IEnumerable<Category> GetAll() => _context.Categories.AsNoTracking().OrderBy(emp => emp.CategoryName);

        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public Category GetSingle(Expression<Func<Category, bool>> predicate) => _context.Set<Category>().FirstOrDefault(predicate);

        public IEnumerable<Category> Query(Expression<Func<Category, bool>> predicate) => _context.Set<Category>().Where(predicate);
        public Category GetById(int categoryId) => _context.Categories.FirstOrDefault(predicate: e => e.CategoryId == categoryId);

        public async Task UpdateAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int categoryId)
        {
            var category = GetById(categoryId);
            _context.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
