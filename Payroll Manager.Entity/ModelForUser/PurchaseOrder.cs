using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public sealed class PurchaseOrder : BaseEntity
    {
        public PurchaseOrder()
        {
            this.PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
        }
        [Key]
        public int PurchaseOrderId { get; set; }
        public string MaGiaoDich { get; set; }
        public DateTime OrderDate { get; set; }
        [NotMapped]
        public string DateStr => OrderDate.Date.ToShortDateString();
        [NotMapped]
        public int SLBan { get; set; }

        [NotMapped]
        public double TotalPrice { get; set; }
        [NotMapped]
        public bool IsReceived { get; set; }

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }
}
