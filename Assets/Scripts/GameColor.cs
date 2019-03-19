using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public enum ColorName { TEST, White, Black, Grey, Orange }

    // https://gamedev.stackexchange.com/questions/92149/changing-color-of-ui-text-in-unity-into-custom-values/
    public class GameColor : MonoBehaviour
    {
        public void ChangeObjectColor(GameObject go, ColorName color)
        {
            go.GetComponent<SpriteRenderer>().color = PickColor(color);
        }

        public Color PickColor(ColorName name)
        {
            switch (name)
            {
                case ColorName.White:
                    return new Color32(171, 178, 191, 255);

                case ColorName.Black:
                    return new Color32(40, 44, 52, 0);

                case ColorName.Grey:
                    return new Color32(73, 81, 98, 255);

                case ColorName.Orange:
                    return new Color32(229, 192, 123, 255);

                case ColorName.TEST:
                    return new Color32(255, 0, 0, 255);

                default:
                    return new Color32();
            }
        }
    }
}
