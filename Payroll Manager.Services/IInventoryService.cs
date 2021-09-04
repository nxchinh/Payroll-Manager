using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface IInventoryService
    {
        DbSet<Inventory> GetInventories();
        IEnumerable<Inventory> GetAll();
        Task CreateAsync(Inventory inventory);
        Inventory GetById(int inventoryId);
        Inventory GetSingle(Expression<Func<Inventory, bool>> predicate);
        IEnumerable<Inventory> Query(Expression<Func<Inventory, bool>> predicate);
        Task UpdateAsync(Inventory inventory);
        Task Delete(int inventoryId);
        IEnumerable<SelectListItem> GetAllInventories();
    }
}
