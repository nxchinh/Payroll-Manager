using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Payroll_Manager.Storage
{
    public class DocumentStorage
    {
        private readonly string _path;
        private readonly StorageInfo _info;

        public DocumentStorage(string path)
        {
            _path = path;
            _info = StorageInfo.Open(path);
        }

        public void AddFile(FileItem file)
        {
            FileItemInfo info = new FileItemInfo
            {
                Id =file.Id,
                FileName = file.FileName,
                MimeType = file.MimeType
            };

            _info.Files.Add(info);
            
            using (FileStream fileStream = new FileStream(Path.Combine(_path, file.FileName), FileMode.Create, FileAccess.Write))
            {
                file.Stream.WriteTo(fileStream);
            }

            _info.Save();
        }

        public void DeleteFile(Guid idFile)
        {
            FileItemInfo file = GetFileInfo(idFile);

            if (file == null) return;

            _info.Files.Remove(file);
            
            File.Delete(Path.Combine(_path, file.FileName));

            _info.Save();
        }

        public IEnumerable<FileItemInfo> GetFileInfos()
        {
            return _info.Files;
        }

        public FileItemInfo GetFileInfo(Guid idFile)
        {
            return GetFileInfos().FirstOrDefault(f => f.Id == idFile);
        }

        public IEnumerable<Guid> GetGuids()
        {
           return _info.Files.Select(f => f.Id);
        }

        public FileItem GetFile(Guid fileId)
        {
            FileItemInfo info = GetFileInfo(fileId);

            if (info == null) return null;

            FileItem item = new FileItem(info.Id)
            {
                FileName = info.FileName,
                MimeType = info.MimeType
            };

            using (FileStream fileStream = new FileStream(Path.Combine(_path, info.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileStream.CopyTo(item.Stream);
            }

            item.Stream.Position = 0;

            return item;
        }

        public void Clear()
        {
            Directory.Delete(_path, true);
        }
    }
}
