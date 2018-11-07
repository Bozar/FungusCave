using UnityEngine;

public class AutoExplore : MonoBehaviour
{
    private readonly int Impassable = 999;
    private readonly int NotChecked = 9999;
    private int[,] board;

    public Command ChooseNextStep()
    {
        ResetBoard();

        return Command.Right;
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
        board = new int[
            FindObjects.GameLogic.GetComponent<DungeonBoard>().Width,
            FindObjects.GameLogic.GetComponent<DungeonBoard>().Height];
    }
}
