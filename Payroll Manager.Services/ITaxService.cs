using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll_Manager.Services
{
    public interface ITaxService
    {
        decimal TaxAmount(decimal totalAmount);
    }
}
