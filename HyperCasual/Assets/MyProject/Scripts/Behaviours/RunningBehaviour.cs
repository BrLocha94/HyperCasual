namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [CreateAssetMenu(menuName = "AIBehaviours/Running")]
    public class RunningBehaviour : AIBehaviourBase
    {
        public override IEnumerator ExecuteBehaviourRoutine(CharacterController target, Transform targetTransform, Action onBehaviourFinished = null)
        {
            throw new System.NotImplementedException();
        }
    }
}