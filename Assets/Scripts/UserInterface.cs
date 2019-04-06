using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public enum UITag
    {
        INVALID,
        Modeline, Status, Message1, Message2, Message3, Message4, Message5,
        SubModeHeader, ViewHelp, Opening, OpeningModeline,

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
        private UIObject getUI;

        private delegate GameObject UIObject(UITag tag);

        private delegate Text UIText(UITag tag);

        private void LateUpdate()
        {
            foreach (IUpdateUI ui in GetComponents<IUpdateUI>())
            {
                ui.PrintText();
            }
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;

            TurnOffUIElements();
        }

        private void TurnOffUIElements()
        {
            UITag[] uits = new UITag[]
            {
                UITag.ExamineMessage, UITag.ExamineModeline,
                UITag.SubModeHeader, UITag.BuyPowerSlotLabel,
                UITag.Log1, UITag.ViewHelp, UITag.Opening,
            };

            foreach (UITag uit in uits)
            {
                getUI(uit).SetActive(false);
            }
        }
    }
}
