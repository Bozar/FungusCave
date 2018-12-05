using Fungus.GameSystem;
using UnityEngine;

public class ActorAI : MonoBehaviour
{
    public Command DummyAI()
    {
        return Command.Wait;
    }
}
