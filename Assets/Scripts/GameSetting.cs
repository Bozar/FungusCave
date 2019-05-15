using Fungus.GameSystem.SaveLoadData;
using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Data
{
    public class GameSetting : MonoBehaviour, ISaveLoadXML
    {
        private string fileName;
        private XElement xFile;

        public int AutoExploreStep
        {
            get { return (int)xFile.Element("AutoExploreStep"); }
        }

        public int BeetleWarning
        {
            get
            {
                int warning = (int)xFile.Element("BeetleWarning");
                warning = Math.Min(10, warning);
                warning = Math.Max(1, warning);

                return warning;
            }
        }

        public string DefaultLanguage
        {
            get { return (string)xFile.Element("Language").Element("Default"); }
        }

        public bool IsRushMode
        {
            get { return (bool)xFile.Element("IsRushMode"); }

            set { xFile.Element("IsRushMode").SetValue(value); }
        }

        public bool IsWizard
        {
            get { return (bool)xFile.Element("IsWizard"); }
        }

        public int LowHP { get { return (int)xFile.Element("LowHP"); } }

        public int LowPotion { get { return (int)xFile.Element("LowPotion"); } }

        public int Seed
        {
            get { return (int)xFile.Element("Seed"); }
        }

        public bool ShowOpening
        {
            get { return (bool)xFile.Element("ShowOpening"); }

            set { xFile.Element("ShowOpening").SetValue(value); }
        }

        public string UserLanguage
        {
            get { return (string)xFile.Element("Language").Element("User"); }
        }

        public string GetValidLanguage(XElement xele)
        {
            if (string.IsNullOrEmpty((string)xele.Element(UserLanguage)))
            {
                return DefaultLanguage;
            }
            return UserLanguage;
        }

        public void LoadXML()
        {
            if (File.Exists(fileName))
            {
                xFile = GetComponent<SaveLoadFile>().LoadXML("", fileName);
            }
            else
            {
                xFile = GetComponent<SaveLoadFile>().LoadXML(fileName);
            }
        }

        public void SaveXML()
        {
            GetComponent<SaveLoadFile>().SaveXML(xFile, fileName, "");
        }

        private void Start()
        {
            fileName = "setting.xml";
            LoadXML();
        }
    }
}
