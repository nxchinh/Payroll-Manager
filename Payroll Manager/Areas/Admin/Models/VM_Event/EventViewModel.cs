using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_Event
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public int masanpham { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public String TenSuKien { get; set; }
        public Boolean Active { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime ThoiGianBatDau { get; set; }
    }
}
