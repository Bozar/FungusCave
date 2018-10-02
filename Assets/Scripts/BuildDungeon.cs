using UnityEngine;

public class BuildDungeon : MonoBehaviour
{
    public static readonly int[] buildWalls = { 1, 1, 0, 1, 1 };

    private float moveStep;
    private GameObject newPC;
    private GameObject newWall;
    private GameObject pcTile;
    private Transform relativePosition;
    private GameObject wallTile;

    private void Awake()
    {
        wallTile = Resources.Load("Prefabs/Wall") as GameObject;
        pcTile = Resources.Load("Prefabs/PC") as GameObject;

        relativePosition
            = GameObject.FindGameObjectWithTag("GameManager").transform;

        newPC = Instantiate(pcTile);
        newPC.transform.SetParent(relativePosition);
        newPC.transform.localPosition = new Vector3(0, 0);
        moveStep = newPC.GetComponent<Move>().moveStep;

        for (int i = 0; i < buildWalls.Length; i++)
        {
            if (buildWalls[i] == 1)
            {
                newWall = Instantiate(wallTile);
                newWall.transform.SetParent(relativePosition);
                newWall.transform.localPosition = new Vector3(i * moveStep, 1);
            }
        }
    }
}
