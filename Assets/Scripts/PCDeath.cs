using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
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

        private delegate GameObject StaticActor(SubObjectTag tag);

        public void Bury()
        {
            getActor(SubObjectTag.Guide).transform.position = transform.position;
            gameObject.SetActive(false);
            getActor(SubObjectTag.Guide).SetActive(true);
        }

        public void Revive()
        {
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            getActor = FindObjects.GetStaticActor;
        }
    }
}
