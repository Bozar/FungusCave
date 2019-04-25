using Fungus.Actor.Render;
using Fungus.GameSystem.SaveLoadData;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public enum ColorName { TEST, White, Black, Grey, Orange, Green }

    // https://gamedev.stackexchange.com/questions/92149/changing-color-of-ui-text-in-unity-into-custom-values/
    public class GameColor : MonoBehaviour
    {
        private readonly string alpha = "Alpha";
        private readonly string blue = "Blue";
        private readonly string file = "colorScheme.xml";
        private readonly string green = "Green";
        private readonly string hex = "HexColor";
        private readonly string red = "Red";
        private readonly string rgba = "RGBAColor";

        private XElement xele;

        public void ChangeObjectColor(GameObject go, ColorName color)
        {
            if (go.GetComponent<RenderSprite>() != null)
            {
                go.GetComponent<RenderSprite>().ChangeDefaultColor(color);
            }
            else
            {
                go.GetComponent<SpriteRenderer>().color = PickColor(color);
            }
        }

        public string GetColorfulText(string text, ColorName color)
        {
            string hex = GetComponent<GameColor>().PickHexColor(color);
            string output = $"<color={hex}>{text}</color>";

            return output;
        }

        public Color PickColor(ColorName name)
        {
            Load();

            XElement e = xele.Element(name.ToString()).Element(rgba);
            byte r = (byte)(int)e.Element(red);
            byte g = (byte)(int)e.Element(green);
            byte b = (byte)(int)e.Element(blue);
            byte a = (byte)(int)e.Element(alpha);

            return new Color32(r, g, b, a);
        }

        public string PickHexColor(ColorName name)
        {
            Load();

            string color = (string)xele.Element(name.ToString()).Element(hex);
            color = "#" + color;

            return color;
        }

        private void Load()
        {
            if (xele == null)
            {
                xele = GetComponent<SaveLoadFile>().LoadXML(file);
            }
        }
    }
}
