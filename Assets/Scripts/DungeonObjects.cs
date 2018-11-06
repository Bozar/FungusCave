using UnityEngine;

// Create game objects based on the 2D array from DungeonBoard.
public class DungeonObjects : MonoBehaviour
{
    private DungeonBoard board;
    private ObjectPool oPool;

    public void CreateBuildings()
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                if (!board.CheckBlock(SubObjectTag.Floor, x, y))
                {
                    oPool.CreateObject(
                        MainObjectTag.Building, board.GetBlockTag(x, y), x, y);
                }
            }
        }
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        oPool = FindObjects.GameLogic.GetComponent<ObjectPool>();
    }
}
