namespace Payroll_Manager.Persistence.Repository.Abstraction
{
    public interface IDataAccess
    {
        IDocumentRepository Document { get; }
        IFamiliarizationRepository Familiarization { get; }
        void Save();
    }
}
