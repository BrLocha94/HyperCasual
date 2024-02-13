namespace Project.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ExpHolder : MonoBehaviour
    {
        [SerializeField]
        private Image fillImage;
        [SerializeField]
        private PopText popText;

        private int levelValue = 50;
        private int currentValue = 0;
        private int finalValue = 0;
        private int levelScaling = 70;
        private float levelUpDelay = 0.4f;

        private int currentLevel = 1;

        Coroutine coroutine = null;

        private void Awake()
        {
            SetCurrentFill();
            popText.SetValue(currentLevel);
        }

        private void SetCurrentFill()
        {
            fillImage.fillAmount = (float)currentValue / (float)levelValue;
        }

        public void AddValue(int value)
        {
            finalValue = currentValue + value;
            CheckValues();
        }

        IEnumerator FillRoutine()
        {
            while(currentValue < finalValue)
            {
                currentValue++;

                if(currentValue >= levelValue)
                {
                    currentValue = levelValue;
                    SetCurrentFill();
                    LevelUpEffect();
                    yield return new WaitForSeconds(levelUpDelay);

                    finalValue -= currentValue;
                    currentValue = 0;
                    levelValue += levelScaling;
                }

                SetCurrentFill();

                yield return new WaitForEndOfFrame();
            }

            coroutine = null;
            CheckValues();
        }

        private void CheckValues()
        {
            if(currentValue < finalValue)
            {
                if(coroutine == null)
                {
                    coroutine = StartCoroutine(FillRoutine());
                }
            }
        }

        private void LevelUpEffect()
        {
            currentLevel++;
            popText.SetValuePop(currentLevel, levelUpDelay);
        }
    }
}