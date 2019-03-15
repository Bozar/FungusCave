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
        private UIObject getUI;
        private InfectionData infectionData;
        private PotionData potionData;

        private delegate GameObject UIObject(UITag tag);

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

            GameObject actor = GetComponent<SubGameMode>().ExamineTarget;
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

            getUI(UITag.ExamineDamageLabel).GetComponent<Text>().text = label;
            getUI(UITag.ExamineDamageData).GetComponent<Text>().text = data;
        }

        private void PrintFog()
        {
            string label = "Fog";
            getUI(UITag.ExamineFogLabel).GetComponent<Text>().text = label;
        }

        private void PrintHP(GameObject actor)
        {
            string label = "HP";
            string data = actor.GetComponent<HP>().CurrentHP.ToString();

            getUI(UITag.ExamineHPLabel).GetComponent<Text>().text = label;
            getUI(UITag.ExamineHPData).GetComponent<Text>().text = data;
        }

        private void PrintInfection(GameObject actor)
        {
            int duration;
            InfectionTag tag;

            if (actor.GetComponent<Infection>().HasInfection(out tag, out duration))
            {
                getUI(UITag.ExamineInfectionLabel).GetComponent<Text>().text
                        = infectionData.GetInfectionName(tag);
                getUI(UITag.ExamineInfectionData).GetComponent<Text>().text
                    = duration.ToString();
            }
            else
            {
                getUI(UITag.ExamineInfectionLabel).GetComponent<Text>().text = "";
                getUI(UITag.ExamineInfectionData).GetComponent<Text>().text = "";
            }
        }

        private void PrintModeline()
        {
            string modeline = "[ Examine | Esc ]";

            getUI(UITag.ExamineModeline).GetComponent<Text>().text = modeline;
        }

        private void PrintName(GameObject actor)
        {
            getUI(UITag.ExamineName).GetComponent<Text>().text
               = coord.RelativeCoord(actor, StringStyle.NameAndBracket);
        }

        private void PrintPool()
        {
            string label = "Pool";

            if (board.CheckBlock(SubObjectTag.Pool,
                FindObjects.GetStaticActor(SubObjectTag.Examiner)
                .transform.position))
            {
                getUI(UITag.ExaminePoolLabel).GetComponent<Text>().text = label;
            }
            else
            {
                getUI(UITag.ExaminePoolLabel).GetComponent<Text>().text = "";
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

            getUI(UITag.ExaminePotionLabel).GetComponent<Text>().text = label;
            getUI(UITag.ExaminePotionData).GetComponent<Text>().text = data;
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            coord = GetComponent<ConvertCoordinates>();
            actorData = GetComponent<ActorData>();
            potionData = GetComponent<PotionData>();
            infectionData = GetComponent<InfectionData>();

            getUI = FindObjects.GetUIObject;
        }
    }
}
