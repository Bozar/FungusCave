using Fungus.Render;
using UnityEngine;

public class Die : MonoBehaviour
{
    private ActorBoard actor;
    private UIMessage message;
    private ObjectPool pool;

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
            message.StoreText("NPC is dead.");
            pool.StoreObject(gameObject);
        }
    }

    private void Start()
    {
        actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
        message = FindObjects.GameLogic.GetComponent<UIMessage>();
        pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
    }
}
