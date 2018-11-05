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

    private void Awake()
    {
        pool = new Dictionary<ObjectTag, Stack<GameObject>>();

        foreach (var tag in Enum.GetValues(typeof(ObjectTag)))
        {
            pool.Add((ObjectTag)tag, new Stack<GameObject>());
        }
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

            return go;
        }

        go = Instantiate(Resources.Load(tag.ToString()) as GameObject);

        // TODO: Remove PC specific components.
        go.AddComponent<PlayerInput>();
        go.AddComponent<PCActions>().enabled = false;

        go.AddComponent<ObjectMetaInfo>();
        go.GetComponent<ObjectMetaInfo>().TagName = tag;

        // ObjectPool is attached to GameLogic.
        go.transform.position
            = gameObject.GetComponent<ConvertCoordinates>().Convert(x, y);

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
}
