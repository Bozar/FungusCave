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

        if (gameObject.GetComponent<FieldOfView>() != null)
        {
            gameObject.GetComponent<FieldOfView>().UpdateFOV();
        }

        FindObjects.GameLogic.GetComponent<Move>().
            MoveAround(gameObject.transform);
    }
}
