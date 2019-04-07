using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
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
        private ActorData actorData;
        private GameColor color;
        private StaticActor getActor;
        private UIMessage message;
        private UIModeline modeline;
        private PotionData potionData;
        private Progress progress;
        private SchedulingSystem schedule;

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

            GetComponent<Potion>().GainPotion(potion + bonusPotion);
            GetComponent<IHP>().RestoreAfterKill();

            progress.CountKill(target);
            if (progress.IsWin())
            {
                BurySelf();
                color.ChangeObjectColor(getActor(SubObjectTag.Guide),
                    ColorName.Orange);

                message.StoreText("You win.");
                modeline.PrintStaticText("Press Space to reload.");
            }
        }

        public void ReviveSelf()
        {
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            progress = FindObjects.GameLogic.GetComponent<Progress>();
            color = FindObjects.GameLogic.GetComponent<GameColor>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();

            getActor = FindObjects.GetStaticActor;
        }
    }
}
