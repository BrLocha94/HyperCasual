namespace Project.Jail
{
    using Project.Characters;
    using Project.Enums;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class JailController : MonoBehaviour
    {
        [SerializeField]
        private List<Jail> jailList = new List<Jail>();

        private float mergeInitialDelay = 2f;

        private void Awake()
        {
            //THIS SHOULD BE INITIALIZED BY AN DATA SCRIPT
            jailList[0].Initialize(ECreatureType.Beatle);
            jailList[1].Initialize(ECreatureType.Elephant);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                List<ECreatureType> creatureTypes = new List<ECreatureType>();

                for(int i = 0; i < jailList.Count; i++)
                {
                    if (!jailList[i].isFull)
                        creatureTypes.Add(jailList[i].CreatureType);
                }

                Character player = other.GetComponent<Character>();
                var list = player.GetCatchedCreaturesByType(creatureTypes);

                if(list != null && list.Count > 0)
                    SetOnJail(list);
            }
        }

        private void SetOnJail(List<Creature> list)
        {
            foreach (var jail in jailList) 
            {
                if (jail.isFull) continue;

                jail.SetCreature(list.Find(x => x.GetCreatureType() == jail.CreatureType));
            }


        }

        private void CheckJailsForMerge()
        {
            foreach (var jail in jailList)
            {
                if (jail.isFull) continue;

                return;
            }


        }

        private IEnumerator MergeRoutine()
        {
            yield return new WaitForSeconds(mergeInitialDelay);
        }
    }
}