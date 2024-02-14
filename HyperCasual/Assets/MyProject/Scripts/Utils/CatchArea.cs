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
        public Action<int, bool> onCountUpdated;

        [SerializeField] private SpriteRenderer captureCone;

        private List<Creature> targets = new List<Creature>();

        public void CatchedCreature(Creature creature)
        {
            targets.Remove(creature);
            onCountUpdated?.Invoke(targets.Count, AnyTargetFull());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Creature"))
            {
                Creature target = other.GetComponent<Creature>();
                if (target != null)
                {
                    if (target.IsLocked())
                        return;

                    targets.Add(target);

                    if (target.CanCatchCreature())
                    {
                        target.StartCatch();
                        captureCone.enabled = targets.Count > 0;
                    }

                    onCountUpdated?.Invoke(targets.Count, AnyTargetFull());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Creature"))
            {
                Creature target = other.GetComponent<Creature>();
                if (target != null)
                {
                    targets.Remove(target);
                    target.StopCatch();
                    captureCone.enabled = targets.Count > 0;
                    onCountUpdated?.Invoke(targets.Count, AnyTargetFull());
                }
            }
        }

        private bool AnyTargetFull()
        {
            foreach(var target in targets)
            {
                if (target.IsNotFull)
                    continue;

                return true;
            }

            return false;
        }
    }
}