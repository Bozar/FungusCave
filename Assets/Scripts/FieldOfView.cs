using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private DungeonBoard board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private FoVStatus[,] fovBoard;
    private int maxRange;
    private int[] position;
    private List<int[]> surround;
    private int x;
    private int y;

    public enum FoVStatus { Unknown, Visited, Insight };

    public FoVStatus CheckFov(int x, int y)
    {
        return fovBoard[x, y];
    }

    public FoVStatus CheckFov(int[] position)
    {
        int x = position[0];
        int y = position[1];

        return CheckFov(x, y);
    }

    private void ChangeFOVBoard(FoVStatus status, int[] position)
    {
        int x = position[0];
        int y = position[1];

        ChangeFOVBoard(status, x, y);
    }

    private void ChangeFOVBoard(FoVStatus status, int x, int y)
    {
        fovBoard[x, y] = status;
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();

        fovBoard = new FoVStatus[board.Width, board.Height];
        maxRange = 5;
        checkPosition = new Stack<int[]>();
    }

    private void Update()
    {
        UpdateMemory();
        UpdatePosition();
        UpdateFov();
    }

    private void UpdateFov()
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
                && (CheckFov(grid) != FoVStatus.Insight))
            {
                checkPosition.Push(grid);
            }

            ChangeFOVBoard(FoVStatus.Insight, grid);
        }

        UpdateFov();
    }

    private void UpdateMemory()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (CheckFov(i, j) == FoVStatus.Insight)
                {
                    ChangeFOVBoard(FoVStatus.Visited, i, j);
                }
            }
        }
    }

    private void UpdatePosition()
    {
        position = coordinate.Convert(gameObject.transform.position);
        checkPosition.Push(position);
    }
}
