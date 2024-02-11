namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public abstract class AIBehaviourBase : ScriptableObject
    {
        public abstract IEnumerator ExecuteBehaviourRoutine(CharacterController target, Transform targetTransform, Action onBehaviourFinished = null);
    }
}