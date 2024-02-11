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

        [Space]
        [SerializeField]
        private ECreatureStates currentState = ECreatureStates.Null;
        private Coroutine coroutine = null;

        private void Awake()
        {
            ChangeState(ECreatureStates.Idle);
        }

        private void ChangeState(ECreatureStates nextState)
        {
            if(coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            currentState = nextState;

            if (currentState == ECreatureStates.Null)
                return;

            if (currentState == ECreatureStates.Idle)
                coroutine = StartCoroutine(idleBehaviour.ExecuteBehaviourRoutine(characterController, transform, FinishedIdle));

            if (currentState == ECreatureStates.Walking)
                coroutine = StartCoroutine(walkingBehaviour.ExecuteBehaviourRoutine(characterController, transform, FinishedWalking));
        }

        private void FinishedIdle()
        {
            ChangeState(ECreatureStates.Walking);
        }

        private void FinishedWalking()
        {
            ChangeState(ECreatureStates.Idle);
        }
    }
}