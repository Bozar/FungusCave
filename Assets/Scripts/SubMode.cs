using Fungus.Actor;
using Fungus.Actor.Render;
using Fungus.Actor.Turn;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem
{
    public enum SubModeUITag { INVALID, Power, Log, Help, Setting };

    public class SubMode : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private StaticActor getActor;
        private UIObject getUI;

        private delegate GameObject StaticActor(SubObjectTag tag);

        private delegate GameObject UIObject(UITag tag);

        public GameObject ExamineTarget
        {
            get
            {
                int[] pos = coord.Convert(
                    getActor(SubObjectTag.Examiner).transform.position);

                return actor.GetActor(pos[0], pos[1]);
            }
        }

        public void SwitchModeBuyPower(bool switchOn)
        {
            SwitchModeNormal(!switchOn);
            SwitchUIHeader(switchOn);
            SwitchUIBuyPower(switchOn);

            getActor(SubObjectTag.BuyPower).GetComponent<BuyPowerStatus>()
                .SetIsActive(switchOn);
            getActor(SubObjectTag.BuyPower).GetComponent<BuyPowerAction>()
                .enabled = switchOn;
        }

        public void SwitchModeExamine(bool switchOn)
        {
            SwitchUIExamineModeline(switchOn);
            SwitchUIExamineMessage(false);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.Examiner).transform.position
                = FindObjects.PC.transform.position;
            getActor(SubObjectTag.Examiner).SetActive(switchOn);
        }

        public void SwitchModeHelp(bool switchOn)
        {
            SwitchModeNormal(!switchOn);
            SwitchUIHeader(switchOn);
            SwitchUIHelp(switchOn);

            getActor(SubObjectTag.ViewHelp).GetComponent<ViewHelpStatus>()
                .SetIsActive(switchOn);
            getActor(SubObjectTag.ViewHelp).GetComponent<ViewHelpAction>()
               .enabled = switchOn;
        }

        public void SwitchModeLog(bool switchOn)
        {
            SwitchModeNormal(!switchOn);
            SwitchUIHeader(switchOn);
            SwitchUILog(switchOn);

            getActor(SubObjectTag.ViewLog).GetComponent<ViewLogStatus>()
                .SetIsActive(switchOn);
            getActor(SubObjectTag.ViewLog).GetComponent<ViewLogAction>()
               .enabled = switchOn;
        }

        public void SwitchModeOpening(bool switchOn)
        {
            SwitchModeNormal(!switchOn);
            SwitchUIOpening(switchOn);

            getActor(SubObjectTag.Opening).GetComponent<OpeningAction>()
                .enabled = switchOn;
        }

        public void SwitchModeSetting(bool switchOn)
        {
            SwitchModeNormal(!switchOn);
            SwitchUIHeader(switchOn);
            SwitchUISetting(switchOn);

            SettingStatus status
                = FindObjects.GetStaticActor(SubObjectTag.Setting)
                .GetComponent<SettingStatus>();

            if (switchOn)
            {
                status.IsModified = false;
            }

            if (!switchOn && status.IsModified)
            {
                GetComponent<GameSetting>().Save();
            }

            getActor(SubObjectTag.Setting).GetComponent<SettingStatus>()
                .SetIsActive(switchOn);
            getActor(SubObjectTag.Setting).GetComponent<SettingAction>()
                .enabled = switchOn;
        }

        public void SwitchUIExamineMessage(bool switchOn)
        {
            getUI(UITag.ExamineMessage).SetActive(switchOn);
            getUI(UITag.Message1).SetActive(!switchOn);
        }

        public void SwitchUIExamineModeline(bool switchOn)
        {
            getUI(UITag.ExamineModeline).SetActive(switchOn);
        }

        private void Start()
        {
            actor = GetComponent<ActorBoard>();
            coord = GetComponent<ConvertCoordinates>();
            getUI = FindObjects.GetUIObject;
            getActor = FindObjects.GetStaticActor;
        }

        private void SwitchModeNormal(bool switchOn)
        {
            SwitchUINormal(switchOn);

            GetComponent<SchedulingSystem>().PauseTurn(!switchOn);
        }

        private void SwitchUIBuyPower(bool switchOn)
        {
            getUI(UITag.BuyPowerSlotLabel).SetActive(switchOn);

            GetComponent<UIBuyPower>().ResetCursorPosition();
        }

        private void SwitchUIHeader(bool switchOn)
        {
            getUI(UITag.SubModeHeader).SetActive(switchOn);
            getUI(UITag.SubModeHeader).GetComponent<Text>().text
                = "Invalid header.";
        }

        private void SwitchUIHelp(bool switchOn)
        {
            getUI(UITag.ViewHelp).SetActive(switchOn);
            getUI(UITag.ViewHelp).GetComponent<Text>().text
                = GetComponent<GameText>().GetHelp();
        }

        private void SwitchUILog(bool switchOn)
        {
            getUI(UITag.Log1).SetActive(switchOn);
        }

        private void SwitchUINormal(bool switchOn)
        {
            // UI
            getUI(UITag.Message1).SetActive(switchOn);
            getUI(UITag.Status).SetActive(switchOn);

            // Dungeon actors
            RenderSprite[] render = FindObjectsOfType<RenderSprite>();
            foreach (RenderSprite rs in render)
            {
                rs.GetComponentInParent<SpriteRenderer>().enabled = switchOn;
                rs.enabled = switchOn;
            }
        }

        private void SwitchUIOpening(bool switchOn)
        {
            getUI(UITag.Opening).SetActive(switchOn);
        }

        private void SwitchUISetting(bool switchOn)
        {
            getUI(UITag.SettingCursor1).SetActive(switchOn);
        }
    }
}
