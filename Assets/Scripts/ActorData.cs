using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum DataTag
    {
        ActorName,

        Stress, Damage, Potion,
        InfectionAttack, InfectionDefend, InfectionRecovery,
        EnergyRestore, EnergyDrain,
        HP, HPRestore,
        SightRange
    }

    public class ActorData : MonoBehaviour
    {
        private Dictionary<DataTag, Dictionary<SubObjectTag, int>> intData;
        private Dictionary<DataTag, Dictionary<SubObjectTag, string>> stringData;

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

        private void InitializeData()
        {
            // PC
            SetIntData(SubObjectTag.PC, DataTag.HP, 10);
            SetIntData(SubObjectTag.PC, DataTag.HPRestore, 1);
            SetIntData(SubObjectTag.PC, DataTag.Damage, 2);

            SetIntData(SubObjectTag.PC, DataTag.InfectionAttack,
                GetComponent<InfectionData>().RateHigh);
            SetIntData(SubObjectTag.PC, DataTag.InfectionDefend,
                GetComponent<InfectionData>().RateNormal);

            SetIntData(SubObjectTag.PC, DataTag.EnergyRestore,
                GetComponent<EnergyData>().ModNormal);

            SetIntData(SubObjectTag.PC, DataTag.Stress, 3);
            SetIntData(SubObjectTag.PC, DataTag.SightRange, 5);

            // Scavenger Beetle
            SetStringData(SubObjectTag.Beetle, DataTag.ActorName,
                "Scavenger Beetle");

            SetIntData(SubObjectTag.Beetle, DataTag.Damage, 3);
            SetIntData(SubObjectTag.Beetle, DataTag.Potion, 0);
            SetIntData(SubObjectTag.Beetle, DataTag.EnergyRestore,
                GetComponent<EnergyData>().ModHigh);

            // Swollen Corpse
            SetStringData(SubObjectTag.Corpse, DataTag.ActorName,
                "Swollen Corpse");

            SetIntData(SubObjectTag.Corpse, DataTag.HP, 9);

            // Yellow Ooze
            SetStringData(SubObjectTag.YellowOoze, DataTag.ActorName,
                "Yellow Ooze");

            SetIntData(SubObjectTag.YellowOoze, DataTag.HP, 4);
            SetIntData(SubObjectTag.YellowOoze, DataTag.Damage, 3);

            // Blood Fly
            SetStringData(SubObjectTag.BloodFly, DataTag.ActorName,
                "Blood Fly");

            SetIntData(SubObjectTag.BloodFly, DataTag.HP, 3);
            SetIntData(SubObjectTag.BloodFly, DataTag.Damage, 2);
            SetIntData(SubObjectTag.BloodFly, DataTag.EnergyRestore,
                GetComponent<EnergyData>().ModNormal);

            // Dummy
            SetStringData(SubObjectTag.Dummy, DataTag.ActorName, "Dummy");
            SetIntData(SubObjectTag.Dummy, DataTag.HP, 3);

            // Default: int
            SetIntData(SubObjectTag.DEFAULT, DataTag.HP, 1);
            SetIntData(SubObjectTag.DEFAULT, DataTag.Damage, 1);
            SetIntData(SubObjectTag.DEFAULT, DataTag.Potion, 1);

            SetIntData(SubObjectTag.DEFAULT, DataTag.EnergyRestore, 0);
            SetIntData(SubObjectTag.DEFAULT, DataTag.EnergyDrain, 0);

            SetIntData(SubObjectTag.DEFAULT, DataTag.InfectionAttack, 0);
            SetIntData(SubObjectTag.DEFAULT, DataTag.InfectionDefend, 0);
            SetIntData(SubObjectTag.DEFAULT, DataTag.InfectionRecovery,
                GetComponent<InfectionData>().RecoveryNormal);

            // These data should remain unchanged for all NPCs.
            SetIntData(SubObjectTag.DEFAULT, DataTag.HPRestore, 0);
            SetIntData(SubObjectTag.DEFAULT, DataTag.Stress, 0);
            SetIntData(SubObjectTag.DEFAULT, DataTag.SightRange, 8);

            // Default: string
            SetStringData(SubObjectTag.DEFAULT, DataTag.ActorName, "INVALID");
        }

        private void SetIntData(SubObjectTag oTag, DataTag dTag, int data)
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

        private void SetStringData(SubObjectTag oTag, DataTag dTag, string data)
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

        private void Start()
        {
            intData = new Dictionary<DataTag, Dictionary<SubObjectTag, int>>();
            stringData = new Dictionary<DataTag,
                Dictionary<SubObjectTag, string>>();
            InitializeData();
        }
    }
}
