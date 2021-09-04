using Payroll_Manager.Persistence.Repository.Abstraction;

namespace Payroll_Manager.Persistence.Repository
{
    public class DataAccess : IDataAccess
    {
        private readonly ApplicationDbContext _repositoryContext;
        private IDocumentRepository _document;
        private IFamiliarizationRepository _familiarization;

        public DataAccess(ApplicationDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IDocumentRepository Document => _document ??= new DocumentRepository(_repositoryContext);
        public IFamiliarizationRepository Familiarization => _familiarization ??= new FamiliarizationRepository(_repositoryContext);

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
