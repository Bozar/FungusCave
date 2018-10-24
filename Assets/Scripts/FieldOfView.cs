using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private DungeonBoard board;
    private FOVStatus[,] fovBoard;

    public enum FOVStatus { Unknown, Visited, Insight };

    public void ChangeFOVBoard(FOVStatus status, int[] position)
    {
        int x = position[0];
        int y = position[1];

        ChangeFOVBoard(status, x, y);
    }

    public void ChangeFOVBoard(FOVStatus status, int x, int y)
    {
        fovBoard[x, y] = status;
    }

    public FOVStatus CheckFOV(int[] position)
    {
        int x = position[0];
        int y = position[1];

        return CheckFOV(x, y);
    }

    public FOVStatus CheckFOV(int x, int y)
    {
        return fovBoard[x, y];
    }

    public void UpdateFOV()
    {
        UpdateMemory();
        gameObject.GetComponent<FOVSimple>().UpdatePosition();
        gameObject.GetComponent<FOVSimple>().UpdateFOVBoard();
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        fovBoard = new FOVStatus[board.Width, board.Height];
    }

    private void UpdateMemory()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (CheckFOV(i, j) == FOVStatus.Insight)
                {
                    ChangeFOVBoard(FOVStatus.Visited, i, j);
                }
            }
        }
    }
}
