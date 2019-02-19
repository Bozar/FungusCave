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
    public enum UITag
    {
        NONE, Seed, Message, Modeline,

        HPData, StressData, PotionData, DamageData,
        Turn, Terrain,
        PowerLabel, PowerData0, PowerData1, PowerData2,
        InfectionLabel,
        InfectionName, InfectionDuration,

        ExamineMessage, ExamineName, ExamineData, ExamineModeline
    };

    public interface IUpdateUI
    {
        // Static text does not update automatically.
        void PrintStaticText();

        void PrintStaticText(string text);

        void PrintText();
    }

    public class UserInterface : MonoBehaviour
    {
        private DungeonBoard board;
        private GameColor color;
        private UIDict getUI;
        private GameObject pc;
        private StringBuilder sb;

        private delegate GameObject UIDict(UITag tag);

        private void Awake()
        {
            sb = new StringBuilder();
        }

        private void LateUpdate()
        {
            if (pc == null)
            {
                pc = FindObjects.PC;
            }

            UpdateHP();
            UpdateStress();
            UpdatePotion();
            UpdateDamage();
            UpdateEnvironment();
            UpdateSeed();
            UpdateInfection();
            UpdatePower();
            UpdateTurn();

            foreach (IUpdateUI ui in GetComponents<IUpdateUI>())
            {
                ui.PrintText();
            }
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            color = GetComponent<GameColor>();
            getUI = FindObjects.GetUIObject;

            TurnOffUIElements();
        }

        private void TurnOffUIElements()
        {
            getUI(UITag.ExamineMessage).SetActive(false);
            getUI(UITag.ExamineModeline).SetActive(false);
        }

        private void UpdateDamage()
        {
            int current = pc.GetComponent<IDamage>().CurrentDamage;

            getUI(UITag.DamageData).GetComponent<Text>().text
                = current.ToString();
        }

        private void UpdateEnvironment()
        {
            // TODO: check the weather.
            //bool hasFog = true;
            bool hasFog = false;
            sb = sb.Remove(0, sb.Length);
            sb.Append("[ ");

            if (pc.GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                sb.Append(FindObjects.IconEnemy);
            }

            if (board.CheckBlock(SubObjectTag.Pool, pc.transform.position))
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

            getUI(UITag.Terrain).GetComponent<Text>().text = sb.ToString();
        }

        private void UpdateHP()
        {
            int current = pc.GetComponent<HP>().CurrentHP;
            int max = pc.GetComponent<HP>().MaxHP;

            getUI(UITag.HPData).GetComponent<Text>().text = current + "/" + max;
        }

        private void UpdateInfection()
        {
            int current;

            if (!pc.GetComponent<Infection>().HasInfection())
            {
                getUI(UITag.InfectionName).GetComponent<Text>().text = "";
                getUI(UITag.InfectionDuration).GetComponent<Text>().text = "";
                getUI(UITag.InfectionLabel).GetComponent<Text>().text = "";

                return;
            }

            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (pc.GetComponent<Infection>().HasInfection(tag, out current))
                {
                    getUI(UITag.InfectionName).GetComponent<Text>().text
                        = tag.ToString();
                    getUI(UITag.InfectionDuration).GetComponent<Text>().text
                        = current.ToString();
                    getUI(UITag.InfectionLabel).GetComponent<Text>().text
                        = "[ Infection ]";
                }
            }
            return;
        }

        private void UpdatePotion()
        {
            int current = pc.GetComponent<Potion>().CurrentPotion;

            getUI(UITag.PotionData).GetComponent<Text>().text
                = current.ToString();
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
                if (pc.GetComponent<Power>().HasPower(slot,
                    out power, out isActive))
                {
                    hasPower = true;

                    uiTag = (UITag)Enum.Parse(typeof(UITag), uiName + (int)slot);

                    getUI(uiTag).GetComponent<Text>().text
                        = GetComponent<PowerData>().GetPowerName(power);

                    getUI(uiTag).GetComponent<Text>().color = isActive
                        ? color.PickColor(ColorName.White)
                        : color.PickColor(ColorName.Grey);
                }
            }

            if (hasPower)
            {
                getUI(UITag.PowerLabel).GetComponent<Text>().text = "[ Power ]";
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

            getUI(UITag.Seed).GetComponent<Text>().text = seed;
        }

        private void UpdateStress()
        {
            int current = pc.GetComponent<Stress>().CurrentStress;
            int max = pc.GetComponent<Stress>().MaxStress;

            getUI(UITag.StressData).GetComponent<Text>().text
                = current + "/" + max;
        }

        private void UpdateTurn()
        {
            int current = pc.GetComponent<TurnIndicator>().CurrentTurn;
            int max = pc.GetComponent<TurnIndicator>().Seperator;
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

            getUI(UITag.Turn).GetComponent<Text>().text = sb.ToString();
        }
    }
}
