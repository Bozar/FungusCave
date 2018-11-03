using UnityEngine;

public class ActorBoard : MonoBehaviour
{
    private GameObject[,] board;
    private DungeonBoard dungeon;

    public void AddActor(GameObject actor, int x, int y)
    {
        if (dungeon.IndexOutOfRange(x, y))
        {
            return;
        }

        board[x, y] = actor;
    }

    public GameObject GetActor(int x, int y)
    {
        if (dungeon.IndexOutOfRange(x, y))
        {
            return null;
        }

        return board[x, y];
    }

    public bool HasActor(int x, int y)
    {
        if (dungeon.IndexOutOfRange(x, y))
        {
            return false;
        }

        return board[x, y] != null;
    }

    public void RemoveActor(int x, int y)
    {
        AddActor(null, x, y);
    }

    private void Start()
    {
        dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        board = new GameObject[dungeon.Width, dungeon.Height];
    }
}
