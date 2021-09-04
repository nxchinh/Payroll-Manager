using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity
{
    public sealed class Department : BaseEntity
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
