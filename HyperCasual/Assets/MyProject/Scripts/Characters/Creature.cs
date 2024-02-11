namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Behaviours;
    using Project.Enums;

    public class Creature : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;

        [Header("Behaviours")]
        [SerializeField]
        private AIBehaviourBase idleBehaviour;
        [SerializeField]
        private AIBehaviourBase walkingBehaviour;
        [SerializeField]
        private AIBehaviourBase runningBehaviour;
        [SerializeField]
        private AIBehaviourBase catchedBehaviour;

        private ECreatureStates currentStates = ECreatureStates.Null;
        private Coroutine coroutine = null;

        private void ChangeState(ECreatureStates nextState)
        {
            if(coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}