using System.Threading.Tasks;

namespace Payroll_Manager.Services
{
    public interface IStatisticSearchService
    {
        Task<string> SearchByUrl(string searchUrl, string tagUrl);
    }
}
