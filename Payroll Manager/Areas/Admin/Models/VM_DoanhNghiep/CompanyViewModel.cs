using System.ComponentModel.DataAnnotations;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        [StringLength(200)]
        [Display(Name = "Tên")]
        public string Name { get; set; }
    }
}
