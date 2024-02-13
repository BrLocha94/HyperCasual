namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using static UnityEditor.PlayerSettings;
    using UnityEditor.Rendering;
    using static UnityEngine.GraphicsBuffer;

    [CreateAssetMenu(menuName = "AIBehaviours/Running")]
    public class RunningBehaviour : AIBehaviourBase
    {
        [SerializeField]
        private float speed = 7f;

        [SerializeField]
        private float minDistanceToStop = 10f;

        private Transform currentTarget = null;
        private Transform currentSource = null;

        Coroutine coroutine = null;
        Vector3 moviment = Vector3.zero;

        public override void SetSource(Transform sourceTransform)
        {
            currentSource = sourceTransform;
        }

        public override void SetTarget(Transform targetTransform)
        {
            currentTarget = targetTransform;
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
            var heading = currentSource.position - currentTarget.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            while (distance < 12f) 
            {
                heading = currentSource.position - currentTarget.position;
                distance = heading.magnitude;
                direction = heading / distance;
                direction = direction.normalized;


                moviment = new Vector3(
                    direction.x * speed,
                    0f,
                    direction.z * speed
                    );

                moviment = direction * speed;

                yield return null;
            }

            OnBehaviourFinished();
        }

        public override Vector3 GetMoviment()
        {
            return moviment;
        }
    }
}