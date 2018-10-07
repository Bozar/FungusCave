using UnityEngine;

public class PCMove : MonoBehaviour
{
    private void Update()
    {
        GameObject.FindGameObjectWithTag("GameLogic")
            .GetComponent<Move>().MoveAround(gameObject.transform);
    }
}
