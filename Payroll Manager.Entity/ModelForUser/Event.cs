using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payroll_Manager.Entity.ModelForUser
{
    [Table("SuKien")]
    public class Event
    {
        public int Id { get; set; }
        public String TenSuKien { get; set; }
        public Boolean Active { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime ThoiGianBatDau { get; set; }
        public ICollection<EventImage> Inventories { get; set; }
    }
}
