namespace Project.Jail
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Enums;
    using Project.Characters;

    public class Jail : MonoBehaviour
    {
        private Creature creature = null;
        private ECreatureType creatureType;

        public ECreatureType CreatureType 
        { 
            get { return creatureType; } 
        }

        public bool isFull => creature != null;

        public void Initialize(ECreatureType creatureType)
        {
            this.creatureType = creatureType;
        }

        public void SetCreature(Creature targetCreature) 
        {
            if (creature != null) return;

            creature = targetCreature;
            creature.transform.parent = this.transform;
            creature.SetOnJail();
        }

        public void ClearJail()
        {
            if(creature == null) return;

            creature.FinishedMerge();
            creature = null;
        }
    }
}