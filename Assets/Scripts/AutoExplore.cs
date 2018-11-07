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
    private int[] position;
    private List<int[]> surround;
    private int x;
    private int y;

    public Command ChooseNextStep()
    {
        FindStartPoint();

        if (checkPosition.Count < 1)
        {
            return Command.Invalid;
        }

        ResetBoard();
        MarkDistance();

        // TODO: Delete these lines.
        surround = coordinate.SurroundCoord(Surround.Diagonal,
            coordinate.Convert(gameObject.transform.position));
        surround = dungeon.FilterPositions(surround);

        foreach (var pos in surround)
        {
            FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
                board[pos[0], pos[1]].ToString());
        }

        return Command.Right;
    }

    private void Awake()
    {
        checkPosition = new Stack<int[]>();
        surround = new List<int[]>();
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

        board = new int[dungeon.Width, dungeon.Height];
    }
}
