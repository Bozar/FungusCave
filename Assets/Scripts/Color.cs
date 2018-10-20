using UnityEngine;

// https://gamedev.stackexchange.com/questions/92149/changing-color-of-ui-text-in-unity-into-custom-values/
public class Color : MonoBehaviour
{
    private Color32 color;

    public enum ColorName { TEST, White, Black, Grey }

    public Color32 PickColor(ColorName name)

    {
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
        }

        return color;
    }

    private void Awake()
    {
        color = new Color32();
    }
}
