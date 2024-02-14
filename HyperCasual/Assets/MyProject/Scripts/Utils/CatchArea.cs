namespace Project.Utils
{
    using Project.Characters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class CatchArea : MonoBehaviour
    {
        public Action<int> onCountUpdated;

        [SerializeField] private SpriteRenderer captureCone;

        private List<Creature> targets = new List<Creature>();

        public void CatchedCreature(Creature creature)
        {
            targets.Remove(creature);
            onCountUpdated?.Invoke(targets.Count);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Creature"))
            {
                Creature target = other.GetComponent<Creature>();
                if (target.CanCatchCreature())
                {
                    targets.Add(target);
                    target.StartCatch();
                    captureCone.enabled = true;
                    onCountUpdated?.Invoke(targets.Count);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Creature"))
            {
                Creature target = other.GetComponent<Creature>();
                targets.Remove(target);
                target?.StopCatch();
                captureCone.enabled = false;
                onCountUpdated?.Invoke(targets.Count);
            }
        }
    }
}