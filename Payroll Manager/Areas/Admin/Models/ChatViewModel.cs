using System.Collections.Generic;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Models
{
    public class ChatViewModel
    {
        public string UserId { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
