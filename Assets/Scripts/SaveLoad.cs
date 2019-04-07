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
            string directory = "Data";
            string current = Directory.GetCurrentDirectory();

            return LoadXML(fileName, Path.Combine(current, directory));
        }
    }
}
