using Fungus.Actor;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class WizardMode : MonoBehaviour
    {
        private SchedulingSystem schedule;

        public bool IsWizardMode { get; private set; }
        public bool PrintEnergyCost { get; private set; }
        public bool RenderAll { get; private set; }

        public void AddEnergy()
        {
            schedule.CurrentActor.GetComponent<Energy>().GainEnergy(2000);
        }

        public void DrinkPotion()
        {
            schedule.CurrentActor.GetComponent<Potion>().GainPotion(1);

            //schedule.CurrentActor.GetComponent<Infection>().GainInfection(
            //    schedule.CurrentActor);

            //int[] pos = GetComponent<ConvertCoordinates>()
            //    .Convert(schedule.CurrentActor.transform.position);

            //schedule.CurrentActor.GetComponent<Attack>().MeleeAttack(
            //    pos[0], pos[1]);

            //if (schedule.CurrentActor.GetComponent<Potion>().HasEnoughPotion(1))
            //{
            //    schedule.CurrentActor.GetComponent<Potion>().DrinkPotion();
            //}
        }

        public void GainHP()
        {
            //schedule.CurrentActor.GetComponent<Potion>().GainPotion(90);
            //schedule.CurrentActor.GetComponent<Stress>().GainStress(1);
            schedule.CurrentActor.GetComponent<HP>().GainHP(10);
            //schedule.CurrentActor.GetComponent<Potion>().DrinkPotion();
        }

        public void Initialize()
        {
            GetComponent<Initialize>().InitializeGame();
        }

        public void LoseHP()
        {
            //schedule.CurrentActor.GetComponent<Potion>().LosePotion(5);
            //schedule.CurrentActor.GetComponent<Stress>().ClearStress();
            //schedule.CurrentActor.GetComponent<HP>().LoseHP(1);
            schedule.CurrentActor.GetComponent<Power>().BuyPower(PowerTag.DefEnergy2);
        }

        public void PrintEnergy()
        {
            schedule.CurrentActor.GetComponent<Energy>().PrintEnergy();
        }

        public void PrintSchedule()
        {
            schedule.PrintSchedule();
        }

        public void SwitchPrintEnergyCost()
        {
            PrintEnergyCost = !PrintEnergyCost;

            GetComponent<UIModeline>().PrintStaticText(
                "Print energy cost: " + PrintEnergyCost);
        }

        public void SwitchRenderAll()
        {
            RenderAll = !RenderAll;
        }

        private void Awake()
        {
            IsWizardMode = true;
            RenderAll = false;
            PrintEnergyCost = false;
        }

        private void Start()
        {
            schedule = GetComponent<SchedulingSystem>();
        }
    }
}
