using UnityEngine;

public class RenderSprite : MonoBehaviour
{
    private ConvertCoordinates coordinate;
    private Color32 defaultColor;
    private GameColor gameColor;
    private GameObject pc;
    private int[] position;
    private int x;
    private int y;

    public void ChangeColor(Color32 newColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void Awake()
    {
        defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void HideSprite()
    {
        ChangeColor(gameColor.PickColor(GameColor.ColorName.Black));
    }

    private void LateUpdate()
    {
        position = coordinate.GetComponent<ConvertCoordinates>().Convert(
            gameObject.transform.position);
        x = position[0];
        y = position[1];

        switch (pc.GetComponent<FieldOfView>().CheckFOV(x, y))
        {
            case FieldOfView.FOVStatus.Unknown:
                HideSprite();
                break;

            case FieldOfView.FOVStatus.Visited:
                RememberSprite();
                break;

            case FieldOfView.FOVStatus.Insight:
                ShowSprite();
                break;

            case FieldOfView.FOVStatus.TEST:
                ChangeColor(gameColor.PickColor(GameColor.ColorName.TEST));
                break;
        }
    }

    private void RememberSprite()
    {
        ChangeColor(gameColor.PickColor(GameColor.ColorName.Grey));
    }

    private void ShowSprite()
    {
        ChangeColor(defaultColor);
    }

    private void Start()
    {
        gameColor = FindObjects.GameLogic.GetComponent<GameColor>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        pc = GameObject.FindGameObjectWithTag("PC");
    }
}
