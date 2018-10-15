using UnityEngine;

public class TileOverlay : MonoBehaviour
{
    private DungeonBoard dungeon;

    public void CoverTile(Vector3 position)
    {
        CoverTile(true, position);
    }

    public void CoverTile(bool cover, Vector3 position)
    {
        if (dungeon.CheckTerrain(DungeonBoard.DungeonBlock.Pool, position))
        {
            dungeon.GetBlock(position).GetComponent<Renderer>().enabled
                = !cover;
        }
    }

    private void Start()
    {
        dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }
}
