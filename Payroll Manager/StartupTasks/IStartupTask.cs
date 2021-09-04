using System.Threading;
using System.Threading.Tasks;

namespace Payroll_Manager.StartupTasks
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
