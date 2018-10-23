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

    private void Update()
    {
        position = coordinate.GetComponent<ConvertCoordinates>().Convert(
            gameObject.transform.position);
        x = position[0];
        y = position[1];

        switch (pc.GetComponent<FieldOfView>().CheckFov(x, y))
        {
            case FieldOfView.FoVStatus.Unknown:
                HideSprite();
                break;

            case FieldOfView.FoVStatus.Visited:
                RememberSprite();
                break;

            case FieldOfView.FoVStatus.Insight:
                //ChangeColor(gameColor.PickColor(GameColor.ColorName.TEST));
                ShowSprite();
                break;
        }
    }
}
