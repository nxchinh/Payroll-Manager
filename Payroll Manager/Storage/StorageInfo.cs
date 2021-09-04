using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Payroll_Manager.Storage
{
    [XmlRoot("Storage")]
    public class StorageInfo
    {
        private const string FILE = "storage.xml";

        [XmlArray("Files")]
        [XmlArrayItem("File")]
        public List<FileItemInfo> Files { get; set; }
        
        [XmlIgnore]
        public string FileName { get; set; }
        
        [XmlIgnore]
        public FileItemInfo this[Guid fileId] => Files.FirstOrDefault(f => f.Id == fileId);

        public StorageInfo()
        {
            Files = new List<FileItemInfo>();
        }

        public static StorageInfo Open(string root)
        {
            string storage = Path.Combine(root, FILE);
            
            if (!File.Exists(storage)) return new StorageInfo {FileName = storage};

            XmlSerializer serializer = new XmlSerializer(typeof(StorageInfo));

            using (StreamReader reader = new StreamReader(storage))
            {
                return (StorageInfo)serializer.Deserialize(reader);
            }
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StorageInfo));

            using (StreamWriter writer = File.CreateText(FileName))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}