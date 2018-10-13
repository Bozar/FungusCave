using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    private int seed;

    public System.Random RNG { get; private set; }

    private void Awake()
    {
        seed = 12345;
        RNG = new System.Random(seed);
    }
}
