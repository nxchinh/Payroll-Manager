using System.ComponentModel.DataAnnotations;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        [Display(Name = "Tên bộ phận")]
        public string Name { get; set; }

        [Display(Name = "Tên Công ty")]
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
