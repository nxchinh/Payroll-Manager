using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep
{
    public class SelectListItemViewModel
    {
        public List<SelectListItem> SelectListWorker { get; set; }


        public List<SelectListItem> SelectListCurrentWorkers { get; set; }


        public List<SelectListItem> SelectListShops { get; set; }
        public List<SelectListItem> SelectListCompanies { get; set; }

        public Company Company { get; set; }
    }
}
