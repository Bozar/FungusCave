using UnityEngine;

public class PCMove : MonoBehaviour
{
    private void Update()
    {
        Move.Instance.MoveAround(gameObject.transform);
    }
}
