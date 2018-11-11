using System.Collections.Generic;
using UnityEngine;

public class FOVSimpleDijkstra : MonoBehaviour
{
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private int[] currentPosition;
    private int[,] distanceBoard;
    private DungeonBoard dungeon;
    private FieldOfView fov;
    private bool fovTest;
    private int gridDistance;
    private int maxDistance;
    private int sightRange;
    private int[] startPosition;
    private List<int[]> surround;

    public void UpdateFOV()
    {
        ResetDistanceBoard();

        UpdateActorPosition();
        UpdateDistanceBoard();
        UpdateShadow();

        UpdateFOVBoard();
    }

    private void Awake()
    {
        maxDistance = 99999;
        gridDistance = 10;
        sightRange = 50;
        fovTest = true;

        checkPosition = new Stack<int[]>();
    }

    private int GetDistance(int[] center)
    {
        int minDistance;

        surround = coordinate.SurroundCoord(Surround.Diagonal, center);
        surround = dungeon.FilterPositions(surround);

        minDistance = distanceBoard[surround[0][0], surround[0][1]];

        foreach (var grid in surround)
        {
            minDistance = System.Math.Min(
                minDistance, distanceBoard[grid[0], grid[1]]);
        }

        return (minDistance == maxDistance) ? 0 : (minDistance + gridDistance);
    }

    private void ResetDistanceBoard()
    {
        for (int i = 0; i < distanceBoard.GetLength(0); i++)
        {
            for (int j = 0; j < distanceBoard.GetLength(1); j++)
            {
                distanceBoard[i, j] = maxDistance;
            }
        }
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        fov = gameObject.GetComponent<FieldOfView>();

        distanceBoard = new int[dungeon.Width, dungeon.Height];
    }

    private void UpdateActorPosition()
    {
        startPosition = coordinate.Convert(gameObject.transform.position);

        checkPosition.Clear();
        checkPosition.Push(startPosition);
    }

    private void UpdateDistanceBoard()
    {
        if (checkPosition.Count < 1)
        {
            return;
        }

        currentPosition = checkPosition.Pop();

        distanceBoard[currentPosition[0], currentPosition[1]]
            = GetDistance(currentPosition);

        surround = coordinate.SurroundCoord(Surround.Diagonal, currentPosition);
        surround = dungeon.FilterPositions(surround);

        foreach (var grid in surround)
        {
            if (distanceBoard[grid[0], grid[1]] == maxDistance)
            {
                checkPosition.Push(grid);
            }
        }

        UpdateDistanceBoard();
    }

    private void UpdateFOVBoard()
    {
        for (int i = 0; i < distanceBoard.GetLength(0); i++)
        {
            for (int j = 0; j < distanceBoard.GetLength(1); j++)
            {
                if (distanceBoard[i, j] <= sightRange)
                {
                    if (fovTest)
                    {
                        fov.ChangeFOVBoard(FOVStatus.TEST, i, j);
                    }
                    else
                    {
                        fov.ChangeFOVBoard(FOVStatus.Insight, i, j);
                    }
                }
            }
        }
    }

    private void UpdateShadow()
    {
    }
}
