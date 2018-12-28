using Fungus.GameSystem.ObjectManager;
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

        public GameObject ExamineTarget { get; private set; }

        public void SwitchModeExamine(bool switchOn)
        {
            if (!switchOn)
            {
                SwitchUIExamine(switchOn);
            }

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            FindObjects.Examiner.transform.position
                = FindObjects.PC.transform.position;
            FindObjects.Examiner.SetActive(switchOn);
        }

        private void LateUpdate()
        {
            if (FindObjects.Examiner.activeSelf)
            {
                int[] pos
                    = coord.Convert(FindObjects.Examiner.transform.position);

                if (actor.HasActor(pos)
                    && !actor.CheckActorTag(SubObjectTag.PC, pos[0], pos[1]))
                {
                    SwitchUIExamine(true);
                    ExamineTarget = actor.GetActor(pos[0], pos[1]);
                }
                else
                {
                    SwitchUIExamine(false);
                    ExamineTarget = null;
                }
            }
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        }

        private void SwitchUIExamine(bool switchOn)
        {
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(switchOn);
            FindObjects.GetUIObject(UITag.Message).SetActive(!switchOn);
        }
    }
}
