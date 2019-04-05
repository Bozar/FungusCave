using Fungus.Actor;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour
    {
        private string defaultLang;
        private string lang;
        private XElement xText;

        public string GetHelp()
        {
            string[] elements = new string[]
            { "Title", "Normal", "Examine", "Menu" };
            string[] text = new string[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                text[i] = GetText(xText.Element("Help").Element(elements[i]));
            }
            return string.Join("\n\n", text);
        }

        public string GetPowerDescription(PowerTag tag)
        {
            return GetText(xText.Element("PowerDescription").Element(
                tag.ToString()));
        }

        private void Awake()
        {
            defaultLang = "English";
            lang = "English";

            Load();
        }

        private string GetText(XElement xElement)
        {
            string text;
            string myLang = lang;

            if (xElement.Element(lang) == null)
            {
                myLang = defaultLang;
            }

            text = xElement.Element(myLang).Value.ToString();
            text = text.Replace(@"\n", "\n");
            return text;
        }

        private void Load()
        {
            if (xText != null)
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
                xText = XElement.Load(path);
                return;
            }
            throw new FileNotFoundException();
        }
    }
}
