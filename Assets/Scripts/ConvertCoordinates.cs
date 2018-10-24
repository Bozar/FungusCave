using System.Collections.Generic;
using UnityEngine;

public class ConvertCoordinates : MonoBehaviour
{
    private readonly float index2Vector = 0.5f;
    private readonly float vector2Index = 2.0f;

    private int[] arrayPosition;
    private int indexX;
    private int indexXorY;
    private int indexY;
    private List<int[]> surround;
    private List<int[]> temp;
    private Vector3 vectorPosition;
    private float vectorX;
    private float vectorY;

    public enum Surround { Horizonal, Diagonal };

    public Vector3 Convert(int x, int y)
    {
        vectorX = x * index2Vector;
        vectorY = y * index2Vector;

        vectorPosition = new Vector3(vectorX, vectorY);

        return vectorPosition;
    }

    public Vector3 Convert(int[] position)
    {
        vectorX = position[0] * index2Vector;
        vectorY = position[1] * index2Vector;

        vectorPosition = new Vector3(vectorX, vectorY);

        return vectorPosition;
    }

    public int[] Convert(Vector3 position)
    {
        indexX = (int)System.Math.Floor(position.x * vector2Index);
        indexY = (int)System.Math.Floor(position.y * vector2Index);

        arrayPosition = new int[] { indexX, indexY };

        return arrayPosition;
    }

    public int Convert(float position)
    {
        indexXorY = (int)System.Math.Floor(position * vector2Index);

        return indexXorY;
    }

    public List<int[]> SurroundCoord(Surround neighbor, int[] position)
    {
        int x = position[0];
        int y = position[1];

        return SurroundCoord(neighbor, x, y);
    }

    public List<int[]> SurroundCoord(Surround neighbor, int x, int y)
    {
        surround = new List<int[]>();
        temp = new List<int[]>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                surround.Add(new int[] { x + i, y + j });
            }
        }

        if (neighbor == Surround.Horizonal)
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
