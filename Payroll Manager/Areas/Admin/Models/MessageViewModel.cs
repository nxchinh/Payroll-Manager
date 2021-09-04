using System;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Models
{
    public class MessageViewModel
    {
        public ApplicationUser user { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }
}
