using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface ICategoryService
    {
        DbSet<Category> GetCategories();
        IEnumerable<Category> GetAll();
        Task CreateAsync(Category category);
        Category GetById(int categoryId);
        Category GetSingle(Expression<Func<Category, bool>> predicate);
        IEnumerable<Category> Query(Expression<Func<Category, bool>> predicate);
        Task UpdateAsync(Category category);
        Task Delete(int categoryId);
    }
}
