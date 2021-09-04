using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Persistence
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "Dữ liệu nhập bị lỗi")]
        [StringLength(50, ErrorMessage = "Dữ liệu không quá 50 ký tự")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Dữ liệu nhập bị lỗi")]
        [StringLength(50, ErrorMessage = "Dữ liệu không quá 50 ký tự")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => FirstName + " " + LastName;
    }
}
