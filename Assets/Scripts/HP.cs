using System;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int CurrentHP { get; private set; }
    public int MaxHP { get; private set; }

    public void GainHP(int hp)
    {
        GainHP(hp, false);
    }

    public void GainHP(int hp, bool setHP)
    {
        if (setHP)
        {
            CurrentHP = hp;
        }
        else
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + hp);
        }
    }

    public bool IsDead()
    {
        return CurrentHP < 1;
    }

    public void LoseHP(int hp)
    {
        CurrentHP = Math.Max(0, CurrentHP - hp);
    }

    private void Start()
    {
        MaxHP = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
            gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.HP);
        CurrentHP = MaxHP;
    }
}
