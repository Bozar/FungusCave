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

        public int Seed
        {
            get
            {
                return (int)xFile.Element("Seed");
            }
        }

        private void Start()
        {
            string fileName = "setting.xml";

            if (xFile == null)
            {
                xFile = GetComponent<SaveLoad>().LoadXML(fileName);
            }
        }
    }
}
