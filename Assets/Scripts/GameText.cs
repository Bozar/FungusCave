using Fungus.GameSystem.SaveLoadData;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Data
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour, IGetData
    {
        private XElement xFile;

        public XElement GetData<T, U>(T parentNode, U childNode)
        {
            XElement xele = xFile.Element(parentNode.ToString())
                .Element(childNode.ToString());
            string lang = GetComponent<GameSetting>().GetValidLanguage(xele);

            return xele.Element(lang);
        }

        public string GetHelp()
        {
            string[] elements = new string[]
            { "Title", "Normal", "Examine", "Menu" };
            string[] text = new string[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                text[i] = GetStringData("Help", elements[i]);
            }
            return string.Join("\n\n", text);
        }

        public int GetIntData<T, U>(T parentNode, U childNode)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetOpening()
        {
            string scene = GetStringData("Opening", "Scene");
            string modeline = GetStringData("Opening", "Modeline");

            string[] text = new string[] { scene, modeline };
            return text;
        }

        public string GetStringData<T, U>(T parentNode, U childNode)
        {
            string text = GetData(parentNode, childNode).Value.ToString();
            text = text.Replace(@"\n", "\n");

            return text;
        }

        private void Start()
        {
            xFile = GetComponent<SaveLoadFile>().LoadXML("text.xml");
        }
    }
}
