using System;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_Employee
{
    public class EmployeeDetailViewModel
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public Department Department { get; set; }
        public int AddressId { get; set; }
        public int DepId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + (string.IsNullOrEmpty(MiddleName) ? " " : (" " + (char?)MiddleName[0] + ". ").ToUpper()) + LastName;
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateJoined { get; set; }
        public string Phone { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string NationalInsuranceNo { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StudentLoan StudentLoan { get; set; }
        public UnionMember UnionMember { get; set; }
        //public string City { get; set; }
        //public string Postcode { get; set; }
    }
}
