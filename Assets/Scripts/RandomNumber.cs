using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    public System.Random RNG { get; private set; }

    public int Seed { get; set; }

    public void InitializeSeed()
    {
        if (Seed == 0)
        {
            RandomSeed();
        }

        RNG = new System.Random(Seed);
    }

    private void RandomSeed()
    {
        System.Random tempRNG = new System.Random();
        double tempSeed = 0;

        while (tempSeed < 0.1)
        {
            tempSeed = tempRNG.NextDouble();
        }

        Seed = (int)(tempSeed * Mathf.Pow(10, 9));
    }
}
