using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models
{
    internal static class CachingExtensions
    {
        internal static string AllCacheKeyEmployee => "Empoyee/Index";
        internal static string ToCacheKeyEmployee(this int id) => $"Empoyee/{id}";
    }
}
