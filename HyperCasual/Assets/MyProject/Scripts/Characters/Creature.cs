namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Behaviours;

    public class Creature : MonoBehaviour
    {
        [SerializeField]
        private AIBehaviourBase idleBehaviour;
        [SerializeField]
        private AIBehaviourBase walkingBehaviour;
        [SerializeField]
        private AIBehaviourBase runningBehaviour;
        [SerializeField]
        private AIBehaviourBase catchedBehaviour;
    }
}