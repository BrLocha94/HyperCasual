namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Utils;
    using Project.Enums;
    using System;

    public class CreatureController : MonoSingleton<CreatureController>
    {
        public Action<ECreatureType, int> onCreatureCountUpdated = null;

        private List<ECreatureType> catchedCreatures = new List<ECreatureType>();
        private Dictionary<ECreatureType, int> currentCatchedCreatures = new Dictionary<ECreatureType, int>();

        protected override void ExecuteOnAwake()
        {
            base.ExecuteOnAwake();

            currentCatchedCreatures.Add(ECreatureType.Beatle, 0);
            currentCatchedCreatures.Add(ECreatureType.Elephant, 0);
        }

        public List<ECreatureType> CatchedCreatures => catchedCreatures;

        public bool CanCatchCreature(ECreatureType creatureType)
        {
            return currentCatchedCreatures[creatureType] == 0;
        }

        public void CatchCreature(ECreatureType creatureType)
        {
            currentCatchedCreatures[creatureType]++;
            onCreatureCountUpdated?.Invoke(creatureType, currentCatchedCreatures[creatureType]);

            if(!catchedCreatures.Contains(creatureType)) 
            { 
                catchedCreatures.Add(creatureType);
            }
        }

        public void MergedCreature(ECreatureType creatureType)
        {
            currentCatchedCreatures[creatureType]--;

            //Safeguard
            if (currentCatchedCreatures[creatureType] < 0)
                currentCatchedCreatures[creatureType] = 0;

            onCreatureCountUpdated?.Invoke(creatureType, currentCatchedCreatures[creatureType]);
        }
    }
}