using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public enum ColorSchemeTag
    { TEST, Foreground, Background, Highlight, Shadow, Warning }

    public class ColorScheme : MonoBehaviour, ISaveLoadXML
    {
        private XElement xele;

        public string GetHexColor(ColorSchemeTag cs)
        {
            return "";
        }

        public Color GetRGBAColor(ColorSchemeTag cs)
        {
            return new Color32();
        }

        public void Load()
        {
            string file = GetComponent<GameSetting>().ColorScheme;
            string extension = ".xml";
            file += extension;
            string directory = "ColorScheme";

            xele = GetComponent<SaveLoadFile>().LoadXML(file, directory);
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
        }
    }
}
