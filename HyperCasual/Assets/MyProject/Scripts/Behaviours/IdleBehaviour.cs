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

        Coroutine coroutine = null;

        public override void StopBehavior()
        {
            if(coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public override void ExecuteBehaviour()
        {
            coroutine = StartCoroutine(ExecuteBehaviourRoutine());
        }

        public IEnumerator ExecuteBehaviourRoutine()
        {
            float random = UnityEngine.Random.Range(minTime, maxTime);

            yield return new WaitForSeconds(random);

            OnBehaviourFinished();
        }
    }
}