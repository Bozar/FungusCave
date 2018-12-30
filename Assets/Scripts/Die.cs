using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class Die : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private UIMessage message;
        private ObjectPool pool;
        private DungeonTerrain terrain;

        public void Bury()
        {
            if (actor.CheckActorTag(SubObjectTag.PC, gameObject))
            {
                // TODO: Kill PC.
                message.StoreText("PC is dead.");
            }
            else
            {
                // TODO: Death explode. Drop potion.
                int[] pos = coord.Convert(gameObject.transform.position);

                message.StoreText("NPC is dead.");
                terrain.ChangeStatus(true, pos[0], pos[1]);
                pool.StoreObject(gameObject);
            }
        }

        private void Start()
        {
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();
        }
    }
}
