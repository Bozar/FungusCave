using UnityEngine;

// Create game objects based on the 2D array from DungeonBoard.
public class DungeonObjects : MonoBehaviour
{
    private DungeonBoard board;
    private ConvertCoordinates coordinate;
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
                        MainObjectTag.Building, board.Blueprint[x, y], x, y);
                }
            }
        }
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        oPool = FindObjects.GameLogic.GetComponent<ObjectPool>();
    }
}
