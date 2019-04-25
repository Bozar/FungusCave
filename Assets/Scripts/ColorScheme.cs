using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public enum ColorSchemeTag
    { TEST, Foreground, Background, Highlight, Shadow, Warning }

    public class ColorScheme : MonoBehaviour
    {
        public string GetHexColor(ColorSchemeTag cs)
        {
            return "";
        }

        public Color GetRGBAColor(ColorSchemeTag cs)
        {
            return new Color32();
        }
    }
}
