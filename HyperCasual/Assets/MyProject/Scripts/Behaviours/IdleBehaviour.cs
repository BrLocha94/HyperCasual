namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using Unity.Mathematics;

    [CreateAssetMenu(menuName = "AIBehaviours/Idle")]
    public class IdleBehaviour : AIBehaviourBase
    {
        [SerializeField]
        private float minTime = 2f;
        [SerializeField]
        private float maxTime = 5f;

        public override IEnumerator ExecuteBehaviourRoutine(CharacterController target, Transform targetTransform, Action onBehaviourFinished = null)
        {
            float random = UnityEngine.Random.Range(minTime, maxTime);

            yield return new WaitForSeconds(random);

            onBehaviourFinished?.Invoke();
        }
    }
}