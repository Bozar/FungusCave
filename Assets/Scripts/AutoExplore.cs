using UnityEngine;

public class AutoExplore : MonoBehaviour
{
    private readonly int Impassable = 999;
    private readonly int NotChecked = 9999;
    private int[,] board;
    private DungeonBoard dungeon;
    private bool isUnknownGrid;
    private bool isWalkable;
    private int[] position;

    public Command ChooseNextStep()
    {
        if (!FindStartPoint())
        {
            return Command.Invalid;
        }

        ResetBoard();

        return Command.Right;
    }

    private bool FindStartPoint()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                isUnknownGrid
                    = gameObject.GetComponent<FieldOfView>().CheckFOV(i, j)
                    == FOVStatus.Unknown;

                isWalkable = gameObject.GetComponent<Move>().IsWalkable(i, j);

                if (isUnknownGrid && isWalkable)
                {
                    position = new int[] { i, j };
                    board[i, j] = 0;

                    return true;
                }
            }
        }

        return false;
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
        board = new int[dungeon.Width, dungeon.Height];
    }
}
