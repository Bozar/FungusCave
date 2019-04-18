using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.SaveLoadData
{
    public interface ISaveLoadBinary
    {
        void Load(IDataTemplate data);

        void Save(out IDataTemplate data);
    }

    public interface ISaveLoadXML
    {
        void Load();

        void Save();
    }

    public class SaveLoadFile : MonoBehaviour
    {
        public string BinaryDirectory { get; private set; }

        public string XmlDirectory { get; private set; }

        public void BackupBinary(string fileName, string directory)
        {
            string source = Path.Combine(directory, fileName);
            string target = Path.Combine(directory, fileName + ".bak");

            if (File.Exists(source))
            {
                File.Copy(source, target, true);
            }
        }

        public void BackupBinary(string fileName)
        {
            BackupBinary(fileName, BinaryDirectory);
        }

        public void DeleteBinary(string fileName, string directory)
        {
            string path = Path.Combine(directory, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeleteBinary(string fileName)
        {
            DeleteBinary(fileName, BinaryDirectory);
        }

        public IDataTemplate[] LoadBinary(string fileName, string directory)
        {
            IFormatter bf = new BinaryFormatter();
            IDataTemplate[] data;
            string path = Path.Combine(directory, fileName);

            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    data = (IDataTemplate[])bf.Deserialize(fs);
                }
                return data;
            }
            throw new FileNotFoundException();
        }

        public IDataTemplate[] LoadBinary(string fileName)
        {
            return LoadBinary(fileName, BinaryDirectory);
        }

        public XElement LoadXML(string fileName, string directory)
        {
            string path = Path.Combine(directory, fileName);

            if (File.Exists(path))
            {
                return XElement.Load(path);
            }
            throw new FileNotFoundException();
        }

        public XElement LoadXML(string fileName)
        {
            return LoadXML(fileName, XmlDirectory);
        }

        public void SaveBinary(IDataTemplate[] data,
            string fileName, string directory)
        {
            IFormatter bf = new BinaryFormatter();
            string path = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (FileStream fs = File.Create(path))
            {
                bf.Serialize(fs, data);
            }
        }

        public void SaveBinary(IDataTemplate[] data, string fileName)
        {
            SaveBinary(data, fileName, BinaryDirectory);
        }

        public void SaveXML(XElement xele, string fileName)
        {
            SaveXML(xele, fileName, XmlDirectory);
        }

        public void SaveXML(XElement xele, string fileName, string directory)
        {
            string path = Path.Combine(directory, fileName);
            xele.Save(path);
        }

        private void Awake()
        {
            XmlDirectory = "Data";
            XmlDirectory = Path.Combine(Directory.GetCurrentDirectory(),
                XmlDirectory);

            BinaryDirectory = "Save";
            BinaryDirectory = Path.Combine(Directory.GetCurrentDirectory(),
                BinaryDirectory);
        }
    }
}
