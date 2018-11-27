using UnityEngine;

public class NPCPowers : MonoBehaviour, ICheckActorPower
{
    public bool HasPower(PowerTag tag)
    {
        return false;
    }

    public bool PowerIsActive(PowerTag tag)
    {
        return false;
    }
}
