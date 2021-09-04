using System.Collections.Generic;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_Address
{
    public class AddressIndexViewModel
    {
        public int AddressId { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
