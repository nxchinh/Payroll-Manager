using System;
using System.ComponentModel.DataAnnotations;

namespace Payroll_Manager.Infrastructure
{
    public class NotLessCurrentDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime dateTime = (DateTime) value;
                
                if (dateTime > DateTime.Now)
                    return true;
                else
                    ErrorMessage = "The set date cannot be less than the current";
            }
            return false;
        }
    }
}
