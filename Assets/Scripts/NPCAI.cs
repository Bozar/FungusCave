using UnityEngine;

public class NPCAI : MonoBehaviour
{
    public Command DummyAI()
    {
        return Command.Wait;
    }
}
