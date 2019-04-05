using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.Actor.Turn;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIStatus : MonoBehaviour, IUpdateUI
    {
        private UIText getText;
        private StringBuilder sb;

        private delegate Text UIText(UITag tag);

        public void PrintStaticText()
        {
            return;
        }

        public void PrintStaticText(string text)
        {
            return;
        }

        public void PrintText()
        {
            UpdateHP();
            UpdateStress();
            UpdatePotion();
            UpdateDamage();

            UpdateEnvironment();
            UpdateTurn();

            UpdatePower();
            UpdateInfection();

            UpdateVersion();
            UpdateSeed();
        }

        private void Awake()
        {
            sb = new StringBuilder();
        }

        private void Start()
        {
            getText = FindObjects.GetUIText;
        }

        private void UpdateDamage()
        {
            int current = FindObjects.PC.GetComponent<IDamage>().CurrentDamage;

            getText(UITag.DamageData).text = current.ToString();
        }

        private void UpdateEnvironment()
        {
            // TODO: check the weather.
            //bool hasFog = true;
            bool hasFog = false;
            sb = sb.Remove(0, sb.Length);
            sb.Append("[ ");

            if (FindObjects.PC.GetComponent<AIVision>().CanSeeTarget(
                MainObjectTag.Actor))
            {
                sb.Append(FindObjects.IconEnemy);
            }

            if (GetComponent<DungeonBoard>().CheckBlock(SubObjectTag.Pool,
                FindObjects.PC.transform.position))
            {
                if (sb.Length > 2)
                {
                    sb.Append(" | ");
                }
                sb.Append(FindObjects.IconPool);
            }

            if (hasFog)
            {
                if (sb.Length > 2)
                {
                    sb.Append(" | ");
                }
                sb.Append(FindObjects.IconFog);
            }
            sb.Append(" ]");

            getText(UITag.Terrain).text = sb.ToString();
        }

        private void UpdateHP()
        {
            int current = FindObjects.PC.GetComponent<HP>().CurrentHP;
            int max = FindObjects.PC.GetComponent<HP>().MaxHP;

            getText(UITag.HPData).text = current + "/" + max;
        }

        private void UpdateInfection()
        {
            string label = "[ Infection ]";
            int duration;
            InfectionTag tag;

            if (FindObjects.PC.GetComponent<Infection>().HasInfection(
                out tag, out duration))
            {
                getText(UITag.InfectionName).text
                    = GetComponent<InfectionData>().GetInfectionName(tag);
                getText(UITag.InfectionDuration).text = duration.ToString();
                getText(UITag.InfectionLabel).text = label;
            }
            else
            {
                getText(UITag.InfectionName).text = "";
                getText(UITag.InfectionDuration).text = "";
                getText(UITag.InfectionLabel).text = "";
            }
        }

        private void UpdatePotion()
        {
            int current = FindObjects.PC.GetComponent<Potion>().CurrentPotion;

            getText(UITag.PotionData).text = current.ToString();
        }

        private void UpdatePower()
        {
            PowerTag power;
            bool isActive;
            bool hasPower = false;
            string uiName = "PowerData";
            UITag uiTag;

            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                if (FindObjects.PC.GetComponent<Power>().HasPower(slot,
                    out power, out isActive))
                {
                    hasPower = true;

                    uiTag = (UITag)Enum.Parse(typeof(UITag), uiName + (int)slot);

                    getText(uiTag).text
                        = GetComponent<PowerData>().GetPowerName(power);

                    getText(uiTag).color = isActive
                        ? GetComponent<GameColor>().PickColor(ColorName.White)
                        : GetComponent<GameColor>().PickColor(ColorName.Grey);
                }
            }

            if (hasPower)
            {
                getText(UITag.PowerLabel).text = "[ Power ]";
            }
        }

        private void UpdateSeed()
        {
            string seed = FindObjects.GameLogic.GetComponent<RandomNumber>()
                .RootSeed.ToString();
            int seedLength = seed.Length;

            for (int i = 1; seedLength > i * 3; i++)
            {
                seed = seed.Insert(i * 4 - 1, "-");
            }

            getText(UITag.Seed).text = seed;
        }

        private void UpdateStress()
        {
            int current = FindObjects.PC.GetComponent<Stress>().CurrentStress;
            int max = FindObjects.PC.GetComponent<Stress>().MaxStress;

            getText(UITag.StressData).text = current + "/" + max;
        }

        private void UpdateTurn()
        {
            int current
                = FindObjects.PC.GetComponent<TurnIndicator>().CurrentTurn;
            int max
                = FindObjects.PC.GetComponent<TurnIndicator>().BarSpearator;
            sb = sb.Remove(0, sb.Length);

            sb.Append("[ ");

            if (current > 0)
            {
                for (int i = 0; i < current; i++)
                {
                    sb.Append("X ");

                    if ((current > max) && (i + 1 == max))
                    {
                        sb.Append("| ");
                    }
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(" ]");
            }

            getText(UITag.Turn).text = sb.ToString();
        }

        private void UpdateVersion()
        {
            string version = FindObjects.Version;
            if (GetComponent<WizardMode>().IsWizardMode)
            {
                version = "? " + version;
            }

            getText(UITag.Version).text = version;
        }
    }
}
