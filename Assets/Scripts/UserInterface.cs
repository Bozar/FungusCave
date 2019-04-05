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
        NONE, Modeline, Status, SubModeHeader, ViewHelp,
        Message1, Message2, Message3, Message4, Message5,

        HPData, StressData, PotionData, DamageData,
        Turn, Terrain,
        PowerLabel, PowerData0, PowerData1, PowerData2,
        InfectionLabel,
        InfectionName, InfectionDuration,
        Version, Seed,

        BuyPowerSlotLabel, BuyPowerModeline,
        BuyPowerSlot1, BuyPowerSlot2, BuyPowerSlot3,
        BuyPowerListLabel,
        BuyPowerHP1, BuyPowerHP2,
        BuyPowerEnergy1, BuyPowerEnergy2,
        BuyPowerInfection1, BuyPowerInfection2,
        BuyPowerAtkHP, BuyPowerAtkEnergy, BuyPowerAtkInfection,
        BuyPowerName, BuyPowerStatus,
        BuyPowerCost, BuyPowerPCHP, BuyPowerDescription,

        Log1, Log2, Log3, Log4, Log5, Log6, Log7, Log8, Log9, Log10,
        Log11, Log12, Log13, Log14, Log15, Log16,

        ExamineMessage, ExamineModeline, ExamineName,
        ExamineHPLabel, ExamineHPData,
        ExamineDamageLabel, ExamineDamageData,
        ExaminePotionLabel, ExaminePotionData,
        ExamineInfectionLabel, ExamineInfectionData,
        ExaminePoolLabel, ExamineFogLabel
    };

    public interface IUpdateUI
    {
        // Static text does not update automatically.
        void PrintStaticText();

        void PrintStaticText(string text);

        // See below, LateUpdate().
        void PrintText();
    }

    public class UserInterface : MonoBehaviour
    {
        private DungeonBoard board;
        private GameColor color;
        private UIText getText;
        private UIObject getUI;
        private InfectionData infectionData;
        private GameObject pc;
        private StringBuilder sb;

        private delegate GameObject UIObject(UITag tag);

        private delegate Text UIText(UITag tag);

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

            UpdateDamage();
            UpdateEnvironment();
            UpdateHP();
            UpdateInfection();

            UpdatePotion();
            UpdatePower();
            UpdateSeed();
            UpdateStress();

            UpdateTurn();
            UpdateVersion();

            foreach (IUpdateUI ui in GetComponents<IUpdateUI>())
            {
                ui.PrintText();
            }
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            color = GetComponent<GameColor>();
            infectionData = GetComponent<InfectionData>();

            getUI = FindObjects.GetUIObject;
            getText = FindObjects.GetUIText;

            TurnOffUIElements();
        }

        private void TurnOffUIElements()
        {
            getUI(UITag.ExamineMessage).SetActive(false);
            getUI(UITag.ExamineModeline).SetActive(false);
            getUI(UITag.SubModeHeader).SetActive(false);
            getUI(UITag.BuyPowerSlotLabel).SetActive(false);
            getUI(UITag.Log1).SetActive(false);
            getUI(UITag.ViewHelp).SetActive(false);
        }

        private void UpdateDamage()
        {
            int current = pc.GetComponent<IDamage>().CurrentDamage;

            getText(UITag.DamageData).text = current.ToString();
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

            getText(UITag.Terrain).text = sb.ToString();
        }

        private void UpdateHP()
        {
            int current = pc.GetComponent<HP>().CurrentHP;
            int max = pc.GetComponent<HP>().MaxHP;

            getText(UITag.HPData).text = current + "/" + max;
        }

        private void UpdateInfection()
        {
            string label = "[ Infection ]";
            int duration;
            InfectionTag tag;

            if (pc.GetComponent<Infection>().HasInfection(out tag, out duration))
            {
                getText(UITag.InfectionName).text
                    = infectionData.GetInfectionName(tag);
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
            int current = pc.GetComponent<Potion>().CurrentPotion;

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
                if (pc.GetComponent<Power>().HasPower(slot,
                    out power, out isActive))
                {
                    hasPower = true;

                    uiTag = (UITag)Enum.Parse(typeof(UITag), uiName + (int)slot);

                    getText(uiTag).text
                        = GetComponent<PowerData>().GetPowerName(power);

                    getText(uiTag).color = isActive
                        ? color.PickColor(ColorName.White)
                        : color.PickColor(ColorName.Grey);
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
            int current = pc.GetComponent<Stress>().CurrentStress;
            int max = pc.GetComponent<Stress>().MaxStress;

            getText(UITag.StressData).text = current + "/" + max;
        }

        private void UpdateTurn()
        {
            int current = pc.GetComponent<TurnIndicator>().CurrentTurn;
            int max = pc.GetComponent<TurnIndicator>().BarSpearator;
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
