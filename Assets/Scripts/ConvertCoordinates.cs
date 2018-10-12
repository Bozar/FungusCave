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
        indexX = (int)Mathf.Floor(position.x * vector2Index);
        indexY = (int)Mathf.Floor(position.y * vector2Index);

        arrayPosition = new int[] { indexX, indexY };

        return arrayPosition;
    }

    public int Convert(float position)
    {
        indexXorY = (int)Mathf.Floor(position * vector2Index);

        return indexXorY;
    }
}
