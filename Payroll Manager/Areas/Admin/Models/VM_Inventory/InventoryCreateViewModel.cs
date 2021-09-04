using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Inventory
{
    public class InventoryCreateViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Ghi chú")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá tiền")]
        public decimal NetPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá khuyến mãi")]
        public decimal SalePrice { get; set; }
        [Display(Name = "Số lượng nhập")]
        public int Quantity { get; set; }
        [Display(Name = "Số lượng đã bán")]
        public int SaleQuantity { get; set; }
        [Display(Name = "Số lượng tồn")]
        public int RestQuantity { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Hình ảnh")]
        public IFormFile Image { get; set; }
        public int Active { get; set; }
        [NotMapped]
        public string DropdownStr => Name + " [ID: " + ProductId.ToString() + "]";
        public Category Category { get; set; }
        [Display(Name = "Hình ảnh cũ")]
        public string ImageURL { get; set; }
    }
}
