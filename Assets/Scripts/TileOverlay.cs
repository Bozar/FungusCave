﻿using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.Render
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
            currentPosition = transform.position;
            CheckTile();
        }

        private void Start()
        {
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        }
    }
}
