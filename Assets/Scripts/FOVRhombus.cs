using System.Collections.Generic;
using UnityEngine;

public class FOVRhombus : MonoBehaviour
{
    private DungeonBoard board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private int[,] distanceBoard;
    private FieldOfView fov;
    private int gridX;
    private int gridY;
    private int maxRange;
    private int[] position;
    private int rangeWall;
    private int shadowWall;
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
        checkPosition = new Stack<int[]>();

        rangeWall = 5;
        shadowWall = 2;
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
        bool shape;
        bool distance;
        bool darkness;

        if (wallGrid.Count < 1)
        {
            return;
        }

        position = wallGrid.Pop();
        surround = coordinate.SurroundCoord(
            Surround.Diagonal, position);

        foreach (var grid in surround)
        {
            gridX = grid[0];
            gridY = grid[1];

            // Wall grid casts a rhombus shadow.
            shape = board.IsInsideRange(FOVShape.Rhombus,
                rangeWall, position, grid);

            // Shadow makes surrounding grids which are farther away from the
            // source darker.

            //* Source: @.
            //* Wall: #.
            //* Distance: 3.

            //* 3 3 3 3  |  3 4 4 4
            //* 2 2 2 3  |  2 2 # 4
            //* 1 1 2 3  |  1 1 2 4
            //* @ 1 2 3  |  @ 1 2 3

            distance = board.GetDistance(source, grid)
                > board.GetDistance(source, position);

            // 1) Shadow from the same type of source do not stack. Two walls do
            // not make one grid even darker.
            // 2) When there are multiple dark sources, only the darkest shadow
            // takes effect.
            // 3) For example, if the distance (X) is increased by 2 from one
            // wall grid (#) and 4 from one fog grid ($), the final modification
            // is +4.

            //* # X $
            //* # $ $

            darkness = shape
                ? (distanceBoard[gridX, gridY]
                < board.GetDistance(source, grid) + shadowWall)
                : false;

            if (shape && distance && darkness)
            {
                distanceBoard[gridX, gridY] += shadowWall;
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

        distanceBoard = new int[board.Width, board.Height];
    }

    private void UpdateDistanceBoard()
    {
        if (checkPosition.Count < 1)
        {
            return;
        }

        position = checkPosition.Pop();
        surround = coordinate.SurroundCoord(
            Surround.Diagonal, position);

        foreach (var grid in surround)
        {
            gridX = grid[0];
            gridY = grid[1];

            if (board.IndexOutOfRange(gridX, gridY))
            {
                continue;
            }

            if (board.CheckBlock(DungeonBlock.Wall, grid))
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
        bool shape;
        bool lighted;

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                position = new int[] { i, j };

                shape = board.IsInsideRange(FOVShape.Rhombus,
                    maxRange, source, position);
                lighted = distanceBoard[i, j] <= maxRange;

                if (shape && lighted)
                {
                    fov.ChangeFOVBoard(FOVStatus.Insight, position);
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
