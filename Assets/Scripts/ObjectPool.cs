using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.Actor.InputManager;
using Fungus.Actor.ObjectManager;
using Fungus.Actor.Render;
using Fungus.Actor.Turn;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    // Dopplegangers are substitutions for PC under certain situations: examine
    // mode, help menu, etc.
    public enum MainObjectTag { NONE, Building, Actor, Doppleganger };

    public enum SubObjectTag
    {
        NONE, DEFAULT,
        Floor, Wall, Pool, Fungus,
        PC, Examiner, Guide,
        Dummy
    };

    public class ObjectPool : MonoBehaviour
    {
        private Dictionary<SubObjectTag, Stack<GameObject>> pool;
        private int[] position;

        public GameObject CreateObject(
            MainObjectTag mainTag, SubObjectTag subTag, int[] position)
        {
            return CreateObject(mainTag, subTag, position[0], position[1]);
        }

        public GameObject CreateObject(
            MainObjectTag mainTag, SubObjectTag subTag, int x, int y)
        {
            switch (mainTag)
            {
                case MainObjectTag.Actor:
                    return CreateActor(subTag, x, y);

                case MainObjectTag.Building:
                    return CreateBuilding(subTag, x, y);

                case MainObjectTag.Doppleganger:
                    return CreateDoppleganger(subTag, x, y);

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

                case MainObjectTag.Doppleganger:
                    StoreDoppleganger(go);
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

                SetTags(MainObjectTag.Actor, tag, go);

                go.AddComponent<FieldOfView>();
                go.AddComponent<FOVRhombus>();
                //go.AddComponent<FOVSimple>();
                go.AddComponent<RenderSprite>();

                //go.AddComponent<TileOverlay>();
                go.AddComponent<InternalClock>();

                go.AddComponent<Energy>();
                go.AddComponent<HP>();
                go.AddComponent<Stress>();
                go.AddComponent<Infection>();

                go.AddComponent<AutoExplore>();
                go.AddComponent<MoveActor>();
                go.AddComponent<Attack>();
                go.AddComponent<Die>();
                go.AddComponent<Power>();

                go.AddComponent<AIVision>();

                if (tag == SubObjectTag.PC)
                {
                    go.AddComponent<PlayerInput>();
                    go.AddComponent<PCActions>();
                    go.AddComponent<Potion>();
                    go.AddComponent<TurnIndicator>();
                    go.AddComponent<PCAutoExplore>();
                    go.AddComponent<InputNormal>();
                    go.AddComponent<PCDamage>();

                    FindObjects.PC = go;
                }
                else
                {
                    go.AddComponent<ActorAI>();
                    go.AddComponent<NPCAutoExplore>();
                    go.AddComponent<NPCDamage>();
                    //go.AddComponent<NPCMemory>();
                    go.AddComponent<NPCActions>().enabled = false;

                    // NOTE: Change sprite.
                    //UnityEngine.Object[] test
                    //      = Resources.LoadAll("curses_vector_32x48", typeof(Sprite));
                    //go.GetComponent<SpriteRenderer>().sprite = (Sprite)test[5];
                }
            }

            // ObjectPool is attached to GameLogic.
            go.transform.position
                = GetComponent<ConvertCoordinates>().Convert(x, y);

            GetComponent<ActorBoard>().AddActor(go, x, y);
            GetComponent<SchedulingSystem>().AddActor(go);

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

                SetTags(MainObjectTag.Building, tag, go);
                go.AddComponent<RenderSprite>();
            }

            // ObjectPool is attached to GameLogic.
            go.transform.position
                = GetComponent<ConvertCoordinates>().Convert(x, y);

            GetComponent<DungeonBoard>().ChangeBlock(go, x, y);
            GetComponent<DungeonBoard>().ChangeBlueprint(tag, x, y);

            return go;
        }

        private GameObject CreateDoppleganger(SubObjectTag tag, int x, int y)
        {
            GameObject go;

            go = Instantiate(Resources.Load(tag.ToString()) as GameObject);

            SetTags(MainObjectTag.Actor, tag, go);
            go.AddComponent<PlayerInput>();

            go.transform.position
               = GetComponent<ConvertCoordinates>().Convert(x, y);
            go.SetActive(false);

            switch (tag)
            {
                // Examine mode.
                case SubObjectTag.Examiner:
                    go.AddComponent<ExaminerActions>();
                    go.AddComponent<MoveExamineMarker>();
                    go.AddComponent<PCSortTargets>();
                    go.AddComponent<InputExamine>();

                    FindObjects.Examiner = go;
                    break;

                // Load screen.
                case SubObjectTag.Guide:
                    go.AddComponent<GuideActions>();
                    go.AddComponent<InputGuide>();

                    FindObjects.Guide = go;
                    break;
            }

            return go;
        }

        private void SetTags(MainObjectTag main, SubObjectTag sub, GameObject go)
        {
            go.AddComponent<ObjectMetaInfo>();
            go.GetComponent<ObjectMetaInfo>().SetMainTag(main);
            go.GetComponent<ObjectMetaInfo>().SetSubTag(sub);
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
            position = GetComponent<ConvertCoordinates>().Convert(
                go.transform.position);

            GetComponent<SchedulingSystem>().RemoveActor(go);

            GetComponent<ActorBoard>().RemoveActor(
                position[0], position[1]);

            pool[go.GetComponent<ObjectMetaInfo>().SubTag].Push(go);

            go.SetActive(false);
        }

        private void StoreBuilding(GameObject go)
        {
            position = GetComponent<ConvertCoordinates>().Convert(
                go.transform.position);

            GetComponent<DungeonBoard>().ChangeBlock(
                null, position[0], position[1]);

            GetComponent<DungeonBoard>().ChangeBlueprint(
                SubObjectTag.Floor, position[0], position[1]);

            pool[go.GetComponent<ObjectMetaInfo>().SubTag].Push(go);

            go.SetActive(false);
        }

        private void StoreDoppleganger(GameObject go)
        {
            pool[go.GetComponent<ObjectMetaInfo>().SubTag].Push(go);
            go.SetActive(false);
        }
    }
}
