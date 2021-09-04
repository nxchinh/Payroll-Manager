using System;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity
{
    public class Familiarization : IEntity
    {
        public int Id { get; set; }
        public DocumentInfo Document { get; set; }
        public String Employee { get; set; }
        public DateTime DateTime { get; set; }
    }
}
