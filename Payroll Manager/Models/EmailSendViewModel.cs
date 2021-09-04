using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll_Manager.Areas.Admin.Models.VM_Inventory;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Models
{
    public class EmailSendViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Quatity { get; set; }
        public string Employeename { get; set; }
        public List<Employee> employee { get; set; }
        public List<Inventory> Inventories { get; set; }
        public int InventoriesId { get; set; }
        public Email _mails { get; set; }
        public InventoryDetailViewModel InventoryDetail { get; set; }
    }
}
