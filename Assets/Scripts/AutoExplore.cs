using System.Collections.Generic;
using UnityEngine;

// http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
public class AutoExplore : MonoBehaviour
{
    private readonly int NotChecked = 9999;
    private int[,] board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private DungeonBoard dungeon;
    private bool isUnknown;
    private bool isVaildDistance;
    private bool isWalkable;
    private int minDistance;
    private int[] pcPosition;
    private int[] position;
    private RandomNumber random;
    private List<int[]> surround;
    private int x;
    private int y;

    public int[] ChooseNextStep()
    {
        FindStartPoint();

        if (checkPosition.Count < 1)
        {
            return null;
        }

        pcPosition = coordinate.Convert(gameObject.transform.position);
        ResetBoard();
        MarkDistance();
        position = ChooseNextGrid();

        return position;
    }

    private void Awake()
    {
        checkPosition = new Stack<int[]>();
        surround = new List<int[]>();
    }

    private int[] ChooseNextGrid()
    {
        surround = coordinate.SurroundCoord(Surround.Diagonal, pcPosition);
        surround = dungeon.FilterPositions(surround);

        minDistance = board[surround[0][0], surround[0][1]];
        checkPosition.Clear();
        checkPosition.Push(surround[0]);

        foreach (var pos in surround)
        {
            if (board[pos[0], pos[1]] < minDistance)
            {
                minDistance = board[pos[0], pos[1]];
                checkPosition.Clear();
                checkPosition.Push(pos);
            }
            else if (board[pos[0], pos[1]] == minDistance)
            {
                checkPosition.Push(pos);
            }
        }

        surround = new List<int[]>();
        while (checkPosition.Count > 0)
        {
            surround.Add(checkPosition.Pop());
        }

        return surround[random.RNG.Next(0, surround.Count)];
    }

    private void FindStartPoint()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                isUnknown = gameObject.GetComponent<FieldOfView>().CheckFOV(
                    FOVStatus.Unknown, i, j);

                isWalkable = gameObject.GetComponent<Move>().IsWalkable(i, j);

                if (isUnknown && isWalkable)
                {
                    position = new int[] { i, j };
                    checkPosition.Push(position);

                    break;
                }
            }
        }
    }

    private int GetMinDistance()
    {
        minDistance = board[surround[0][0], surround[0][1]];

        foreach (var pos in surround)
        {
            if (board[pos[0], pos[1]] < minDistance)
            {
                minDistance = board[pos[0], pos[1]];
            }
        }

        minDistance = minDistance == NotChecked ? 0 : (minDistance + 1);

        return minDistance;
    }

    private void MarkDistance()
    {
        if (checkPosition.Count < 1)
        {
            return;
        }

        position = checkPosition.Pop();
        x = position[0];
        y = position[1];

        surround = coordinate.SurroundCoord(Surround.Diagonal, position);
        surround = dungeon.FilterPositions(surround);

        if (gameObject.GetComponent<FieldOfView>().CheckFOV(
            FOVStatus.Unknown, position))
        {
            board[x, y] = 0;
        }
        else
        {
            board[x, y] = GetMinDistance();
        }

        foreach (var pos in surround)
        {
            isWalkable = gameObject.GetComponent<Move>().IsWalkable(
                pos[0], pos[1]);

            isVaildDistance
                = System.Math.Abs(board[x, y] - board[pos[0], pos[1]])
                <= 1;

            if (isWalkable && !isVaildDistance)
            {
                checkPosition.Push(pos);
            }
        }

        MarkDistance();
    }

    private void ResetBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = NotChecked;
            }
        }
    }

    private void Start()
    {
        dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        random = FindObjects.GameLogic.GetComponent<RandomNumber>();

        board = new int[dungeon.Width, dungeon.Height];
    }
}
