using System;
using System.Collections.Generic;
using UnityEngine;

public enum MainObjectTag { NONE, Building, Actor };

public enum SubObjectTag
{
    NONE,
    Floor, Wall, Pool, Fungus,
    PC, Dummy
};

public class ObjectPool : MonoBehaviour
{
    private Dictionary<SubObjectTag, Stack<GameObject>> pool;
    private int[] position;

    public GameObject CreateObject(MainObjectTag mainTag, SubObjectTag subTag,
        int[] position)
    {
        return CreateObject(mainTag, subTag, position[0], position[1]);
    }

    public GameObject CreateObject(MainObjectTag mainTag, SubObjectTag subTag,
        int x, int y)
    {
        switch (mainTag)
        {
            case MainObjectTag.Actor:
                return CreateActor(subTag, x, y);

            case MainObjectTag.Building:
                return CreateBuilding(subTag, x, y);

            default:
                return null;
        }
    }

    public void StoreObject(GameObject go)
    {
        switch (go.GetComponent<ObjectMetaInfo>().MainTag)
        {
            case MainObjectTag.Actor:
                StoreActor(go);
                break;

            case MainObjectTag.Building:
                StoreBuilding(go);
                break;
        }
    }

    private GameObject CreateActor(SubObjectTag tag, int x, int y)
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

            go.AddComponent<ObjectMetaInfo>();
            go.GetComponent<ObjectMetaInfo>().SetMainTag(MainObjectTag.Actor);
            go.GetComponent<ObjectMetaInfo>().SetSubTag(tag);

            go.AddComponent<FieldOfView>();
            go.AddComponent<FOVRhombus>();
            //go.AddComponent<FOVSimple>();
            go.AddComponent<RenderSprite>();

            go.AddComponent<TileOverlay>();
            go.AddComponent<InternalClock>();

            go.AddComponent<Energy>();
            go.AddComponent<HP>();
            go.AddComponent<Stress>();

            go.AddComponent<Move>();
            go.AddComponent<Attack>();
            go.AddComponent<Defend>();

            go.AddComponent<AIVision>();

            if (tag == SubObjectTag.PC)
            {
                go.AddComponent<PlayerInput>();
                go.AddComponent<PCActions>();
                go.AddComponent<AutoExplore>();
                go.AddComponent<Potion>();
            }
            else
            {
                go.AddComponent<ActorAI>();
                go.AddComponent<NPCActions>().enabled = false;

                go.GetComponent<RenderSprite>().ChangeColor(
                    gameObject.GetComponent<GameColor>().PickColor(
                        ColorName.Black));
            }
        }

        // ObjectPool is attached to GameLogic.
        go.transform.position
            = gameObject.GetComponent<ConvertCoordinates>().Convert(x, y);

        gameObject.GetComponent<ActorBoard>().AddActor(go, x, y);
        gameObject.GetComponent<SchedulingSystem>().AddActor(go);

        return go;
    }

    private GameObject CreateBuilding(SubObjectTag tag, int x, int y)
    {
        GameObject go;

        if (pool[tag].Count > 0)
        {
            go = pool[tag].Pop();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(Resources.Load(tag.ToString()) as GameObject);

            go.AddComponent<ObjectMetaInfo>();
            go.GetComponent<ObjectMetaInfo>().SetMainTag(MainObjectTag.Building);
            go.GetComponent<ObjectMetaInfo>().SetSubTag(tag);

            go.AddComponent<RenderSprite>();
        }

        // ObjectPool is attached to GameLogic.
        go.transform.position
            = gameObject.GetComponent<ConvertCoordinates>().Convert(x, y);

        go.GetComponent<RenderSprite>().ChangeColor(
            gameObject.GetComponent<GameColor>().PickColor(ColorName.Black));

        gameObject.GetComponent<DungeonBoard>().ChangeBlock(go, x, y);
        gameObject.GetComponent<DungeonBoard>().ChangeBlueprint(tag, x, y);

        return go;
    }

    private void Start()
    {
        pool = new Dictionary<SubObjectTag, Stack<GameObject>>();

        foreach (var tag in Enum.GetValues(typeof(SubObjectTag)))
        {
            pool.Add((SubObjectTag)tag, new Stack<GameObject>());
        }
    }

    private void StoreActor(GameObject go)
    {
        position = gameObject.GetComponent<ConvertCoordinates>().Convert(
            go.transform.position);

        gameObject.GetComponent<SchedulingSystem>().RemoveActor(go);

        gameObject.GetComponent<ActorBoard>().RemoveActor(
            position[0], position[1]);

        pool[go.GetComponent<ObjectMetaInfo>().SubTag].Push(go);

        go.SetActive(false);
    }

    private void StoreBuilding(GameObject go)
    {
        position = gameObject.GetComponent<ConvertCoordinates>().Convert(
            go.transform.position);

        gameObject.GetComponent<DungeonBoard>().ChangeBlock(
            null, position[0], position[1]);

        gameObject.GetComponent<DungeonBoard>().ChangeBlueprint(
            SubObjectTag.Floor, position[0], position[1]);

        pool[go.GetComponent<ObjectMetaInfo>().SubTag].Push(go);

        go.SetActive(false);
    }
}
