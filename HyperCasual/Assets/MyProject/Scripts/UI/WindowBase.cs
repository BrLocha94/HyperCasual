namespace Project.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public abstract class WindowBase : MonoBehaviour
    {
        public Action onTurnOnFinishedEvent;
        public Action onTurnOffFinishedEvent;
        public Action onResetFinishedEvent;

        public abstract void TurnOn();
        public abstract void TurnOff();
        public abstract void ResetWindow();

        protected virtual void OnTurnOnFinished()
        {
            onTurnOnFinishedEvent?.Invoke();
        }

        protected virtual void OnTurnOffFinished()
        {
            onTurnOffFinishedEvent?.Invoke();
        }

        protected virtual void OnResetFinished()
        {
            onResetFinishedEvent?.Invoke();
        }
    }
}