using Fungus.Actor;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour
    {
        private string error;
        private XElement gameText;

        public string GetHelp()
        {
            XElement xLang = gameText.Element("English");
            XElement xHelp = xLang.Element("Help");
            string text;

            if (xHelp == null)
            {
                text = error;
            }
            else
            {
                text = xHelp.Value.ToString();
                text = text.Replace(@"\n", "\n");
            }
            return text;
        }

        public string GetPowerDescription(PowerTag tag)
        {
            XElement xLang = gameText.Element("English");
            XElement xPower = xLang.Element("PowerDescription")
                .Element(tag.ToString());
            string text;

            if (xPower == null)
            {
                text = error;
            }
            else
            {
                text = xPower.Value.ToString();
                text = text.Replace(@"\n", "\n");
            }
            return text;
        }

        private void Awake()
        {
            error = "INVALID TEXT";
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
