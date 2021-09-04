using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Event
{
    public class EventFileViewModel
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
