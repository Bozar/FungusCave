using Fungus.Actor.InputManager;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem
{
    public enum Surround { Cardinal, Diagonal };

    public class ConvertCoordinates : MonoBehaviour
    {
        private readonly float index2Vector = 0.5f;
        private readonly float vector2Index = 2.0f;

        public Vector3 Convert(int x, int y)
        {
            float vectorX = x * index2Vector;
            float vectorY = y * index2Vector;

            return new Vector3(vectorX, vectorY);
        }

        public Vector3 Convert(int[] position)
        {
            float vectorX = position[0] * index2Vector;
            float vectorY = position[1] * index2Vector;

            return new Vector3(vectorX, vectorY);
        }

        public int[] Convert(Command direction, Vector3 position)
        {
            int[] start = Convert(position);

            return Convert(direction, start[0], start[1]);
        }

        public int[] Convert(Command direction, int x, int y)
        {
            switch (direction)
            {
                case Command.Left:
                    x -= 1;
                    break;

                case Command.Right:
                    x += 1;
                    break;

                case Command.Up:
                    y += 1;
                    break;

                case Command.Down:
                    y -= 1;
                    break;

                case Command.UpLeft:
                    x -= 1;
                    y += 1;
                    break;

                case Command.UpRight:
                    x += 1;
                    y += 1;
                    break;

                case Command.DownLeft:
                    x -= 1;
                    y -= 1;
                    break;

                case Command.DownRight:
                    x += 1;
                    y -= 1;
                    break;
            }
            return new int[] { x, y };
        }

        public int[] Convert(Vector3 position)
        {
            int indexX = (int)Math.Floor(position.x * vector2Index);
            int indexY = (int)Math.Floor(position.y * vector2Index);

            return new int[] { indexX, indexY };
        }

        public int Convert(float position)
        {
            int indexXorY = (int)Math.Floor(position * vector2Index);

            return indexXorY;
        }

        public int[] RelativeCoord(GameObject target)
        {
            int[] sourcePos = Convert(FindObjects.PC.transform.position);
            int[] targetPos = Convert(target.transform.position);
            int[] relativePos = new int[]
            {
                targetPos[0] - sourcePos[0], targetPos[1] - sourcePos[1]
            };

            return relativePos;
        }

        public string RelativeCoordWithName(GameObject target)
        {
            int[] relativePos = RelativeCoord(target);
            string targetName = GetComponent<ActorData>().GetStringData(
                target.GetComponent<MetaInfo>().SubTag, DataTag.ActorName);
            string posWithName
                = targetName
                + " [" + relativePos[0] + ", " + relativePos[1] + "]";

            return posWithName;
        }

        public List<int[]> SurroundCoord(Surround neighbor, int[] position)
        {
            int x = position[0];
            int y = position[1];

            return SurroundCoord(neighbor, x, y);
        }

        public List<int[]> SurroundCoord(Surround neighbor, int x, int y)
        {
            List<int[]> surround = new List<int[]>();
            List<int[]> temp = new List<int[]>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    surround.Add(new int[] { x + i, y + j });
                }
            }

            if (neighbor == Surround.Cardinal)
            {
                foreach (var coord in surround)
                {
                    if ((x == coord[0]) || (y == coord[1]))
                    {
                        temp.Add(coord);
                    }
                }
                surround = temp;
            }
            return surround;
        }
    }
}
