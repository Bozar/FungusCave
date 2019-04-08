using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public interface ISaveLoad
    {
        void Load();

        void Save();
    }

    public class SaveLoad : MonoBehaviour
    {
        private string defaultDirectory;

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
            return LoadXML(fileName, defaultDirectory);
        }

        public void SaveXML(XElement xele, string fileName)
        {
            SaveXML(xele, fileName, defaultDirectory);
        }

        public void SaveXML(XElement xele, string fileName, string directory)
        {
            string path = Path.Combine(directory, fileName);
            xele.Save(path);
        }

        private void Awake()
        {
            defaultDirectory = "Data";
            defaultDirectory = Path.Combine(Directory.GetCurrentDirectory(),
                defaultDirectory);
        }
    }
}
