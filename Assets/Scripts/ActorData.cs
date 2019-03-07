using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum DataTag
    {
        ActorName,
        Stress, Damage, Potion,
        InfectionDuration, InfectionAttack, InfectionDefend,
        EnergyRestore, EnergyDrain,
        HP, HPRestore
    }

    public class ActorData : MonoBehaviour
    {
        private Dictionary<DataTag, Dictionary<SubObjectTag, int>> intData;
        private Dictionary<DataTag, Dictionary<SubObjectTag, string>> stringData;

        public string Version { get; private set; }

        public int GetIntData(SubObjectTag oTag, DataTag dTag)
        {
            Dictionary<SubObjectTag, int> dataDict;
            int gameData;

            if (intData.TryGetValue(dTag, out dataDict))
            {
                if (dataDict.TryGetValue(oTag, out gameData))
                {
                    return gameData;
                }
                else if (dataDict.TryGetValue(SubObjectTag.DEFAULT, out gameData))
                {
                    return gameData;
                }
            }
            throw new MemberAccessException();
        }

        public string GetStringData(SubObjectTag oTag, DataTag dTag)
        {
            Dictionary<SubObjectTag, string> dataDict;
            string gameData;

            if (stringData.TryGetValue(dTag, out dataDict))
            {
                if (dataDict.TryGetValue(oTag, out gameData))
                {
                    return gameData;
                }
                else if (dataDict.TryGetValue(SubObjectTag.DEFAULT, out gameData))
                {
                    return gameData;
                }
            }
            throw new MemberAccessException();
        }

        private void AddIntData(SubObjectTag oTag, DataTag dTag, int data)
        {
            if (!intData.ContainsKey(dTag))
            {
                intData.Add(dTag, new Dictionary<SubObjectTag, int>());
            }

            if (!intData[dTag].ContainsKey(oTag))
            {
                intData[dTag].Add(oTag, data);
            }
        }

        private void AddStringData(SubObjectTag oTag, DataTag dTag, string data)
        {
            if (!stringData.ContainsKey(dTag))
            {
                stringData.Add(dTag, new Dictionary<SubObjectTag, string>());
            }

            if (!stringData[dTag].ContainsKey(oTag))
            {
                stringData[dTag].Add(oTag, data);
            }
        }

        private void InitializeData()
        {
            // PC
            AddIntData(SubObjectTag.PC, DataTag.HP, 10);
            AddIntData(SubObjectTag.PC, DataTag.Damage, 2);
            AddIntData(SubObjectTag.PC, DataTag.EnergyRestore,
                GetComponent<EnergyData>().BonusRestoreNormal);

            AddIntData(SubObjectTag.PC, DataTag.HPRestore, 1);
            AddIntData(SubObjectTag.PC, DataTag.Stress, 3);

            // Grey Ooze
            AddStringData(SubObjectTag.GreyOoze, DataTag.ActorName, "Grey Ooze");

            AddIntData(SubObjectTag.GreyOoze, DataTag.Potion, 0);
            AddIntData(SubObjectTag.GreyOoze, DataTag.EnergyRestore,
                GetComponent<EnergyData>().BonusRestoreHigh);

            // Swollen Corpse
            AddStringData(SubObjectTag.Corpse, DataTag.ActorName,
                "Swollen Corpse");

            AddIntData(SubObjectTag.Corpse, DataTag.HP, 9);
            AddIntData(SubObjectTag.Corpse, DataTag.Potion, 2);

            // Blood Fly
            AddStringData(SubObjectTag.BloodFly, DataTag.ActorName,
                "Blood Fly");

            AddIntData(SubObjectTag.BloodFly, DataTag.HP, 4);
            AddIntData(SubObjectTag.BloodFly, DataTag.Damage, 3);

            // Yellow Ooze
            AddStringData(SubObjectTag.YellowOoze, DataTag.ActorName,
                "Yellow Ooze");

            AddIntData(SubObjectTag.YellowOoze, DataTag.HP, 3);
            AddIntData(SubObjectTag.YellowOoze, DataTag.Damage, 2);
            AddIntData(SubObjectTag.YellowOoze, DataTag.EnergyRestore,
                GetComponent<EnergyData>().BonusRestoreNormal);

            // Dummy
            AddStringData(SubObjectTag.Dummy, DataTag.ActorName, "Dummy");
            AddIntData(SubObjectTag.Dummy, DataTag.HP, 3);

            // Default: int
            AddIntData(SubObjectTag.DEFAULT, DataTag.HP, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.Damage, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.Potion, 1);

            AddIntData(SubObjectTag.DEFAULT, DataTag.EnergyRestore, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.EnergyDrain, 0);

            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionAttack, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionDefend, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionDuration,
                GetComponent<InfectionData>().NormalDuration);

            // These data should remain unchanged for all NPCs.
            AddIntData(SubObjectTag.DEFAULT, DataTag.HPRestore, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.Stress, 0);

            // Default: string
            AddStringData(SubObjectTag.DEFAULT, DataTag.ActorName, "INVALID");
        }

        private void Start()
        {
            intData = new Dictionary<DataTag, Dictionary<SubObjectTag, int>>();
            stringData = new Dictionary<DataTag,
                Dictionary<SubObjectTag, string>>();
            InitializeData();

            Version = "0.0.2";
        }
    }
}
