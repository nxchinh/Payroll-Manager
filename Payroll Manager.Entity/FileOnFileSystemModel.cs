using System.ComponentModel.DataAnnotations.Schema;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Entity
{
    public class FileOnFileSystemModel : FileModel
    {
        public string FilePath { get; set; }
    }
}
