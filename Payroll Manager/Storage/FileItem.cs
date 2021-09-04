using System;
using System.IO;

namespace Payroll_Manager.Storage
{
    public class FileItem
    {
        public FileItem() : this(Guid.NewGuid())
        {
            Stream = new MemoryStream();
        }

        public FileItem(Guid id)
        {
            Id = id;
            Stream = new MemoryStream();
        }

        public Guid Id { get; } 
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public MemoryStream Stream { get; set; }
    }
}
