using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class ActorAI : MonoBehaviour
    {
        private DungeonBoard board;
        private int meleeRange;

        public Command DummyAI()
        {
            if (!GetComponent<AIVision>().CanSeeTarget(SubObjectTag.PC))
            {
                if (GetComponent<NPCMemory>().RememberPC())
                {
                    Debug.Log("Chase.");
                    return Command.Wait;
                }
                return Command.Wait;
            }
            else
            {
                if (board.GetDistance(gameObject, FindObjects.PC) > meleeRange)
                {
                    Debug.Log("Approach.");
                    return Command.Wait;
                }
                Debug.Log("Attack.");
                return Command.Wait;
            }
        }

        private void Awake()
        {
            meleeRange = 1;
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        }
    }
}
