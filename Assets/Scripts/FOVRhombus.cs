using System.Collections.Generic;
using UnityEngine;

public class FOVRhombus : MonoBehaviour
{
    private DungeonBoard board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private float[,] distanceBoard;
    private float distanceWall;
    private FieldOfView fov;
    private int gridX;
    private int gridY;
    private int maxRange;
    private int[] position;
    private int[] source;
    private List<int[]> surround;
    private Stack<int[]> wallGrid;

    public void UpdateFOV()
    {
        ResetDistanceBoard();

        UpdatePosition();
        UpdateDistanceBoard();
        UpdateShadow();

        UpdateFOVBoard();
    }

    private void Awake()
    {
        maxRange = 5;
        distanceWall = 2.5f;
        checkPosition = new Stack<int[]>();
        wallGrid = new Stack<int[]>();
    }

    private void ResetDistanceBoard()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                distanceBoard[i, j] = -1;
            }
        }
    }

    private void ShadowWall()
    {
        if (wallGrid.Count < 1)
        {
            return;
        }

        position = wallGrid.Pop();
        surround = coordinate.SurroundCoord(
            ConvertCoordinates.Surround.Diagonal, position);

        //* Source: @.
        //* Wall: #.
        //* Distance: 3.

        //* 3 3 3 3  |  3 4 4 4
        //* 2 2 2 3  |  2 2 # 4
        //* 1 1 2 3  |  1 1 2 4
        //* @ 1 2 3  |  @ 1 2 3

        foreach (var grid in surround)
        {
            gridX = grid[0];
            gridY = grid[1];

            // Wall grid casts a rhombus shadow.
            bool shape = board.IsInsideRange(DungeonBoard.FOVShape.Rhombus,
                maxRange, position, grid);

            // The shadow will make surrounding grids which are farther away from
            // the source darker.
            bool distance = board.GetDistance(source, grid)
                > board.GetDistance(source, position);

            // Shadow from the same source, for example, wall, do not stack. Only
            // the darkest shadow takes effect.
            bool darkness = false;
            if (shape)
            {
                darkness = distanceBoard[gridX, gridY]
                    < (board.GetDistance(source, grid)) + distanceWall;
            }

            if (shape && distance && darkness)
            {
                distanceBoard[gridX, gridY] += distanceWall;
                wallGrid.Push(grid);
            }
        }

        ShadowWall();
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        fov = gameObject.GetComponent<FieldOfView>();

        distanceBoard = new float[board.Width, board.Height];
    }

    private void UpdateDistanceBoard()
    {
        if (checkPosition.Count < 1)
        {
            return;
        }

        position = checkPosition.Pop();
        surround = coordinate.SurroundCoord(
            ConvertCoordinates.Surround.Diagonal, position);

        foreach (var grid in surround)
        {
            gridX = grid[0];
            gridY = grid[1];

            if (board.IndexOutOfRange(gridX, gridY))
            {
                continue;
            }

            if (board.CheckBlock(DungeonBoard.DungeonBlock.Wall, grid))
            {
                wallGrid.Push(grid);
            }
            else if (distanceBoard[gridX, gridY] < 0)
            {
                checkPosition.Push(grid);
            }

            distanceBoard[gridX, gridY] = board.GetDistance(source, grid);
        }

        UpdateDistanceBoard();
    }

    private void UpdateFOVBoard()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                position = new int[] { i, j };

                if (board.IsInsideRange(DungeonBoard.FOVShape.Rhombus,
                    maxRange, source, position)
                    && distanceBoard[i, j] <= maxRange
                    && distanceBoard[i, j] >= 0)
                {
                    fov.ChangeFOVBoard(FieldOfView.FOVStatus.Insight, position);
                }
            }
        }
    }

    private void UpdatePosition()
    {
        source = coordinate.Convert(gameObject.transform.position);
        checkPosition.Push(source);
    }

    private void UpdateShadow()
    {
        ShadowWall();
    }
}
