using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private DungeonBoard board;
    private ConvertCoordinates coordinate;
    private int[] position;
    private int[][] surround;
    private int x;
    private int y;

    private void PaintRed()
    {
        position = coordinate.Convert(gameObject.transform.position);
        x = position[0];
        y = position[1];
        surround = coordinate.SurroundCoord(
            ConvertCoordinates.Surround.Diagonal, x, y);

        foreach (var pos in surround)
        {
            if (board.CheckTerrain(DungeonBoard.DungeonBlock.Wall,
                pos[0], pos[1]))
            {
                board.GetBlock(pos[0], pos[1]).GetComponent<SpriteRenderer>()
                    .color = FindObjects.GameLogic.GetComponent<Color>()
                    .PickColor(Color.ColorName.TEST);
            }
        }
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }

    private void Update()
    {
        PaintRed();
    }
}
