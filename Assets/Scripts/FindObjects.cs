using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem
{
    // A helper class that stores references to other game objects. The ONLY game
    // object which it CAN be and MUST be attached to is GameLogic.
    public class FindObjects : MonoBehaviour
    {
        private static Dictionary<SubObjectTag, GameObject> actorDict;
        private static Dictionary<UITag, GameObject> mainUIDict;
        private static GameObject pcActor;

        public static GameObject GameLogic { get; private set; }

        public static string IconEnemy { get { return "@"; } }
        public static string IconFog { get { return "%"; } }
        public static string IconPool { get { return "="; } }

        public static GameObject PC
        {
            get
            {
                if (pcActor == null)
                {
                    Debug.Log("PC is null.");
                }
                return pcActor;
            }

            set
            {
                if (pcActor != null)
                {
                    Debug.Log("PC already exists.");
                    return;
                }
                pcActor = value;
            }
        }

        public static string Version { get; private set; }

        public static GameObject GetStaticActor(SubObjectTag tag)
        {
            GameObject checkActor;

            if (actorDict.TryGetValue(tag, out checkActor)
                && (checkActor != null))
            {
                return checkActor;
            }
            Debug.Log(tag.ToString() + " is null.");
            return null;
        }

        public static GameObject GetUIObject(UITag tag)
        {
            return mainUIDict[tag];
        }

        public static Text GetUIText(UITag tag)
        {
            return mainUIDict[tag].GetComponent<Text>();
        }

        public static void SetStaticActor(SubObjectTag tag, GameObject actor)
        {
            GameObject checkActor;

            if (actorDict.TryGetValue(tag, out checkActor)
                && (checkActor != null))
            {
                Debug.Log(tag.ToString() + " already exists.");
            }
            else
            {
                actorDict[tag] = actor;
            }
        }

        private void Awake()
        {
            GameLogic = gameObject;
            mainUIDict = new Dictionary<UITag, GameObject>();
            actorDict = new Dictionary<SubObjectTag, GameObject>();
        }

        private void InitializeUIDict()
        {
            // NOTE: If a GameObject's tag is MainUI AND its name is stored in
            // UITags, it can be found in MainUIDict.
            UITag tempDictKey;
            GameObject[] tempGOArray = GameObject.FindGameObjectsWithTag("UI");

            foreach (var go in tempGOArray)
            {
                if (Enum.IsDefined(typeof(UITag), go.name))
                {
                    tempDictKey = (UITag)Enum.Parse(typeof(UITag), go.name);

                    if (!mainUIDict.ContainsKey(tempDictKey))
                    {
                        mainUIDict.Add(tempDictKey, go);
                        go.GetComponent<Text>().text = "";
                    }
                }
            }
        }

        private void Start()
        {
            InitializeUIDict();
            Version = "0.1.0";
        }
    }
}
