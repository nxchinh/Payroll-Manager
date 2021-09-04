using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Entity
{
    public class EventImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FilePath { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
