namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.UIElements;
    using UnityEngine.Windows;

    [CreateAssetMenu(menuName = "AIBehaviours/Walking")]
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


        public override IEnumerator ExecuteBehaviourRoutine(CharacterController targetCharacter, Transform targetTransform, Action onBehaviourFinished = null)
        {
            float randomTime = UnityEngine.Random.Range(minTime, maxTime);
            float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);

            float randomDirectionX = UnityEngine.Random.Range(-1, 1);
            float randomDirectionZ = UnityEngine.Random.Range(-1, 1);

            Vector3 moviment;

            float time = 0;
            while (time < randomTime)
            {
                moviment = new Vector3(
                    randomDirectionX * randomSpeed * Time.fixedDeltaTime,
                    0f,
                    randomDirectionZ * randomSpeed * Time.fixedDeltaTime
                    );

                targetTransform.localRotation = Quaternion.LookRotation(moviment);

                targetCharacter.Move(moviment);

                time += Time.fixedDeltaTime;
                yield return null;
            }

            onBehaviourFinished?.Invoke();
        }
    }
}