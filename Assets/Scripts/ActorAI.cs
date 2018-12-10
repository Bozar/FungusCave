using UnityEngine;

namespace Fungus.Actor.AI
{
    public class ActorAI : MonoBehaviour
    {
        public Command DummyAI()
        {
            return Command.Wait;
        }
    }
}
