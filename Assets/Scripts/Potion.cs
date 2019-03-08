using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour
    {
        private int maxPotion;
        private UIMessage message;
        private PotionData potionData;
        private int relieveStress;
        private int restoreEnergy;

        public int CurrentPotion { get; private set; }

        public void DrinkPotion()
        {
            if (CurrentPotion < 1)
            {
                return;
            }

            if (GetComponent<Infection>().HasInfection(InfectionTag.Mutated))
            {
                GetComponent<HP>().GainHP(potionData.ReducedHP);
            }
            else
            {
                GetComponent<HP>().GainHP(GetComponent<HP>().MaxHP);
            }

            GetComponent<Infection>().ResetInfection();
            GetComponent<Stress>().LoseStress(relieveStress);
            GetComponent<Energy>().GainEnergy(restoreEnergy);
            LosePotion(1);

            message.StoreText("You inject yourself with a blood potion.");
            return;
        }

        public void GainPotion(int potion)
        {
            CurrentPotion = Math.Min(maxPotion, CurrentPotion + potion);
        }

        public void LosePotion(int potion)
        {
            CurrentPotion = Math.Max(0, CurrentPotion - potion);
        }

        private void Awake()
        {
            CurrentPotion = 2;
            maxPotion = 6;
            relieveStress = 3;
            restoreEnergy = 4000;
        }

        private void Start()
        {
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
