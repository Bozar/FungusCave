using Fungus.Actor;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIExamine : MonoBehaviour, IUpdateUI
    {
        private DungeonBoard board;
        private StringBuilder sb;

        public void PrintStaticText()
        {
            return;
        }

        public void PrintStaticText(string text)
        {
            return;
        }

        public void PrintText()
        {
            FindObjects.GetUIObject(UITag.ExamineModeline).GetComponent<Text>()
                .text = "[ Examine | Esc ]";

            GameObject actor = GetComponent<SubGameMode>().ExamineTarget;

            if (actor == null)
            {
                return;
            }

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
            //bool hasFog = true;
            bool hasFog = false;

            //> [ Dummy | = | % ]
            sb.Remove(0, sb.Length);

            sb.Append("[ ");
            sb.Append(name);

            if (board.CheckBlock(SubObjectTag.Pool,
                FindObjects.Examiner.transform.position))
            {
                sb.Append(" | ");
                sb.Append(FindObjects.IconPool);
            }

            if (hasFog)
            {
                sb.Append(" | ");
                sb.Append(FindObjects.IconFog);
            }

            sb.Append(" ]");

            return sb.ToString();
        }

        private void Awake()
        {
            sb = new StringBuilder();
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
        }
    }
}
