using System;
using System.Collections.Generic;
using System.Text;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Entity
{
    public class TaxYear : BaseEntity
    {
        public int Id { get; set; }
        public string YearOfTax { get; set; }
    }
}