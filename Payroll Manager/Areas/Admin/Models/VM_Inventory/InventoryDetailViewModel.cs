using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_Inventory
{
    public class InventoryDetailViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Giá khuyến mãi")]
        public decimal SalePrice { get; set; }
        public int LuotDat { get; set; }
        public string Image { get; set; }
        public string TenKhachHang { get; set; }
        public string EmailKhachHang { get; set; }
        public string PhoneKhachHang { get; set; }
        public int QuatityKhachHang { get; set; }
    }
}
