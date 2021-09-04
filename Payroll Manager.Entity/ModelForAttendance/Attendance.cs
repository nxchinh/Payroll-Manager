using System;

namespace Payroll_Manager.Entity.ModelForAttendance
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public string Name { get; set; }
        public DateTime Attendance_Date { get; set; }
        public Boolean Attendance_status { get; set; }
        public Employee Employee { get; set; }

        public Users Users { get; set; }
    }
}
