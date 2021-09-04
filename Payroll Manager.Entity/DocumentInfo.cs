using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity
{
    public class DocumentInfo : IEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime SeeBefore { get; set; }
        public string Caption { get; set; }
        public string Note { get; set; }
        public String Employee { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Familiarization> Familiarizations { get; set; }

        public DocumentInfo()
        {
            Familiarizations = new List<Familiarization>();
        }

        public override string ToString()
        {
            return $"Tài liệu ID={Id} [#{Number} от {Date:dd.MM.yyyy}]";
        }
    }
}
