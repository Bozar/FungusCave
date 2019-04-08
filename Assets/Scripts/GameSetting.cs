using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameSetting : MonoBehaviour, ISaveLoad
    {
        private string fileName;
        private XElement xFile;

        public bool IsWizard
        {
            get
            {
                return (bool)xFile.Element("IsWizard");
            }
        }

        public string Language
        {
            get
            {
                return (string)xFile.Element("Language");
            }
        }

        public int Seed
        {
            get
            {
                return (int)xFile.Element("Seed");
            }
        }

        public bool ShowOpening
        {
            get
            {
                return (bool)xFile.Element("ShowOpening");
            }

            set
            {
                xFile.Element("ShowOpening").SetValue(value);
            }
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
