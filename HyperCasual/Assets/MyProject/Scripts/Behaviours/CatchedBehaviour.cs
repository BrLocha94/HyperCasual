namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [CreateAssetMenu(menuName = "AIBehaviours/Catched")]
    public class CatchedBehaviour : AIBehaviourBase
    {
        public override IEnumerator ExecuteBehaviourRoutine(CharacterController target, Transform targetTransform, Action onBehaviourFinished = null)
        {
            throw new System.NotImplementedException();
        }
    }
}