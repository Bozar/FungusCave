using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour
    {
        private int maxPotion;
        private int relieveStress;
        private int restoreEnergy;

        public int CurrentPotion { get; private set; }

        public void DrinkPotion()
        {
            GetComponent<HP>().GainHP(GetComponent<HP>().MaxHP);
            GetComponent<Infection>().ResetInfection();
            GetComponent<Stress>().LoseStress(relieveStress);
            GetComponent<Energy>().GainEnergy(restoreEnergy);
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
            maxPotion = 9;
            relieveStress = 3;
            restoreEnergy = 4000;
        }
    }
}
