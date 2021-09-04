using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Category
{
    public class CategoryIndexViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
