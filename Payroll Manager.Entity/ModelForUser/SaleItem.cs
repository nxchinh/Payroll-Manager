using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public class SaleItem : BaseEntity
    {
        [Key]
        public int SaleItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Returned { get; set; }
        public int SaleId { get; set; }
        [NotMapped]
        [Display(Name = "Sale Price")]
        public double TotalSiPrice { get; set; }
        [Display(Name = "Items")]
        [NotMapped]
        public int TotalSI { get; set; }
        [NotMapped]
        public int ActiveQuantity => TotalSI - Returned;

        public virtual Inventory Inventory { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
