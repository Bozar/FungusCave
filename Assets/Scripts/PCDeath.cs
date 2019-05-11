using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IDeath
    {
        // Do something after actor is dead.
        void BurySelf();

        // Do something after actor kills the target.
        void DefeatTarget(GameObject target);

        void ReviveSelf();
    }

    public class PCDeath : MonoBehaviour, IDeath
    {
        private readonly string node = "EnterExit";

        private ActorData actorData;
        private GameColor color;
        private ConvertCoordinates coord;
        private NourishFungus fungus;
        private StaticActor getActor;
        private UIMessage message;
        private UIModeline modeline;
        private PotionData potionData;
        private DungeonProgress progress;
        private SchedulingSystem schedule;
        private GameText text;

        private delegate GameObject StaticActor(SubObjectTag tag);

        public void BurySelf()
        {
            schedule.PauseTurn(true);
            gameObject.SetActive(false);

            getActor(SubObjectTag.Guide).transform.position = transform.position;
            getActor(SubObjectTag.Guide).SetActive(true);
        }

        public void DefeatTarget(GameObject target)
        {
            SubObjectTag tag = target.GetComponent<MetaInfo>().SubTag;
            int potion = actorData.GetIntData(tag, DataTag.Potion);
            int bonusPotion = target.GetComponent<Infection>().HasInfection(
                InfectionTag.Mutated) ? potionData.BonusPotion : 0;
            string victory;

            GetComponent<Potion>().GainPotion(potion + bonusPotion);

            progress.CountKill(target);
            if (progress.IsWin())
            {
                BurySelf();
                color.ChangeObjectColor(getActor(SubObjectTag.Guide),
                    ColorName.Orange);

                victory = text.GetStringData(node, "Win");
                victory = color.GetColorfulText(victory, ColorName.Green);
                message.StoreText(victory);
                modeline.PrintStaticText(text.GetStringData(node, "ReloadWin"));
            }
            else if (progress.LevelCleared())
            {
                BurySelf();
                color.ChangeObjectColor(getActor(SubObjectTag.Guide),
                    ColorName.Orange);

                victory = text.GetStringData(node, "Level");
                victory = color.GetColorfulText(victory, ColorName.Green);
                message.StoreText(victory);
                modeline.PrintStaticText(text.GetStringData(node, "Continue"));
            }
        }

        public void ReviveSelf()
        {
            fungus.CountDown(new ActorInfoEventArgs(
                GetComponent<MetaInfo>().SubTag,
                coord.Convert(transform.position)));
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            progress = FindObjects.GameLogic.GetComponent<DungeonProgress>();
            color = FindObjects.GameLogic.GetComponent<GameColor>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            fungus = FindObjects.GameLogic.GetComponent<NourishFungus>();
            text = FindObjects.GameLogic.GetComponent<GameText>();

            getActor = FindObjects.GetStaticActor;
        }
    }
}
