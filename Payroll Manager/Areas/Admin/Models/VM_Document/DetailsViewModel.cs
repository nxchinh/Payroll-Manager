using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_Document
{
    public class DetailsViewModel
    {
        public DocumentInfo Document { get; set; }
        public bool СurrentUserIsFamiliarized { get; set; }
        public bool Check { get; set; }
    }
}
