using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IDeath
    {
        void Bury();

        void Revive();
    }

    public class PCDeath : MonoBehaviour, IDeath
    {
        private StaticActor getActor;
        private SchedulingSystem schedule;

        private delegate GameObject StaticActor(SubObjectTag tag);

        public void Bury()
        {
            schedule.PauseTurn(true);
            gameObject.SetActive(false);

            getActor(SubObjectTag.Guide).transform.position = transform.position;
            getActor(SubObjectTag.Guide).SetActive(true);
        }

        public void Revive()
        {
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
            getActor = FindObjects.GetStaticActor;
        }
    }
}
