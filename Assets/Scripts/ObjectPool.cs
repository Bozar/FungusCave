using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTag
{
    TEMP, TActor, TBuilding,
    PC, Dummy,
    Wall, Pool, Fungus
};

public class ObjectPool : MonoBehaviour
{
    private Dictionary<ObjectTag, Stack<GameObject>> pool;

    public GameObject CreateObject(ObjectTag tag, int x, int y)
    {
        switch (ClassifyTag(tag))
        {
            case ObjectTag.TActor:
                return CreateNPC(tag, x, y);

            case ObjectTag.TBuilding:
                return null;

            case ObjectTag.PC:
                return CreatePC(x, y);

            default:
                return null;
        }
    }

    public void StoreObject(GameObject go)
    {
        switch (ClassifyTag(go))
        {
            case ObjectTag.TActor:
            case ObjectTag.PC:
                StoreActor(go);
                break;

            case ObjectTag.TBuilding:
                break;
        }
    }

    private void Awake()
    {
        pool = new Dictionary<ObjectTag, Stack<GameObject>>();

        foreach (var tag in Enum.GetValues(typeof(ObjectTag)))
        {
            pool.Add((ObjectTag)tag, new Stack<GameObject>());
        }
    }

    private ObjectTag ClassifyTag(GameObject go)
    {
        return ClassifyTag(go.GetComponent<ObjectMetaInfo>().TagName);
    }

    private ObjectTag ClassifyTag(ObjectTag tag)
    {
        switch (tag)
        {
            case ObjectTag.Wall:
            case ObjectTag.Pool:
            case ObjectTag.Fungus:
                return ObjectTag.TBuilding;

            case ObjectTag.Dummy:
                return ObjectTag.TActor;

            default:
                return tag;
        }
    }

    private GameObject CreateNPC(ObjectTag tag, int x, int y)
    {
        GameObject go;

        if (pool[tag].Count > 0)
        {
            // TODO: Refresh the actor.
            go = pool[tag].Pop();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(Resources.Load(tag.ToString()) as GameObject);

            // TODO: Remove PC specific components.
            go.AddComponent<PlayerInput>();
            go.AddComponent<PCActions>().enabled = false;

            go.AddComponent<ObjectMetaInfo>();
            go.GetComponent<ObjectMetaInfo>().TagName = tag;

            go.AddComponent<FieldOfView>();
            go.AddComponent<FOVRhombus>();
            //go.AddComponent<FOVSimple>();
            go.AddComponent<RenderSprite>();

            go.AddComponent<TileOverlay>();
            go.AddComponent<InternalClock>();

            go.AddComponent<Energy>();
            go.AddComponent<Move>();
            go.AddComponent<Attack>();
            go.AddComponent<Defend>();
        }

        // ObjectPool is attached to GameLogic.
        go.transform.position
            = gameObject.GetComponent<ConvertCoordinates>().Convert(x, y);
        gameObject.GetComponent<ActorBoard>().AddActor(go, x, y);
        gameObject.GetComponent<SchedulingSystem>().AddActor(go);

        return go;
    }

    private GameObject CreatePC(int x, int y)
    {
        GameObject go;

        // TODO: Add PC specific components.
        go = CreateNPC(ObjectTag.PC, x, y);
        go.GetComponent<PCActions>().enabled = true;

        return go;
    }

    private void StoreActor(GameObject go)
    {
        gameObject.GetComponent<SchedulingSystem>().RemoveActor(go);

        gameObject.GetComponent<ActorBoard>().RemoveActor(
            gameObject.GetComponent<ConvertCoordinates>()
            .Convert(go.transform.position)[0],
            gameObject.GetComponent<ConvertCoordinates>()
            .Convert(go.transform.position)[1]);

        pool[go.GetComponent<ObjectMetaInfo>().TagName].Push(go);

        go.SetActive(false);
    }
}
