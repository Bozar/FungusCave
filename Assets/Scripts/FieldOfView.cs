using UnityEngine;

public enum FOVStatus { Unknown, Visited, Insight, TEST };

public class FieldOfView : MonoBehaviour
{
    private DungeonBoard board;
    private FOVStatus[,] fovBoard;

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

    public bool CheckFOV(FOVStatus status, int[] position)
    {
        int x = position[0];
        int y = position[1];

        return CheckFOV(status, x, y);
    }

    public bool CheckFOV(FOVStatus status, int x, int y)
    {
        return fovBoard[x, y] == status;
    }

    public FOVStatus GetFOVStatus(int x, int y)
    {
        return fovBoard[x, y];
    }

    public void UpdateFOV()
    {
        UpdateMemory();

        // You can enable multiple FOV algorithms at the same time for testing.
        // FOVSimple covers a larger area than FOVRhombus and it is painted red.
        // 1) In `ObjectPool.cs`, attach FOVSimple component to the testing
        // actor.
        // 2) In `FOVSimple.cs`, set `fovTest` to `true`.
        // 3) Uncomment the following line.

        //gameObject.GetComponent<FOVSimpleRhombus>().UpdateFOV();
        gameObject.GetComponent<FOVRhombus>().UpdateFOV();
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
                if (CheckFOV(FOVStatus.Insight, i, j)
                    || CheckFOV(FOVStatus.TEST, i, j))
                {
                    ChangeFOVBoard(FOVStatus.Visited, i, j);
                }
            }
        }
    }
}
