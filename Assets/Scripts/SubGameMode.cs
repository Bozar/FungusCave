using Fungus.Actor.Render;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem
{
    public class SubGameMode : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private StaticActor getActor;
        private UIObject getUI;
        private SubModeUITag[] sortedHeader;
        private Dictionary<SubModeUITag, string> subModeName;

        private delegate GameObject StaticActor(SubObjectTag tag);

        private delegate GameObject UIObject(UITag tag);

        public enum SubModeUITag { Power, Message, Help, Setting };

        public GameObject ExamineTarget
        {
            get
            {
                int[] pos = coord.Convert(
                    getActor(SubObjectTag.Examiner).transform.position);

                return actor.GetActor(pos[0], pos[1]);
            }
        }

        public string GetSubModeName(SubModeUITag ui)
        {
            return subModeName[ui];
        }

        public void SwitchModeBuyPower(bool switchOn)
        {
            SwitchUINormal(!switchOn);
            SwitchUIBuyPower(switchOn);
            SwitchUISubModeHeader(switchOn);
            PrintSubModeHeader(SubModeUITag.Power);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.BuyPower).SetActive(switchOn);
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

        public void SwitchModeViewMessage(bool switchOn)
        {
            SwitchUINormal(!switchOn);
            SwitchUIViewMessage(switchOn);
            SwitchUISubModeHeader(switchOn);
            PrintSubModeHeader(SubModeUITag.Message);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.ViewMessage).SetActive(switchOn);
        }

        public void SwitchUIExamineMessage(bool switchOn)
        {
            getUI(UITag.ExamineMessage).SetActive(switchOn);
            getUI(UITag.Message).SetActive(!switchOn);
        }

        public void SwitchUIExamineModeline(bool switchOn)
        {
            getUI(UITag.ExamineModeline).SetActive(switchOn);
        }

        private void PrintSubModeHeader(SubModeUITag ui)
        {
            string[] header = new string[sortedHeader.Length];
            string joined;

            for (int i = 0; i < sortedHeader.Length; i++)
            {
                if (ui == sortedHeader[i])
                {
                    header[i] = GetSubModeName(sortedHeader[i]);
                }
                else
                {
                    header[i] = GetComponent<GameColor>().GetColorfulText(
                        GetSubModeName(sortedHeader[i]), ColorName.Grey);
                }
            }

            joined = string.Join(" | ", header);
            joined = "[ " + joined + " ]";

            getUI(UITag.SubModeHeader).GetComponent<Text>().text = joined;
        }

        private void Start()
        {
            actor = GetComponent<ActorBoard>();
            coord = GetComponent<ConvertCoordinates>();
            getUI = FindObjects.GetUIObject;
            getActor = FindObjects.GetStaticActor;

            subModeName = new Dictionary<SubModeUITag, string>
            {
                { SubModeUITag.Power, "Power" },
                { SubModeUITag.Message, "Message" },
                { SubModeUITag.Help, "Help" },
                { SubModeUITag.Setting, "Setting" }
            };

            sortedHeader = new SubModeUITag[]
            {
                SubModeUITag.Power,
                SubModeUITag.Message,
                SubModeUITag.Help,
                SubModeUITag.Setting
            };
        }

        private void SwitchUIBuyPower(bool switchOn)
        {
            getUI(UITag.BuyPowerSlotLabel).SetActive(switchOn);

            GetComponent<UIBuyPower>().ResetCursorPosition();
        }

        private void SwitchUINormal(bool switchOn)
        {
            // UI
            getUI(UITag.Message).SetActive(switchOn);
            getUI(UITag.Status).SetActive(switchOn);

            // Dungeon actors
            RenderSprite[] render = FindObjectsOfType<RenderSprite>();
            foreach (RenderSprite rs in render)
            {
                rs.GetComponentInParent<SpriteRenderer>().enabled = switchOn;
                rs.enabled = switchOn;
            }
        }

        private void SwitchUISubModeHeader(bool switchOn)
        {
            getUI(UITag.SubModeHeader).SetActive(switchOn);
            getUI(UITag.SubModeHeader).GetComponent<Text>().text
                = "Invalid header.";
        }

        private void SwitchUIViewMessage(bool switchOn)
        {
            Debug.Log("UI: ViewMessage");
        }
    }
}
