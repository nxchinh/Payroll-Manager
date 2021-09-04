using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity
{
    public sealed class Company : BaseEntity
    {
        public Company()
        {
            Departments = new HashSet<Department>();

        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public ICollection<Department> Departments { get; set; }

    }
}
