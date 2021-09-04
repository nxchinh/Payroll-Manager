using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;
using Payroll_Manager.Persistence.Repository.Abstraction;

namespace Payroll_Manager.Infrastructure
{
    [HtmlTargetElement(Attributes = "state-acq-for")]
    public class StateAcqByDocIdTagHelper : TagHelper
    {
        private readonly IDataAccess _dataAccess;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public StateAcqByDocIdTagHelper(IDataAccess dataAccess, IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager)
        {
            _dataAccess = dataAccess;
            _accessor = accessor;
            _userManager = userManager;
        }

        public int StateAcqFor { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            DocumentInfo document = _dataAccess.Document.FirstOrDefault(StateAcqFor);

            if (document == null) return;

            var a = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            //bool isAcq = document.Familiarizations.Any(f => f.Employee == a.FullName);

            //if (!isAcq)
            //{
                if (document.Familiarizations.FirstOrDefault() != null)
                {
                    output.Content.Append("Đã nhận tài liệu");
                }
                else
                {
                    output.Content.Append(document.SeeBefore > DateTime.Now ? "Nhận tài liệu" : "Quá hạn!");
                }

            //}
            //else
            //{
            //    output.Content.Append("Đã nhận tài liệu");
            //}
        }
    }
}
