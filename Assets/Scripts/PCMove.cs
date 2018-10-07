using UnityEngine;

public class PCMove : MonoBehaviour
{
    private void Update()
    {
        FindObjects.GameLogic.GetComponent<Move>().
            MoveAround(gameObject.transform);
    }
}
