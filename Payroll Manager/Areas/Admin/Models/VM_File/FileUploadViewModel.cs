using System.Collections.Generic;
using Payroll_Manager.Areas.Admin.Models.VM_Event;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Areas.Admin.Models.VM_File
{
    public class FileUploadViewModel
    {
        public List<FileOnFileSystemModel> FilesOnFileSystem { get; set; }

        public List<FileOnDatabaseModel> FilesOnDatabase { get; set; }

        public List<EventImage> EventFile { get; set; }

        public EventFileViewModel ChooseEvent { get; set; }
    }
}
