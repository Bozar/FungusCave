using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameSetting : MonoBehaviour
    {
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
        }

        private void Start()
        {
            xFile = GetComponent<SaveLoad>().LoadXML("setting.xml");
        }
    }
}
