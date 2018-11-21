using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private int maxPotion;

    public int CurrentPotion { get; private set; }

    public void DrinkPotion()
    {
        // TODO: Lose stress. Lose infections. Gain energy.
        gameObject.GetComponent<HP>().GainHP(
            gameObject.GetComponent<HP>().MaxHP, true);

        LosePotion(1);
    }

    public void GainPotion(int potion)
    {
        CurrentPotion = Math.Min(maxPotion, CurrentPotion + potion);
    }

    public bool HasEnoughPotion(int min)
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
