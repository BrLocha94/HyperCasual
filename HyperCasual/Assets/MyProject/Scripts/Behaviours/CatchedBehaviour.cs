namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [CreateAssetMenu(menuName = "AIBehaviours/Catched")]
    public class CatchedBehaviour : AIBehaviourBase
    {
        public override IEnumerator ExecuteBehaviourRoutine(Action onFinishCallback = null)
        {
            throw new NotImplementedException();
        }

        public override void SetTarget(Transform targetTransform)
        {
            throw new NotImplementedException();
        }

        public override void StopBehavior()
        {
            throw new NotImplementedException();
        }
    }
}