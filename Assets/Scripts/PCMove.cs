using UnityEngine;

public class PCMove : MonoBehaviour
{
    //private GameObject instance;

    private void Start()
    {
        Instantiate(Resources.Load("Prefabs/Interaction"));
        //instance = Instantiate(Resources.Load("Prefabs/Interaction")) as GameObject;
    }

    private void Update()
    {
        GameObject.FindGameObjectWithTag("Interaction")
            .GetComponent<Move>().MoveAround(gameObject.transform);
    }
}
