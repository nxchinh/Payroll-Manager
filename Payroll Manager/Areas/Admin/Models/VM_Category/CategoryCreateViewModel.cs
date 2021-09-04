using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Category
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Tên Loại")]
        [MaxLength(60, ErrorMessage = "Tên loại dưới 60 ký tự")]
        [MinLength(3, ErrorMessage = "Tên loại trên 3 ký tự")]
        public string CategoryName { get; set; }
        [Display(Name = "Hoạt động")]
        public int Active { get; set; }
        public List<IFormFile> CatProfile { get; set; }
    }
}
