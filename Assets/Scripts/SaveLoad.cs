using System.IO;
using UnityEngine;

// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-read-object-data-from-an-xml-file
public class SaveLoad : MonoBehaviour
{
    public int mySeed;
    public Book overview;
    private StreamReader file;
    private string myTitie;
    private bool writeFile;

    private void ReadXML()
    {
        writeFile = false;
        var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "test.xml";
        var altPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar
            + "Build" + Path.DirectorySeparatorChar
            + "test.xml";

        if (writeFile)
        {
            // First write something so that there is something to read ...
            var b = new Book { title = "Serialization Overview", seed = 12345 };
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(Book));
            var wfile = new StreamWriter(path);
            //var wfile = new StreamWriter(@"c:\temp\SerializationOverview.xml");
            writer.Serialize(wfile, b);
            wfile.Close();
        }

        // Now we can read the serialized book ...
        System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(Book));

        if (File.Exists(path))
        {
            file = new StreamReader(path);
        }
        else
        {
            file = new StreamReader(altPath);
        }

        //@"c:\temp\SerializationOverview.xml");
        overview = (Book)reader.Deserialize(file);
        file.Close();

        myTitie = overview.title;
        mySeed = overview.seed;
    }

    private void Start()
    {
        ReadXML();
        Debug.Log(myTitie);
        Debug.Log(mySeed);

        //Debug.Log(overview.title);
        //Debug.Log(overview.seed);
    }

    public class Book
    {
        public int seed;
        public string title;
    }
}
