using System.Collections.Generic;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Persistence.Repository.Abstraction
{
    public interface IDocumentRepository : IRepositoryBase<DocumentInfo>
    {
        IEnumerable<DocumentInfo> GetAll();

        List<DocumentInfo> GetList();

        DocumentInfo FirstOrDefault(int id);
    }
}
