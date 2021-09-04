using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Payroll_Manager.Areas.Admin.Controllers;
using Payroll_Manager.Storage;

namespace Payroll_Manager.Infrastructure
{
    [HtmlTargetElement("ul", TagStructure = TagStructure.NormalOrSelfClosing, Attributes = "document-id, item-class")]
    public class ListTagHelper : TagHelper
    {
        private readonly FileStorage _storage;
        private readonly IUrlHelperFactory _urlHelperFactory;
        
        public ListTagHelper(FileStorage storage, IUrlHelperFactory urlHelperFactory)
        {
            _storage = storage;
            _urlHelperFactory = urlHelperFactory;
        }

        public int DocumentId { get; set; }
        public string ItemClass { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            foreach (FileItemInfo info in _storage[DocumentId].GetFileInfos())
            {
                TagBuilder li = new TagBuilder("li");

                li.Attributes.Add("class", ItemClass);

                TagBuilder link = new TagBuilder("a");

                link.Attributes["href"] = urlHelper.Action(nameof(DocumentsController.Attachment), new {docId = DocumentId,  attId = info.Id});
                link.Attributes["target"] = "_blank";

                link.InnerHtml.Append($"Tập tin: {info.FileName}");
                li.InnerHtml.AppendHtml(link);

                output.Content.AppendHtml(li);
            }
        }
    }
}
