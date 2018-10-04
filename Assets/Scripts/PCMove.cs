using UnityEngine;

public class PCMove : MonoBehaviour
{
    //private GameObject instance;

    private void Start()
    {
        Instantiate(Resources.Load("Interaction"));
    }

    private void Update()
    {
        GameObject.FindGameObjectWithTag("Interaction")
            .GetComponent<Move>().MoveAround(gameObject.transform);
    }
}
