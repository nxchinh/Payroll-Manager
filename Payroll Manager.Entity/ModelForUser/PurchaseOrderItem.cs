using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public class PurchaseOrderItem : BaseEntity
    {
        [Key]
        public int PoItemId { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        public int Received { get; set; }
        public int PurchaseOrderId { get; set; }
        [NotMapped]
        public double TotalPrice { get; set; }
        [NotMapped]
        public int QtyReturned => Quantity - Received;


        public virtual Inventory Inventory { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}
