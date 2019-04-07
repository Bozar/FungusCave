﻿using Fungus.Actor;
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

        public string[] GetOpening()
        {
            XElement xOpening = xText.Element("Opening");
            string scene = GetText(xOpening.Element("Scene"));
            string modeline = GetText(xOpening.Element("Modeline"));

            string[] text = new string[] { scene, modeline };
            return text;
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

        private void Start()
        {
            string fileName = "text.xml";

            if (xText == null)
            {
                xText = GetComponent<SaveLoad>().LoadXML(fileName);
            }
        }
    }
}