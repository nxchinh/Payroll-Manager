using System.Threading.Tasks;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Services
{
    public interface IBingSearchService 
    {
        Task<SearchResult> Search(string keywords, string url);
    }
}
