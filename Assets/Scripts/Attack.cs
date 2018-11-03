using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;

    public void DealDamage(int x, int y)
    {
        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            "You hit: " + x + "," + y);
        actorBoard.GetActor(x, y).GetComponent<Defend>().TakeDamage(1);
    }

    private void Start()
    {
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
    }
}
