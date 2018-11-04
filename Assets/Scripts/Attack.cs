using UnityEngine;

public class Attack : MonoBehaviour
{
    private ActorBoard actorBoard;
    private int[] attackerPosition;
    private int attackerX;
    private int attackerY;
    private int baseEnergy;
    private double diagonalFactor;
    private double linearFactor;
    private bool useLinearFactor;

    public void DealDamage(int x, int y)
    {
        if (!gameObject.GetComponent<Energy>().HasEnoughEnergy()
            || !actorBoard.HasActor(x, y))
        {
            return;
        }

        attackerPosition
            = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);
        attackerX = attackerPosition[0];
        attackerY = attackerPosition[1];

        useLinearFactor = AttackLinearly(x, y);

        gameObject.GetComponent<Energy>().CurrentEnergy -= GetEnergyCost();

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            "You hit: " + x + "," + y);
        actorBoard.GetActor(x, y).GetComponent<Defend>().TestKill();
    }

    private bool AttackLinearly(int x, int y)
    {
        bool checkX = (attackerX == x) && (System.Math.Abs(attackerY - y) == 1);
        bool checkY = (attackerY == y) && (System.Math.Abs(attackerX - x) == 1);

        return checkX || checkY;
    }

    private void Awake()
    {
        baseEnergy = 1200;
        linearFactor = 1.0;
        diagonalFactor = 1.4;
    }

    private int GetEnergyCost()
    {
        int totalEnergy;
        double direction;

        // TODO: Attack in fog.
        direction = useLinearFactor ? linearFactor : diagonalFactor;

        totalEnergy = (int)System.Math.Floor(baseEnergy * direction);

        return totalEnergy;
    }

    private void Start()
    {
        actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
    }
}
