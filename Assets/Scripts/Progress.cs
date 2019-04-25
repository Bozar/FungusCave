using Fungus.Actor;
using Fungus.GameSystem.Data;
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
            DungeonLevel current = GetComponent<ProgressData>().GetDungeonLevel();
            DungeonLevel next = GetComponent<ProgressData>().GetNextLevel();
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
            progress = GetComponent<ProgressData>();
            actorData = GetComponent<ActorData>();
        }
    }
}
