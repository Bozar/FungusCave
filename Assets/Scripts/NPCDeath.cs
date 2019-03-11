using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDeath : MonoBehaviour, IDeath
    {
        private ObjectPool pool;

        public void Bury()
        {
            pool.StoreObject(gameObject);
        }

        public void Revive()
        {
            return;
        }

        private void Start()
        {
            pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
        }
    }
}
