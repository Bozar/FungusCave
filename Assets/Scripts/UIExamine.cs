using Fungus.Actor;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIExamine : MonoBehaviour, IUpdateUI
    {
        private StringBuilder sb;

        public void PrintText()
        {
            if (GetComponent<ObjectPool>().TESTDUMMY == null)
            {
                return;
            }

            GameObject actor = GetComponent<ObjectPool>().TESTDUMMY;

            FindObjects.GetUIObject(UITag.ExamineData).GetComponent<Text>()
                .text = ActorData(actor);

            FindObjects.GetUIObject(UITag.ExamineName).GetComponent<Text>()
                .text = "[ Goblin | Lurker ]";
        }

        public void SwitchExamineMode(bool isExamine)
        {
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(isExamine);
            FindObjects.GetUIObject(UITag.Message).SetActive(!isExamine);
        }

        private string ActorData(GameObject go)
        {
            int hp = go.GetComponent<HP>().CurrentHP;
            int damage = go.GetComponent<Attack>().CurrentDamage;
            int potion = go.GetComponent<ObjectMetaInfo>().DropPotion;
            string infection = go.GetComponent<Infection>().HasInfection()
                ? " | #" : "";

            //> [ 3+ | 5! | 1$ | # ]
            //> [ HP | Damage | Drop | Infected ]
            sb.Remove(0, sb.Length);

            sb.Append("[ ");
            sb.Append(hp);
            sb.Append("+ | ");
            sb.Append(damage);
            sb.Append("! | ");
            sb.Append(potion);
            sb.Append("$");
            sb.Append(infection);
            sb.Append(" ]");

            return sb.ToString();
        }

        private void Awake()
        {
            sb = new StringBuilder();
        }
    }
}
