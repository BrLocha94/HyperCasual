namespace Project.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Enums;

    [CreateAssetMenu(menuName = "Assets/CreatureInfo")]
    public class CreatureInfoHolder : ScriptableObject
    {
        [SerializeField]
        private ECreatureType creatureType;
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private string creatureName;
        [SerializeField]
        private string creatureDescription;

        public ECreatureType GetCreatureType => creatureType;
        public Sprite GetSprite => sprite;
        public string GetCreatureName => creatureName;
        public string GetCreatureDescription => creatureDescription;
    }
}