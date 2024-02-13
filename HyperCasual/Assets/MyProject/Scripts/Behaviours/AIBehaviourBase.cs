namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public abstract class AIBehaviourBase : MonoBehaviour
    {
        public Action onFinishEvent = null;

        public abstract void ExecuteBehaviour();
        public abstract void StopBehavior();

        public virtual Vector3 GetMoviment()
        {
            return Vector3.zero;
        }

        protected virtual void OnBehaviourFinished()
        {
            onFinishEvent?.Invoke();
        }

        public virtual void SetSource(Transform sourceTransform) { }
        public virtual void SetTarget(Transform targetTransform) { }
    }
}