using System.Collections.Generic;
using UnityEngine;

// Create game objects based on the 2D array from DungeonBoard.
public class DungeonObjects : MonoBehaviour
{
    private Dictionary<string, GameObject> blockDict;
    private string blockKey;
    private DungeonBoard board;
    private ConvertCoordinates coordinate;
    private GameObject fungusTile;
    private GameObject newTile;
    private GameObject poolTile;
    private GameObject wallTile;

    public void CreateBuildings()
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                switch (board.Board[x, y])
                {
                    case DungeonBoard.DungeonBlock.Wall:
                        newTile = wallTile;
                        blockDict = board.WallBlocks;
                        break;

                    case DungeonBoard.DungeonBlock.Pool:
                        newTile = poolTile;
                        blockDict = board.PoolBlocks;
                        break;

                    case DungeonBoard.DungeonBlock.Fungus:
                        newTile = fungusTile;
                        blockDict = board.FungusBlocks;
                        break;

                    default:
                        newTile = null;
                        blockDict = null;
                        break;
                }

                blockKey = x.ToString() + ',' + y.ToString();

                if (newTile != null && blockDict != null
                    && !blockDict.ContainsKey(blockKey))
                {
                    newTile = Instantiate(newTile);
                    newTile.transform.position = coordinate.Convert(x, y);
                    newTile.AddComponent<RenderSprite>();

                    blockDict.Add(blockKey, newTile);
                }
            }
        }
    }

    private void Awake()
    {
        wallTile = Resources.Load("Wall") as GameObject;
        poolTile = Resources.Load("Pool") as GameObject;
        fungusTile = Resources.Load("Fungus") as GameObject;
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
    }
}
