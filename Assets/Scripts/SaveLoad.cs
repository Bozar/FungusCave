using System.IO;
using UnityEngine;

namespace Fungus.GameSystem
{
    public interface ISaveLoad
    {
        void Load();

        void Save();
    }

    public class GameData
    {
        public bool IsWizard;
        public int Seed;
    }

    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/
    public class SaveLoad : MonoBehaviour
    {
        public GameData SaveFile { get; private set; }

        private void Awake()
        {
            string fileName = "test.xml";
            string[] path = new string[] { "Data", "Build" };
            string current = Directory.GetCurrentDirectory();

            for (int i = 0; i < path.Length; i++)
            {
                path[i] = Path.Combine(current, path[i]);
                path[i] = Path.Combine(path[i], fileName);
            }

            WriteXML(path);
            ReadXML(path);
        }

        private void ReadXML(string[] path)
        {
            var reader = new System.Xml.Serialization.XmlSerializer(
                typeof(GameData));

            foreach (string p in path)
            {
                if (File.Exists(p))
                {
                    StreamReader sr = new StreamReader(p);
                    SaveFile = (GameData)reader.Deserialize(sr);
                    sr.Close();

                    return;
                }
            }
        }

        private void WriteXML(string[] path)
        {
            GameData gd = new GameData
            {
                Seed = 0,
                IsWizard = false
            };

            string newPath = path[0];
            bool pathExists = false;

            foreach (string p in path)
            {
                if (File.Exists(p))
                {
                    return;
                }
                if (Directory.Exists(Path.GetDirectoryName(p)))
                {
                    newPath = p;
                    pathExists = true;
                    break;
                }
            }
            if (!pathExists)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            }

            var writer = new System.Xml.Serialization.XmlSerializer(
                typeof(GameData));
            StreamWriter sw = new StreamWriter(newPath);

            writer.Serialize(sw, gd);
            sw.Close();
        }
    }
}
