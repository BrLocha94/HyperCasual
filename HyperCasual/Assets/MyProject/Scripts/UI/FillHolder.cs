namespace Project.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class FillHolder : MonoBehaviour
    {
        [SerializeField]
        private Image fillImage;

        public void SetCurrentFill(int current, int total)
        {
            fillImage.fillAmount = (float)current / (float)total;
        }
    }
}