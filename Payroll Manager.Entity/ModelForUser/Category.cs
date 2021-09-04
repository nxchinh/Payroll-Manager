using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public sealed class Category : BaseEntity
    {
        public Category()
        {
            this.Inventories = new HashSet<Inventory>();
        }
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Active { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
