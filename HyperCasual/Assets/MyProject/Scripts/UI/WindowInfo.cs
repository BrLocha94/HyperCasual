namespace Project.UI
{
    using Project.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Project.Characters;
    using Project.Enums;

    public class WindowInfo : WindowSimple
    {
        [Header("Window info")]
        [SerializeField]
        private Image targetImage;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;

        [SerializeField]
        private List<CreatureInfoHolder> infoHolderList = new List<CreatureInfoHolder>();

        private List<ECreatureType> catchedCreatures = new List<ECreatureType>();
        private int currentIndex = 0;

        public override void TurnOn()
        {
            catchedCreatures = CreatureController.Instance.CatchedCreatures;
            currentIndex = 0;
            SetCurrentInfo();

            base.TurnOn();
        }

        public void Next() 
        {
            currentIndex++;
            if(currentIndex >= infoHolderList.Count)
                currentIndex = 0;

            SetCurrentInfo();
        }

        public void Previous() 
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = infoHolderList.Count - 1;

            SetCurrentInfo();
        }

        private void SetCurrentInfo()
        {
            CreatureInfoHolder info = infoHolderList[currentIndex];

            if (catchedCreatures.Contains(info.GetCreatureType))
            {
                targetImage.sprite = info.GetSprite;
                targetImage.color = Color.white;

                title.text = info.GetCreatureName;
                description.text = info.GetCreatureDescription;
            }
            else
            {
                targetImage.sprite = info.GetSprite;
                targetImage.color = Color.black;

                title.text = "? ? ? ?";
                description.text = string.Empty;
            }
        }
    }
}