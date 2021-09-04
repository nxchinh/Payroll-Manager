using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using Payroll_Manager.Entity.Abstract;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Entity
{
    public class Address:BaseEntity
    {
        public int Id { get; set; }
        public string AId { get; set; } 
        public string StreetAddress { get; set; }

        public string AptNumber { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
        [NotMapped]
        public string FullAddress
        {
            get
            {
                if (string.IsNullOrEmpty(State))
                {
                    State = "no value";
                }
                if (string.IsNullOrEmpty(StreetAddress))
                {
                    StreetAddress = "no value";
                }
                if (string.IsNullOrEmpty(City))
                {
                    City = "no value";
                }
                if (string.IsNullOrEmpty(AptNumber))
                {
                    AptNumber = "no value";
                }
                return $"Street: {StreetAddress} - City: {City} - Apt: {AptNumber} - State: {State}";
            }
        }

        public virtual IEnumerable<Employee> Employees { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
