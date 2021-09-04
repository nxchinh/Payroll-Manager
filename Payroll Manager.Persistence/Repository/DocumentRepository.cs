using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence.Repository.Abstraction;

namespace Payroll_Manager.Persistence.Repository
{
    public class DocumentRepository : RepositoryBase<DocumentInfo>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<DocumentInfo> GetAll()
        {
            return FindAll().AsNoTracking().OrderByDescending(d => d.SeeBefore).ToList();
        }

        public List<DocumentInfo> GetList()
        {
            List<DocumentInfo> paging = GetAll().ToList();

            return paging;
        }

        public DocumentInfo FirstOrDefault(int id)
        {
            return FindByCondition(d => d.Id == id)
                   .Include(d => d.Familiarizations)
                   .AsNoTracking()
                   .FirstOrDefault();
        }
    }
}
