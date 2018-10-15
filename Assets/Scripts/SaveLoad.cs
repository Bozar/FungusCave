using System.IO;
using UnityEngine;

// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/
public class SaveLoad : MonoBehaviour
{
    private string altPath;
    private StreamReader file;
    private string fileName;
    private string path;
    private StreamWriter wfile;

    public GameData SaveFile { get; private set; }

    private void Awake()
    {
        fileName = "test.xml";

        path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        altPath = Path.Combine(Directory.GetCurrentDirectory(), "Build");
        altPath = Path.Combine(altPath, fileName);

        if (!File.Exists(path) && !File.Exists(altPath))
        {
            WriteXML();
        }

        ReadXML();
    }

    private void ReadXML()
    {
        var reader
            = new System.Xml.Serialization.XmlSerializer(typeof(GameData));

        if (File.Exists(path))
        {
            file = new StreamReader(path);
        }
        else if (File.Exists(altPath))
        {
            file = new StreamReader(altPath);
        }
        else
        {
            return;
        }

        SaveFile = (GameData)reader.Deserialize(file);
        file.Close();
    }

    private void WriteXML()
    {
        var data = new GameData { Seed = 0 };
        var writer
            = new System.Xml.Serialization.XmlSerializer(typeof(GameData));
        wfile = new StreamWriter(path);

        writer.Serialize(wfile, data);
        wfile.Close();
    }

    public class GameData
    {
        public int Seed;
    }
}
