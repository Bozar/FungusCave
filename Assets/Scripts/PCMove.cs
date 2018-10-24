using UnityEngine;

public class PCMove : MonoBehaviour
{
    private void Update()
    {
        if (!FindObjects.GameLogic.GetComponent<SchedulingSystem>()
            .IsCurrentActor(gameObject))
        {
            return;
        }

        FindObjects.GameLogic.GetComponent<Move>().
            MoveAround(gameObject.transform);
    }
}
