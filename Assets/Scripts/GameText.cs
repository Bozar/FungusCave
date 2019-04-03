using Fungus.Actor;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour
    {
        private XElement gameText;

        public string GetPowerDescription(PowerTag tag)
        {
            string text = gameText.Element("Test").Value;
            text = text.Replace(@"\n", "\n");

            return text;
        }

        private void Awake()
        {
            Load();
        }

        private void Load()
        {
            if (gameText != null)
            {
                return;
            }

            string fileName = "text.xml";
            string path = "Data";
            string current = Directory.GetCurrentDirectory();

            path = Path.Combine(current, path);
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                gameText = XElement.Load(path);
                return;
            }
            throw new FileNotFoundException();
        }
    }
}
