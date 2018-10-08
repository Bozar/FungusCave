using UnityEngine;

public class ConvertCoordinates : MonoBehaviour
{
    private readonly float index2Vector = 0.5f;
    private readonly float vector2Index = 2.0f;

    public Vector3 Convert(int indexX, int indexY)
    {
        float vectorX;
        float vectorY;
        Vector3 vectorPosition;

        vectorX = indexX * index2Vector;
        vectorY = indexY * index2Vector;

        vectorPosition = new Vector3(vectorX, vectorY);

        return vectorPosition;
    }

    public Vector3 Convert(int[] arrayPosition)
    {
        float vectorX;
        float vectorY;
        Vector3 vectorPosition;

        vectorX = arrayPosition[0] * index2Vector;
        vectorY = arrayPosition[1] * index2Vector;

        vectorPosition = new Vector3(vectorX, vectorY);

        return vectorPosition;
    }

    public int[] Convert(Vector3 vectorPosition)
    {
        int indexX;
        int indexY;
        int[] arrayPosition;

        indexX = (int)Mathf.Floor(vectorPosition.x * vector2Index);
        indexY = (int)Mathf.Floor(vectorPosition.y * vector2Index);

        arrayPosition = new int[] { indexX, indexY };

        return arrayPosition;
    }

    public void Test()
    {
        int[] inputArray = new int[] { 2, 3 };
        Vector3 inputVector = new Vector3(1.2f, 3.6f);

        Debug.Log("Array -> vector3: " + Convert(inputArray));
        Debug.Log("x, y -> vector3: " + Convert(inputArray[0], inputArray[1]));
        Debug.Log("Vector3 -> arrayX: " + Convert(inputVector)[0]);
        Debug.Log("Vector3 -> arrayY: " + Convert(inputVector)[1]);
    }
}
