using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll_Manager.Services
{
    public interface INationalInsuranceContributionService
    {
        decimal NIContribution(decimal totalAmount);
    }
}
