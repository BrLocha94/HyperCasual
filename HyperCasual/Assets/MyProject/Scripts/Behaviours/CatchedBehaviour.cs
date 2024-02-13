namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class CatchedBehaviour : AIBehaviourBase
    {
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private float animationUpTime;
        [SerializeField]
        private float animationDownTime;

        Coroutine coroutine = null;
        Transform source = null;

        public override void SetSource(Transform sourceTransform)
        {
            source = sourceTransform;
        }

        public override void ExecuteBehaviour()
        {
            coroutine = StartCoroutine(ExecuteBehaviourRoutine());
        }

        public override void StopBehavior()
        {
            if(coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            source = null;
        }

        public IEnumerator ExecuteBehaviourRoutine()
        {
            float time = 0;

            Vector3 initialPoint = source.localPosition;
            Vector3 initialScale = source.localScale;

            Vector3 midPoint = new Vector3(
                source.localPosition.x / 2,
                source.localPosition.y + 3,
                source.localPosition.z / 2);

            Vector3 midScale = new Vector3(
                source.localScale.x * 3 / 4,
                source.localScale.y * 3 / 4,
                source.localScale.z * 3 / 4);

            while (time < animationUpTime)
            {
                source.localPosition = Vector3.Lerp(initialPoint, midPoint, curve.Evaluate(time / animationUpTime));
                source.localScale = Vector3.Lerp(initialScale, midScale, curve.Evaluate(time / animationDownTime));
                time += Time.deltaTime;
                yield return null;
            }

            time = 0f;
            initialPoint = source.localPosition;
            initialScale = source.localScale;

            while (time < animationDownTime)
            {
                source.localPosition = Vector3.Lerp(initialPoint, Vector3.zero, curve.Evaluate(time / animationDownTime));
                source.localScale = Vector3.Lerp(initialScale, Vector3.zero, curve.Evaluate(time / animationDownTime));
                time += Time.deltaTime;
                yield return null;
            }

            OnBehaviourFinished();
        }
    }
}