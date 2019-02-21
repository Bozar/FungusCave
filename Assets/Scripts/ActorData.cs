using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum DataTag
    {
        Stress, Damage, DropPotion,
        InfectionDuration, InfectionAttack, InfectionDefend,
        EnergyRestore, EnergyDrain,
        HP, HPRestore
    }

    public class ActorData : MonoBehaviour
    {
        private Dictionary<DataTag, Dictionary<SubObjectTag, int>> intData;

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

        private void InitializeData()
        {
            // PC
            AddIntData(SubObjectTag.PC, DataTag.Damage, 2);
            AddIntData(SubObjectTag.PC, DataTag.EnergyRestore, 400);

            AddIntData(SubObjectTag.PC, DataTag.HP, 10);
            AddIntData(SubObjectTag.PC, DataTag.HPRestore, 1);

            AddIntData(SubObjectTag.PC, DataTag.Stress, 3);

            // Dummy
            AddIntData(SubObjectTag.Dummy, DataTag.HP, 3);

            // Default
            AddIntData(SubObjectTag.DEFAULT, DataTag.Damage, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.DropPotion, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.EnergyRestore, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.EnergyDrain, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.HPRestore, 0);

            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionAttack, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionDefend, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionDuration,
                GetComponent<InfectionData>().NormalDuration);

            AddIntData(SubObjectTag.DEFAULT, DataTag.Stress, 0);
        }

        private void Start()
        {
            intData = new Dictionary<DataTag, Dictionary<SubObjectTag, int>>();
            InitializeData();

            Version = "0.0.1";
        }
    }
}
