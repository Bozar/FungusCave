using Fungus.Actor;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIExamine : MonoBehaviour, IUpdateUI
    {
        private ActorData actorData;
        private DungeonBoard board;
        private ConvertCoordinates coord;
        private PotionData potionData;
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

            FindObjects.GetUIObject(UITag.ExamineName).GetComponent<Text>()
                .text = coord.RelativeCoord(actor, StringStyle.NameAndBracket);
        }

        private string ActorData(GameObject go)
        {
            int hp = go.GetComponent<HP>().CurrentHP;
            int damage = go.GetComponent<IDamage>().CurrentDamage;

            int potion = actorData.GetIntData(
                go.GetComponent<MetaInfo>().SubTag, DataTag.Potion);
            int bonusPotion = go.GetComponent<Infection>().HasInfection(
                InfectionTag.Mutated) ? potionData.BonusPotion : 0;

            List<InfectionTag> infectionNames
                = go.GetComponent<Infection>().InfectionNames;
            string infection = (infectionNames.Count > 0)
                ? (" | " + infectionNames[0]) : "";

            //> [ 3+ | 5! | 1$ | # ]
            //> [ HP | Damage | Drop | Infected ]
            sb.Remove(0, sb.Length);

            sb.Append("[ ");
            sb.Append(hp);
            sb.Append("+ | ");
            sb.Append(damage);
            sb.Append("! | ");
            sb.Append(potion + bonusPotion);
            sb.Append("$");
            sb.Append(infection);
            sb.Append(" ]");

            return sb.ToString();
        }

        private string ActorName(GameObject go)
        {
            string name = actorData.GetStringData(
                go.GetComponent<MetaInfo>().SubTag, DataTag.ActorName);
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
            coord = GetComponent<ConvertCoordinates>();
            actorData = GetComponent<ActorData>();
            potionData = GetComponent<PotionData>();
        }
    }
}
