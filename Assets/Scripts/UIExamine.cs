using Fungus.Actor;
using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIExamine : MonoBehaviour, IUpdateUI
    {
        private ActorData actorData;
        private DungeonBoard board;
        private ConvertCoordinates coord;
        private UIText getUI;
        private InfectionData infectionData;
        private PotionData potionData;

        private delegate Text UIText(UITag tag);

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
            PrintModeline();

            GameObject actor = GetComponent<SubMode>().ExamineTarget;
            if (actor == null)
            {
                return;
            }

            PrintName(actor);
            PrintHP(actor);
            PrintDamage(actor);
            PrintPotion(actor);

            PrintInfection(actor);
            PrintPool();
            //PrintFog();
        }

        private void PrintDamage(GameObject actor)
        {
            string label = "Damage";
            string data = actor.GetComponent<IDamage>().CurrentDamage.ToString();

            getUI(UITag.ExamineDamageLabel).text = label;
            getUI(UITag.ExamineDamageData).text = data;
        }

        private void PrintFog()
        {
            string label = "Fog";
            getUI(UITag.ExamineFogLabel).text = label;
        }

        private void PrintHP(GameObject actor)
        {
            string label = "HP";
            string data = actor.GetComponent<HP>().CurrentHP.ToString();

            getUI(UITag.ExamineHPLabel).text = label;
            getUI(UITag.ExamineHPData).text = data;
        }

        private void PrintInfection(GameObject actor)
        {
            int duration;
            InfectionTag tag;

            if (actor.GetComponent<Infection>().HasInfection(
                out tag, out duration))
            {
                getUI(UITag.ExamineInfectionLabel).text
                        = infectionData.GetInfectionName(tag);
                getUI(UITag.ExamineInfectionData).text = duration.ToString();
            }
            else
            {
                getUI(UITag.ExamineInfectionLabel).text = "";
                getUI(UITag.ExamineInfectionData).text = "";
            }
        }

        private void PrintModeline()
        {
            string modeline = "[ Examine | Esc ]";

            getUI(UITag.ExamineModeline).text = modeline;
        }

        private void PrintName(GameObject actor)
        {
            getUI(UITag.ExamineName).text
               = coord.RelativeCoord(actor, StringStyle.NameAndBracket);
        }

        private void PrintPool()
        {
            string label = "Pool";

            if (board.CheckBlock(SubObjectTag.Pool,
                FindObjects.GetStaticActor(SubObjectTag.Examiner)
                .transform.position))
            {
                getUI(UITag.ExaminePoolLabel).text = label;
            }
            else
            {
                getUI(UITag.ExaminePoolLabel).text = "";
            }
        }

        private void PrintPotion(GameObject actor)
        {
            string label = "Potion";

            int basic = actorData.GetIntData(
                actor.GetComponent<MetaInfo>().SubTag, DataTag.Potion);
            int bonus = actor.GetComponent<Infection>().HasInfection(
                InfectionTag.Mutated) ? potionData.BonusPotion : 0;

            string data = (basic + bonus).ToString();

            getUI(UITag.ExaminePotionLabel).text = label;
            getUI(UITag.ExaminePotionData).text = data;
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            coord = GetComponent<ConvertCoordinates>();
            actorData = GetComponent<ActorData>();
            potionData = GetComponent<PotionData>();
            infectionData = GetComponent<InfectionData>();

            getUI = FindObjects.GetUIText;
        }
    }
}
