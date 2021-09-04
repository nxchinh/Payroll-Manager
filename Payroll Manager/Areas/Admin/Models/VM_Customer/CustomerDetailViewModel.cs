using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_Customer
{
    public class CustomerDetailViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public int Active { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
