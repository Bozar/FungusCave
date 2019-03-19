using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
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
        private StaticActor getActor;
        private PotionData potionData;
        private GameProgress progress;
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
            int potion = actorData.GetIntData(
                target.GetComponent<MetaInfo>().SubTag, DataTag.Potion);
            int bonusPotion = target.GetComponent<Infection>().HasInfection(
                InfectionTag.Mutated) ? potionData.BonusPotion : 0;

            GetComponent<IHP>().RestoreAfterKill();
            GetComponent<Potion>().GainPotion(potion + bonusPotion);
            progress.CountKill(target);

            // TODO: Remove this line.
            Debug.Log(progress.IsWin());
        }

        public void ReviveSelf()
        {
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            progress = FindObjects.GameLogic.GetComponent<GameProgress>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();

            getActor = FindObjects.GetStaticActor;
        }
    }
}
