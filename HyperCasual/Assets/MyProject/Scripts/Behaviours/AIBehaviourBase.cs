namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public abstract class AIBehaviourBase : ScriptableObject
    {
        public virtual Vector3 GetMoviment()
        {
            return Vector3.zero;
        }
        public abstract void SetTarget(Transform targetTransform);
        public abstract IEnumerator ExecuteBehaviourRoutine(Action onFinishCallback = null);
        public abstract void StopBehavior();
    }
}