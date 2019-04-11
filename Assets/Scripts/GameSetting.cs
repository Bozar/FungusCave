using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameSetting : MonoBehaviour, ISaveLoad
    {
        private string fileName;
        private XElement xFile;

        public string DefaultLanguage
        {
            get { return (string)xFile.Element("Language").Element("Default"); }
        }

        public bool IsWizard
        {
            get { return (bool)xFile.Element("IsWizard"); }
        }

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

        public void Load()
        {
            xFile = GetComponent<SaveLoad>().LoadXML(fileName);
        }

        public void Save()
        {
            GetComponent<SaveLoad>().SaveXML(xFile, fileName);
        }

        private void Start()
        {
            fileName = "setting.xml";
            Load();
        }
    }
}
