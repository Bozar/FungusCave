using UnityEngine;

public class Defend : MonoBehaviour
{
    private ConvertCoordinates coordinate;
    private UIMessage message;

    public void TakeDamage(int damage)
    {
        int[] position;

        position = coordinate.Convert(gameObject.transform.position);
        message.StoreText(position[0] + "," + position[1] + " is hit.");

        // TODO: Check if actor is dead.
        gameObject.GetComponent<HP>().LoseHP(damage);
    }

    public void TestKill()
    {
        FindObjects.GameLogic.GetComponent<ObjectPool>().StoreObject(gameObject);
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        message = FindObjects.GameLogic.GetComponent<UIMessage>();
    }
}
