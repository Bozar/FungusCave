using UnityEngine;

public class RenderSprite : MonoBehaviour
{
    public Color32 Color { get; private set; }

    private void Awake()
    {
        Color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().color
            = FindObjects.GameLogic.GetComponent<GameColor>().PickColor(
                GameColor.ColorName.Black);

        if (FindObjects.GameLogic.GetComponent<DungeonBoard>().IsInsideRange(
            DungeonBoard.FOVShape.Rhombus, 5,
            FindObjects.GameLogic.GetComponent<ConvertCoordinates>().Convert(
            GameObject.FindGameObjectWithTag("PC").transform.position),
            FindObjects.GameLogic.GetComponent<ConvertCoordinates>().Convert(
            gameObject.transform.position)
            ))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color;
        }
    }
}
