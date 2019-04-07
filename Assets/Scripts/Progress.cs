using Fungus.Actor.ObjectManager;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class Progress : MonoBehaviour
    {
        private ActorData actorData;
        private int kill;
        private ProgressData progress;

        public void CountKill(GameObject actor)
        {
            SubObjectTag tag = actor.GetComponent<MetaInfo>().SubTag;

            if (actorData.GetIntData(tag, DataTag.Potion) > 0)
            {
                kill++;
            }
        }

        public bool IsWin()
        {
            // TODO: Change this based on dungeon level.
            return kill >= progress.MaxSoldier;
        }

        private void Awake()
        {
            kill = 0;
        }

        private void Start()
        {
            progress = GetComponent<ProgressData>();
            actorData = GetComponent<ActorData>();
        }
    }
}
