using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_Address
{
    public class AddressAddViewModel
    {
        public int Id { get; set; }
        public string AId { get; set; }
        [DisplayName("Địa chỉ")]
        public string StreetAddress { get; set; }
        [DisplayName("Mã Căn hộ")]
        public string AptNumber { get; set; }
        [DisplayName("Tên thành phố")]
        public string City { get; set; }
        [DisplayName("Tên quận")]
        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}
