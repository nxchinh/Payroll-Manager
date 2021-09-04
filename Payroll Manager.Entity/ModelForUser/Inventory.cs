using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity.ModelForUser
{
    public sealed class Inventory : BaseEntity
    {
        public Inventory()
        {
            this.PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
            this.SaleItems = new HashSet<SaleItem>();
        }
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public int Active { get; set; }
        public int SaleQuantity { get; set; }
        public int RestQuantity { get; set; }
        public int LuotDat { get; set; }
        [NotMapped]
        public string DropdownStr => Name + " [ID: " + ProductId.ToString() + "]";
        [NotMapped]
        public string FullAd => $"Name: {Name} - Giá: {NetPrice} - Giá KM: {SalePrice}";
        public string FullAdv => $"{Name} - Giá: {NetPrice} - Giá KM: {SalePrice}";

        public Category Category { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
    }
}
