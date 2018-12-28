using Fungus.Actor;
using Fungus.Actor.ObjectManager;
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
            if (GetComponent<SubGameMode>().ExamineTarget == null)
            {
                return;
            }

            GameObject actor = GetComponent<SubGameMode>().ExamineTarget;

            FindObjects.GetUIObject(UITag.ExamineData).GetComponent<Text>()
                .text = ActorData(actor);

            FindObjects.GetUIObject(UITag.ExamineName).GetComponent<Text>()
                .text = ActorName(actor);
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

        private string ActorName(GameObject go)
        {
            string name = go.GetComponent<ObjectMetaInfo>().Name;

            //> [ Dummy ]
            sb.Remove(0, sb.Length);

            sb.Append("[ ");
            sb.Append(name);
            sb.Append(" ]");

            return sb.ToString();
        }

        private void Awake()
        {
            sb = new StringBuilder();
        }
    }
}
