using System.Collections.Generic;
using UnityEngine;

public class FOVSimple : MonoBehaviour
{
    private DungeonBoard board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private FieldOfView fov;
    private int maxRange;
    private int[] position;
    private List<int[]> surround;
    private int x;
    private int y;

    public void UpdateFOV()
    {
        UpdatePosition();
        UpdateFOVBoard();
    }

    private void Awake()
    {
        maxRange = 5;
        checkPosition = new Stack<int[]>();
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        fov = gameObject.GetComponent<FieldOfView>();
    }

    private void UpdateFOVBoard()
    {
        if (checkPosition.Count < 1)
        {
            return;
        }

        position = checkPosition.Pop();
        x = position[0];
        y = position[1];

        surround = coordinate.SurroundCoord(
            ConvertCoordinates.Surround.Diagonal, x, y);

        foreach (var grid in surround)
        {
            if (!board.IsInsideRange(DungeonBoard.FOVShape.Rhombus,
                maxRange,
                coordinate.Convert(gameObject.transform.position),
                grid))
            {
                continue;
            }

            if ((board.CheckBlock(DungeonBoard.DungeonBlock.Floor, grid)
                || board.CheckBlock(DungeonBoard.DungeonBlock.Pool, grid))
                && (fov.CheckFOV(grid) != FieldOfView.FOVStatus.TEST))
            {
                checkPosition.Push(grid);
            }
            fov.ChangeFOVBoard(FieldOfView.FOVStatus.TEST, grid);
            //fov.ChangeFOVBoard(FieldOfView.FOVStatus.Insight, grid);
        }

        UpdateFOVBoard();
    }

    private void UpdatePosition()
    {
        position = coordinate.Convert(gameObject.transform.position);
        checkPosition.Push(position);
    }
}
