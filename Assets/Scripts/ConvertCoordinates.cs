using UnityEngine;

public class ConvertCoordinates : MonoBehaviour
{
    private readonly float index2Vector = 0.5f;
    private readonly float vector2Index = 2.0f;

    private int[] arrayPosition;
    private int indexX;
    private int indexXorY;
    private int indexY;
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

    public int[][] SurroundCoord(Surround neighbor, int x, int y)
    {
        int[] n = new int[] { x, y + 1 };
        int[] s = new int[] { x, y - 1 };
        int[] e = new int[] { x + 1, y };
        int[] w = new int[] { x - 1, y };
        int[] nw = new int[] { x - 1, y + 1 };
        int[] ne = new int[] { x + 1, y + 1 };
        int[] sw = new int[] { x - 1, y - 1 };
        int[] se = new int[] { x + 1, y - 1 };
        int[][] surround;

        switch (neighbor)
        {
            case Surround.Horizonal:
                surround = new int[][] { n, s, e, w };
                break;

            case Surround.Diagonal:
                surround = new int[][] { n, s, e, w, ne, nw, se, sw };
                break;

            default:
                surround = new int[][] { };
                break;
        }

        return surround;
    }
}
