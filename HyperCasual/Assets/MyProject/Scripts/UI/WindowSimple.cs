namespace Project.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class WindowSimple : WindowBase
    {
        [Header("Window simple")]
        [SerializeField]
        protected AnimationCurve turnOnCurve;
        [SerializeField]
        protected float turnOnDuration;

        [SerializeField]
        protected AnimationCurve turnOffCurve;
        [SerializeField]
        protected float turnOffDuration;

        protected Coroutine currentRoutine = null;

        private CanvasGroup _canvasGroup;
        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();

                return _canvasGroup;
            }
        }

        public override void TurnOn()
        {
            if (currentRoutine != null) return;

            //Remove this to a state machine logic
            Time.timeScale = 0f;

            CanvasGroup.alpha = 0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = true;

            gameObject.SetActive(true);

            currentRoutine = StartCoroutine(TurnOnRoutine());
        }

        protected virtual IEnumerator TurnOnRoutine()
        {
            float time = 0;

            while (time < turnOnDuration)
            {
                CanvasGroup.alpha = Mathf.Lerp(0f, 1f, turnOnCurve.Evaluate(time / turnOnDuration));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            CanvasGroup.alpha = 1f;
            CanvasGroup.interactable = true;

            OnTurnOnFinished();

            currentRoutine = null;
        }

        public override void TurnOff()
        {
            if (currentRoutine != null) return;

            CanvasGroup.alpha = 1f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = true;

            currentRoutine = StartCoroutine(TurnOffRoutine());
        }

        protected virtual IEnumerator TurnOffRoutine()
        {
            float time = 0;

            while (time < turnOffDuration)
            {
                CanvasGroup.alpha = Mathf.Lerp(1f, 0f, turnOffCurve.Evaluate(time / turnOffDuration));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            CanvasGroup.alpha = 0f;
            CanvasGroup.blocksRaycasts = false;

            OnTurnOffFinished();

            //Remove this to a state machine logic
            Time.timeScale = 1f;

            currentRoutine = null;
            gameObject.SetActive(false);
        }


        public override void ResetWindow()
        {
            currentRoutine = StartCoroutine(ResetRoutine());
        }

        protected virtual IEnumerator ResetRoutine()
        {
            yield return null;
            OnResetFinished();
            currentRoutine = null;
        }
    }
}