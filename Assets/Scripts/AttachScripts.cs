using UnityEngine;

public class AttachScripts : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<Singleton>();
        gameObject.AddComponent<BuildDungeon>();
        gameObject.AddComponent<UserInput>();
        gameObject.AddComponent<Move>();
        gameObject.AddComponent<FindObjects>();
    }
}
