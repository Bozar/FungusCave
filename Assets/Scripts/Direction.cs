using UnityEngine;

public enum RelativePosition { INVALID, Cardinal, Diagonal };

public class Direction : MonoBehaviour
{
    private bool checkX;
    private bool checkY;
    private int[] position;

    public bool CheckDirection(RelativePosition checkType,
       int[] source, int targetX, int targetY)
    {
        position = new int[] { targetX, targetY };

        return CheckDirection(checkType, source, position);
    }

    public bool CheckDirection(RelativePosition checkType,
        int[] source, int[] target)
    {
        switch (checkType)
        {
            case RelativePosition.Cardinal:
                return CheckCardinal(source, target);

            case RelativePosition.Diagonal:
                return CheckDiagonal(source, target);

            default:
                return false;
        }
    }

    private bool CheckCardinal(int[] source, int[] target)
    {
        checkX = (source[0] == target[0])
            && (System.Math.Abs(source[1] - target[1]) == 1);
        checkY = (source[1] == target[1])
            && (System.Math.Abs(source[0] - target[0]) == 1);

        return checkX || checkY;
    }

    private bool CheckDiagonal(int[] source, int[] target)
    {
        checkX = System.Math.Abs(source[0] - target[0]) == 1;
        checkY = System.Math.Abs(source[1] - target[1]) == 1;

        return checkX && checkY;
    }
}
