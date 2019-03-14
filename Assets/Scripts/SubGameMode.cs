using Fungus.Actor.Render;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class SubGameMode : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private UIObject getUI;

        private delegate GameObject UIObject(UITag tag);

        public GameObject ExamineTarget
        {
            get
            {
                int[] pos = coord.Convert(
                    FindObjects.Examiner.transform.position);

                return actor.GetActor(pos[0], pos[1]);
            }
        }

        public void SwitchModeBuyPower(bool switchOn)
        {
            SwitchUINormal(!switchOn);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            //FindObjects.Examiner.transform.position
            //    = FindObjects.PC.transform.position;
            //FindObjects.Examiner.SetActive(switchOn);
        }

        public void SwitchModeExamine(bool switchOn)
        {
            SwitchUIExamineModeline(switchOn);
            SwitchUIExamineMessage(false);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            FindObjects.Examiner.transform.position
                = FindObjects.PC.transform.position;
            FindObjects.Examiner.SetActive(switchOn);
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

        private void Start()
        {
            actor = GetComponent<ActorBoard>();
            coord = GetComponent<ConvertCoordinates>();
            getUI = FindObjects.GetUIObject;
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
    }
}
