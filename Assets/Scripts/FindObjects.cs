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
        private static GameObject examiner;
        private static Dictionary<UITag, GameObject> mainUIDict;
        private static GameObject pcActor;

        public static GameObject Examiner
        {
            get
            {
                if (examiner == null)
                {
                    Debug.Log("Examiner is null.");
                }
                return examiner;
            }

            set
            {
                if (examiner != null)
                {
                    Debug.Log("Examiner already exists.");
                    return;
                }
                examiner = value;
            }
        }

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

        public static GameObject GetUIObject(UITag tag)
        {
            return mainUIDict[tag];
        }

        private void Awake()
        {
            GameLogic = gameObject;
            mainUIDict = new Dictionary<UITag, GameObject>();
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
        }
    }
}
