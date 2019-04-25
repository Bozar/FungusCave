using Fungus.Actor;
using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class DungeonProgress : MonoBehaviour
    {
        private ActorData actorData;
        private int kill;
        private DungeonProgressData progress;

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
            DungeonLevel current = GetComponent<DungeonProgressData>()
                .GetDungeonLevel();
            DungeonLevel next = GetComponent<DungeonProgressData>()
                .GetNextLevel();

            return (current == next) && LevelCleared();
        }

        public bool LevelCleared()
        {
            return kill >= progress.MaxSoldier;
        }

        private void Awake()
        {
            kill = 0;
        }

        private void Start()
        {
            progress = GetComponent<DungeonProgressData>();
            actorData = GetComponent<ActorData>();
        }
    }
}
