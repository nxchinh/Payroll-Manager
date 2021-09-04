using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [Display(Name = "Tên role")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
