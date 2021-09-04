using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_Employee
{
    public class EmployeeIndexViewModel
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + (string.IsNullOrEmpty(MiddleName) ? " " : (" " + (char?)MiddleName[0] + ". ").ToUpper()) + LastName;
        public int DepartmentId { get; set; }
        public Department department { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
        public string Designation { get; set; }
        public Boolean Temporary_status { get; set; }
        [NotMapped]
        public bool ConditionaValue { get; set; }
    }
}
