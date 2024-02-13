namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.UIElements;
    using UnityEngine.Windows;

    public class WalkingBehaviour : AIBehaviourBase
    {
        [SerializeField]
        private float minTime = 2f;
        [SerializeField]
        private float maxTime = 5f;
        [SerializeField]
        private float minSpeed = 1f;
        [SerializeField]
        private float maxSpeed = 3f;

        [SerializeField]
        private float movimentRange = 0.3f;

        Vector3 moviment = Vector3.zero;

        Coroutine coroutine = null;

        public override Vector3 GetMoviment()
        {
            return moviment;
        }

        public override void StopBehavior()
        {
            if (coroutine != null)
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
            float randomTime = UnityEngine.Random.Range(minTime, maxTime);
            float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);

            float randomDirectionX = UnityEngine.Random.Range(-movimentRange, movimentRange);
            float randomDirectionZ = UnityEngine.Random.Range(-movimentRange, movimentRange);

            moviment = new Vector3(
                    randomDirectionX * randomSpeed,
                    0f,
                    randomDirectionZ * randomSpeed
                    );

            yield return new WaitForSeconds(randomTime);

            OnBehaviourFinished();
        }
    }
}