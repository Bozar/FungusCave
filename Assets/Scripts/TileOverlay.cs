using UnityEngine;

public class TileOverlay : MonoBehaviour
{
    private BuildDungeon dungeon;

    public void CoverTile(Vector3 position)
    {
        CoverTile(true, position);
    }

    public void CoverTile(bool cover, Vector3 position)
    {
        if (dungeon.CheckTerrain(BuildDungeon.DungeonBlock.Pool, position))
        {
            dungeon.GetBlock(position).GetComponent<Renderer>().enabled
                = !cover;
        }
    }

    private void Start()
    {
        dungeon = FindObjects.GameLogic.GetComponent<BuildDungeon>();
    }
}
