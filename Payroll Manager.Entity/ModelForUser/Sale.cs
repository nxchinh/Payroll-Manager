using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public sealed class Sale : BaseEntity
    {
        public Sale()
        {
            this.SaleItems = new HashSet<SaleItem>();
        }
        [Key]
        public int SaleId { get; set; }
        public int? CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime SaleDate { get; set; }
        [NotMapped]
        public string SaleDateString => SaleDate.Date.ToShortDateString();

        [NotMapped]
        [Display(Name = "Total Price")]
        public double TotalSalePrice { get; set; }
        [NotMapped]
        [Display(Name = "Total Items")]
        public int TotalSaleItems { get; set; }
        [NotMapped]
        public int Status { get; set; }
        [NotMapped]
        public string HoatDong { get; set; }
        [NotMapped]
        public int items { get; set; }
        [NotMapped]
        public double price { get; set; }
        [NotMapped]
        public string TenNhanVien { get; set; }
        [NotMapped]
        public string TenKhachHang { get; set; }

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
    }
}
