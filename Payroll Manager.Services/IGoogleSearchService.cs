using System.Threading.Tasks;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Services
{
    public interface IGoogleSearchService
    {
        Task<SearchResult> Search(string searchUrl, string url);
    }
}
