using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class AttendancesViewModel
    {
        public int AttendanceID { get; set; }
        public string Name { get; set; }
        public DateTime Attendance_Date { get; set; }
        public Boolean Attendance_status { get; set; }
    }
}
