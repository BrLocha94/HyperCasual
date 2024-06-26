namespace Project.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using static Unity.VisualScripting.Member;

    public class PopText : MonoBehaviour
    {
        [SerializeField]
        private RectTransform targetRect;
        [SerializeField]
        private TextMeshProUGUI targetText;

        [SerializeField]
        private bool changeColorOnPop = false;
        [SerializeField]
        private Color popColor = Color.yellow;

        [SerializeField]
        private AnimationCurve curveIn;
        [SerializeField]
        private AnimationCurve curveOut;
        [SerializeField]
        private float scaleFactor = 0.25f;
        [SerializeField]
        private string prefix = string.Empty;

        Color defaultColor = new Color();
        Coroutine coroutine = null;

        private void Awake()
        {
            defaultColor.r = targetText.color.r;
            defaultColor.g = targetText.color.g;
            defaultColor.b = targetText.color.b;
            defaultColor.a = targetText.color.a;
        }

        public void SetValue(int value)
        {
            targetText.text = prefix + value.ToString();
        }

        public void SetValuePop(int value, float time, float delay = 0f)
        {
            if (coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            targetText.color = defaultColor;
            targetRect.localScale = Vector3.one;
            coroutine = StartCoroutine(PopRoutine(time, delay, () => targetText.text = prefix + value.ToString()));
        }

        private IEnumerator PopRoutine(float time, float delay, Action onDelayFinished)
        {
            yield return new WaitForSeconds(delay);

            onDelayFinished?.Invoke();

            float currentTime = 0;
            float partialTime = time / 2;

            Vector3 initialScale = targetRect.localScale;
            Vector3 finalScale = new Vector3(
                initialScale.x + scaleFactor,
                initialScale.y + scaleFactor,
                1f
                );

            if (changeColorOnPop)
                targetText.color = popColor;

            while (currentTime < partialTime)
            {
                targetRect.localScale = Vector3.Lerp(initialScale, finalScale, curveIn.Evaluate(currentTime / partialTime));
                currentTime += Time.deltaTime;
                yield return null;
            }

            currentTime = 0;

            initialScale = targetRect.localScale;
            finalScale = Vector3.one;

            while (currentTime < partialTime)
            {
                targetRect.localScale = Vector3.Lerp(initialScale, finalScale, curveIn.Evaluate(currentTime / partialTime)); 
                currentTime += Time.deltaTime;
                yield return null;
            }

            targetText.color = defaultColor;
            coroutine = null;
        }
    }
}