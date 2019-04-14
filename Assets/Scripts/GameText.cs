﻿using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour, IGetData
    {
        private XElement xFile;

        public XElement GetData<T, U>(T t, U u)
        {
            string myLang = GetComponent<GameSetting>().UserLanguage;
            XElement xele = xFile.Element(t.ToString()).Element(u.ToString());

            if (string.IsNullOrEmpty((string)xele.Element(myLang)))
            {
                myLang = GetComponent<GameSetting>().DefaultLanguage;
            }
            return xele.Element(myLang);
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

        public int GetIntData<T, U>(T t, U u)
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

        public string GetStringData<T, U>(T t, U u)
        {
            string text = GetData(t, u).Value.ToString();
            text = text.Replace(@"\n", "\n");

            return text;
        }

        private void Start()
        {
            xFile = GetComponent<SaveLoad>().LoadXML("text.xml");
        }
    }
}
