using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private int maxPotion;

    public int CurrentPotion { get; private set; }

    public void DrinkPotion()
    {
        int restoreFull;
        int restoreHalf;

        restoreFull = gameObject.GetComponent<HP>().MaxHP;
        restoreHalf = (int)Math.Floor(restoreFull * 0.5);

        if (gameObject.GetComponent<Infection>().HasInfection(
            InfectionTag.Mutate))
        {
            gameObject.GetComponent<HP>().GainHP(restoreHalf);
        }
        else
        {
            // TODO: Lose stress. Gain energy.
            gameObject.GetComponent<HP>().GainHP(restoreFull);
            gameObject.GetComponent<Infection>().ResetInfection();
        }

        LosePotion(1);
    }

    public void GainPotion(int potion)
    {
        CurrentPotion = Math.Min(maxPotion, CurrentPotion + potion);
    }

    public bool HasEnoughPotion(int min = 1)
    {
        return CurrentPotion >= min;
    }

    public void LosePotion(int potion)
    {
        CurrentPotion = Math.Max(0, CurrentPotion - potion);
    }

    private void Awake()
    {
        CurrentPotion = 0;
        maxPotion = 99;
    }
}
