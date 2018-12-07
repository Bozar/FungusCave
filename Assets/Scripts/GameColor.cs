using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public enum ColorName { TEST, White, Black, Grey }

    // https://gamedev.stackexchange.com/questions/92149/changing-color-of-ui-text-in-unity-into-custom-values/
    public class GameColor : MonoBehaviour
    {
        public Color PickColor(ColorName name)
        {
            Color32 color;

            switch (name)
            {
                case ColorName.White:
                    color = new Color32(171, 178, 191, 255);
                    break;

                case ColorName.Black:
                    color = new Color32(40, 44, 52, 0);
                    break;

                case ColorName.Grey:
                    color = new Color32(73, 81, 98, 255);
                    break;

                case ColorName.TEST:
                    color = new Color32(255, 0, 0, 255);
                    break;

                default:
                    color = new Color32();
                    break;
            }

            return color;
        }
    }
}
