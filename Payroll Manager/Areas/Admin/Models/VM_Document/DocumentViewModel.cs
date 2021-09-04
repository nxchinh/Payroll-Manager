using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Payroll_Manager.Infrastructure;

namespace Payroll_Manager.Areas.Admin.Models.VM_Document
{
    public class DocumentViewModel
    {
        [DisplayName("Số thứ tự")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])(?=\S+$).{6,20}$", ErrorMessage = "Vui lòng nhập lại")]
        public string Number { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Ngày nhập")]
        public DateTime? Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Không được bỏ trống")]
        [DisplayName("Xem trước")]
        [NotLessCurrentDate]
        public DateTime SeeBefore { get; set; } = DateTime.Today.AddDays(2);

        [Required(ErrorMessage = "Không được bỏ trống")]
        [DisplayName("Tên tài liệu")]
        public string Caption { get; set; }
            
        [DisplayName("Ghi chú")]
        public string Note { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
