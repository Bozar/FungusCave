using Fungus.Actor.ObjectManager;
using Fungus.Actor.WorldBuilding;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Render
{
    public class TileOverlay : MonoBehaviour
    {
        private Vector3 currentPosition;
        private DungeonBoard dungeon;
        private Vector3 previousPosition;

        private void CheckTile()
        {
            if (currentPosition == previousPosition)
            {
                CoverTile(true, currentPosition);
            }
            else
            {
                CoverTile(false, previousPosition);
                CoverTile(true, currentPosition);

                previousPosition = currentPosition;
            }
        }

        private void CoverTile(bool cover, Vector3 position)
        {
            if (dungeon.CheckBlock(SubObjectTag.Pool, position))
            {
                dungeon.GetBlockObject(position).GetComponent<Renderer>().enabled
                    = !cover;
            }
        }

        private void LateUpdate()
        {
            currentPosition = gameObject.transform.position;
            CheckTile();
        }

        private void Start()
        {
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        }
    }
}
