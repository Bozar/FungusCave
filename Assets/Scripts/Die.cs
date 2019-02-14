using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor
{
    public class Die : MonoBehaviour
    {
        private ConvertCoordinates coord;
        private UIMessage message;
        private UIModeline modeline;
        private ObjectPool pool;
        private SchedulingSystem schedule;
        private DungeonTerrain terrain;

        public void Bury()
        {
            if (GetComponent<MetaInfo>().IsPC)
            {
                // TODO: Kill PC.
                message.StoreText("PC is dead.");
                modeline.PrintStaticText("Press Space to reload.");

                schedule.PauseTurn(true);
                FindObjects.PC.SetActive(false);

                FindObjects.Guide.transform.position
                    = FindObjects.PC.transform.position;
                FindObjects.Guide.SetActive(true);
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
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
        }
    }
}
