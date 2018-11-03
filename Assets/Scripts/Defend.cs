using UnityEngine;

public class Defend : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        int[] position;

        position = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);

        FindObjects.GameLogic.GetComponent<UIMessage>()
            .StoreText(position[0] + "," + position[1] + " is hit.");
    }
}
