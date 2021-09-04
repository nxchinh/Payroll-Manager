using System;

namespace Payroll_Manager.Entity
{
    public partial class Chat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
