namespace Project.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Project.Characters;
    using Project.Enums;

    public class CreatureHolder : MonoBehaviour
    {
        [SerializeField]
        private ECreatureType creatureType;
        [SerializeField]
        private PopText targetText;
        [SerializeField]
        private float popTime = 1f;

        private void Awake()
        {
            targetText.SetValue(0);
        }

        private void Start()
        {
            CreatureController.Instance.onCreatureCountUpdated += OnCountUpdated;
        }

        private void OnCountUpdated(ECreatureType type, int count)
        {
            if (creatureType != type)
                return;

            targetText.SetValuePop(count, popTime);
        }
    }
}