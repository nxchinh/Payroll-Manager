using AutoMapper;
using Payroll_Manager.Areas.Admin.Models.VM_Document;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.Profiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<DocumentInfo, DocumentViewModel>().ReverseMap();
        }
    }
}
