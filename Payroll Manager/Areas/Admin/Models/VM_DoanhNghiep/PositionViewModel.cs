using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class PositionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa daxil edin")]
        [StringLength(200)]
        [Display(Name = "Vəzifə adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zəhmət olmasa daxil edin")]
        [Display(Name = "Maaş")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Display(Name = "Şirkət")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "Depatament")]
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
