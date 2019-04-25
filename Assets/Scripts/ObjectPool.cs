using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.Actor.FOV;
using Fungus.Actor.InputManager;
using Fungus.Actor.Render;
using Fungus.Actor.Turn;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem
{
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
            switch (go.GetComponent<MetaInfo>().MainTag)
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

                go.AddComponent<AIVision>();
                go.AddComponent<AutoExplore>();
                go.AddComponent<Attack>();

                go.AddComponent<Energy>();
                go.AddComponent<FieldOfView>();
                go.AddComponent<FOVRhombus>();
                //go.AddComponent<FOVSimple>();

                go.AddComponent<HP>();
                go.AddComponent<Infection>();
                go.AddComponent<InfectionRate>();
                go.AddComponent<InternalClock>();

                go.AddComponent<MoveActor>();
                go.AddComponent<RenderSprite>();
                go.AddComponent<Stress>();
                //go.AddComponent<TileOverlay>();

                if (tag == SubObjectTag.PC)
                {
                    go.AddComponent<InputNormal>();

                    go.AddComponent<PCAction>();
                    go.AddComponent<PCAutoExplore>();
                    go.AddComponent<PCDamage>();
                    go.AddComponent<PCDeath>();

                    go.AddComponent<PCEnergy>();
                    go.AddComponent<PCHP>();
                    go.AddComponent<PCInfection>();
                    go.AddComponent<PCMessage>();

                    go.AddComponent<PlayerInput>();
                    go.AddComponent<Power>();
                    go.AddComponent<Potion>();
                    go.AddComponent<TurnIndicator>();

                    FindObjects.PC = go;
                }
                else
                {
                    go.AddComponent<ActorAI>();
                    go.AddComponent<NPCAutoExplore>();
                    go.AddComponent<NPCDamage>();
                    go.AddComponent<NPCDeath>();

                    go.AddComponent<NPCEnergy>();
                    go.AddComponent<NPCHP>();
                    go.AddComponent<NPCInfection>();
                    go.AddComponent<NPCMessage>();
                    //go.AddComponent<NPCMemory>();

                    go.AddComponent<NPCAction>().enabled = false;

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
            GameObject go
                = Instantiate(Resources.Load(tag.ToString()) as GameObject);
            FindObjects.SetStaticActor(tag, go);

            go.transform.position
                = GetComponent<ConvertCoordinates>().Convert(x, y);
            SetTags(MainObjectTag.Actor, tag, go);
            go.AddComponent<PlayerInput>();

            switch (tag)
            {
                // These actors appear on screen even when activated.
                case SubObjectTag.Examiner:
                    go.AddComponent<ExaminerAction>();
                    go.AddComponent<InputExamine>();
                    go.AddComponent<MoveExamineMarker>();
                    go.AddComponent<PCSortTargets>();

                    go.SetActive(false);
                    break;

                case SubObjectTag.Guide:
                    go.AddComponent<GuideAction>();
                    go.AddComponent<InputGuide>();

                    go.SetActive(false);
                    break;

                // These actors do not appear on screen even when activated.
                case SubObjectTag.BuyPower:
                    go.AddComponent<BuyPowerAction>();
                    go.AddComponent<BuyPowerStatus>();
                    go.AddComponent<InputHeader>();
                    go.AddComponent<InputBuyPower>();

                    go.GetComponent<BuyPowerAction>().enabled = false;
                    break;

                case SubObjectTag.ViewHelp:
                    go.AddComponent<ViewHelpAction>();
                    go.AddComponent<ViewHelpStatus>();
                    go.AddComponent<InputHeader>();

                    go.GetComponent<ViewHelpAction>().enabled = false;
                    break;

                case SubObjectTag.ViewLog:
                    go.AddComponent<ViewLogAction>();
                    go.AddComponent<ViewLogStatus>();
                    go.AddComponent<InputHeader>();

                    go.GetComponent<ViewLogAction>().enabled = false;
                    break;

                case SubObjectTag.Setting:
                    go.AddComponent<SettingAction>();
                    go.AddComponent<SettingStatus>();
                    go.AddComponent<InputHeader>();
                    go.AddComponent<InputSetting>();

                    go.GetComponent<SettingAction>().enabled = false;
                    break;

                case SubObjectTag.Opening:
                    go.AddComponent<OpeningAction>();
                    go.AddComponent<InputOpening>();

                    go.GetComponent<OpeningAction>().enabled = false;
                    break;
            }
            return go;
        }

        private void SetTags(MainObjectTag main, SubObjectTag sub, GameObject go)
        {
            go.AddComponent<MetaInfo>();
            go.GetComponent<MetaInfo>().SetMainTag(main);
            go.GetComponent<MetaInfo>().SetSubTag(sub);
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
            GetComponent<ActorBoard>().RemoveActor(position[0], position[1]);
            GetComponent<DungeonTerrain>().ChangeStatus(true,
                position[0], position[1]);

            pool[go.GetComponent<MetaInfo>().SubTag].Push(go);
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

            pool[go.GetComponent<MetaInfo>().SubTag].Push(go);

            go.SetActive(false);
        }

        private void StoreDoppleganger(GameObject go)
        {
            pool[go.GetComponent<MetaInfo>().SubTag].Push(go);
            go.SetActive(false);
        }
    }
}

namespace Fungus.GameSystem.Data
{
    // Dopplegangers are substitutions for PC under certain situations: examine
    // mode, help menu, etc.
    public enum MainObjectTag { NONE, Building, Actor, Doppleganger };

    public enum SubObjectTag
    {
        NONE, DEFAULT,
        Floor, Wall, Pool, Fungus,
        PC, Examiner, Guide, BuyPower, Setting, ViewHelp, ViewLog, Opening,
        Scavenger, Carnivore, Corpse, Specter,
        BloodFly, Frog, AcidOoze, YellowOoze,
        Dummy
    };
}
