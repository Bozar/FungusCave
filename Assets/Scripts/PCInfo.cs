using UnityEngine;

public class PCInfo : ActorTemplate
{
    private void Start()
    {
        ActorName = "Player Character";
        ActorType = "Mark II";

        if (!InfoPrinted)
        {
            Debug.Log(ActorName);
            Debug.Log(ActorType);

            InfoPrinted = true;
        }
    }
}
