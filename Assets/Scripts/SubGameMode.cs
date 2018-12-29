using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class SubGameMode : MonoBehaviour
    {
        public GameObject ExamineTarget { get; set; }

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
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(switchOn);
            FindObjects.GetUIObject(UITag.Message).SetActive(!switchOn);
        }

        public void SwitchUIExamineModeline(bool switchOn)
        {
            FindObjects.GetUIObject(UITag.ExamineModeline).SetActive(switchOn);
        }
    }
}
