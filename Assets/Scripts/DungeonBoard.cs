using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public enum FOVShape { Rhombus };

    // Create a 2D array. Provide methods to inspect and change its content.
    public class DungeonBoard : MonoBehaviour
    {
        private GameObject[,] blocks;
        private SubObjectTag[,] blueprint;
        private ConvertCoordinates coordinate;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public bool ChangeBlock(GameObject go, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            blocks[x, y] = go;
            return true;
        }

        public bool ChangeBlueprint(SubObjectTag block, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            blueprint[x, y] = block;
            return true;
        }

        public bool CheckBlock(SubObjectTag block, int[] position)
        {
            int x = position[0];
            int y = position[1];

            return CheckBlock(block, x, y);
        }

        public bool CheckBlock(SubObjectTag block, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            return blueprint[x, y] == block;
        }

        public bool CheckBlock(SubObjectTag block, Vector3 position)
        {
            int[] index = coordinate.Convert(position);

            return CheckBlock(block, index[0], index[1]);
        }

        public List<int[]> FilterPositions(List<int[]> positions)
        {
            List<int[]> validPositions = new List<int[]>();

            foreach (var pos in positions)
            {
                if (!IndexOutOfRange(pos[0], pos[1]))
                {
                    validPositions.Add(pos);
                }
            }

            return validPositions;
        }

        public GameObject GetBlockObject(Vector3 position)
        {
            int[] index = coordinate.Convert(position);
            return GetBlockObject(index[0], index[1]);
        }

        public GameObject GetBlockObject(int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return null;
            }

            return blocks[x, y];
        }

        public SubObjectTag GetBlockTag(int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return SubObjectTag.NONE;
            }

            return blueprint[x, y];
        }

        public int GetDistance(int[] source, int[] target)
        {
            int x = Math.Abs(source[0] - target[0]);
            int y = Math.Abs(source[1] - target[1]);

            return Math.Max(x, y);
        }

        public bool IndexOutOfRange(int x, int y)
        {
            bool checkWidth = (x < 0) || (x > Width - 1);
            bool checkHeight = (y < 0) || (y > Height - 1);

            return checkWidth || checkHeight;
        }

        public bool IsInsideRange(FOVShape shape, int maxRange,
            int[] source, int[] target)
        {
            bool check;

            if (IndexOutOfRange(target[0], target[1]))
            {
                return false;
            }

            switch (shape)
            {
                case FOVShape.Rhombus:
                    check = Math.Abs(target[0] - source[0])
                        + Math.Abs(target[1] - source[1])
                        <= maxRange;
                    break;

                default:
                    check = false;
                    break;
            }
            return check;
        }

        private void Awake()
        {
            Height = 17;
            Width = 24;
        }

        private void Start()
        {
            coordinate = GetComponent<ConvertCoordinates>();

            blueprint = new SubObjectTag[Width, Height];
            blocks = new GameObject[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    blueprint[i, j] = SubObjectTag.Floor;
                }
            }
        }
    }
}
