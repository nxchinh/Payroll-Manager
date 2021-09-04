using Payroll_Manager.Entity;
using Payroll_Manager.Persistence.Repository.Abstraction;

namespace Payroll_Manager.Persistence.Repository
{
    public class FamiliarizationRepository : RepositoryBase<Familiarization>, IFamiliarizationRepository
    {
        public FamiliarizationRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
