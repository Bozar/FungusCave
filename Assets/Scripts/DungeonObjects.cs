using UnityEngine;

// Create game objects based on the 2D array from DungeonBoard.
public class DungeonObjects : MonoBehaviour
{
    private DungeonBoard board;
    private ConvertCoordinates coordinate;
    private GameObject fungusTile;
    private GameColor gameColor;
    private GameObject newTile;
    private GameObject poolTile;
    private GameObject wallTile;

    public void CreateBuildings()
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                switch (board.Blueprint[x, y])
                {
                    case BuildingTag.Wall:
                        newTile = wallTile;
                        break;

                    case BuildingTag.Pool:
                        newTile = poolTile;
                        break;

                    case BuildingTag.Fungus:
                        newTile = fungusTile;
                        break;

                    default:
                        newTile = null;
                        break;
                }

                if (newTile != null)
                {
                    newTile = Instantiate(newTile);
                    newTile.transform.position = coordinate.Convert(x, y);
                    newTile.AddComponent<RenderSprite>();

                    newTile.GetComponent<RenderSprite>().ChangeColor(
                        gameColor.PickColor(ColorName.Black));

                    board.Blocks[x, y] = newTile;
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
        gameColor = FindObjects.GameLogic.GetComponent<GameColor>();
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
    }
}
