using System.ComponentModel.DataAnnotations;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Dữ liệu sai")]
        [Display(Name = "Tên Role")]
        public string RoleName { get; set; }
    }
}
