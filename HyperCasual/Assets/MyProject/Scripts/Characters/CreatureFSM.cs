namespace Project.Characters
{
    using Project.Behaviours;
    using Project.Enums;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class CreatureFSM : MonoBehaviour
    {
        public Action<ECreatureStates> onStateChange;

        [Header("Behaviours")]
        [SerializeField]
        private AIBehaviourBase idleBehaviour;
        [SerializeField]
        private AIBehaviourBase walkingBehaviour;
        [SerializeField]
        private AIBehaviourBase runningBehaviour;
        [SerializeField]
        private AIBehaviourBase catchBehaviour;
        [SerializeField]
        private AIBehaviourBase jailedBehaviour;

        private ECreatureStates currentState = ECreatureStates.Null;
        private AIBehaviourBase currentBehaviour = null;

        private Character player = null;
        private Transform targetTransfom = null;

        public void Initialize(Character player, Transform targetTransform)
        {
            this.player = player;
            this.targetTransfom = targetTransform;

            ChangeState(ECreatureStates.Idle);
        }

        public Vector3 GetMoviment()
        {
            if (currentBehaviour == null)
                return Vector3.zero;

            return currentBehaviour.GetMoviment();
        }

        private void Awake()
        {
            idleBehaviour.onFinishEvent += FinishedIdle;
            walkingBehaviour.onFinishEvent += FinishedWalking;
            runningBehaviour.onFinishEvent += FinishedRunning;
            catchBehaviour.onFinishEvent += FinishedCatching;
            jailedBehaviour.onFinishEvent += FinishedJail;
        }

        public void ChangeState(ECreatureStates nextState)
        {
            currentState = nextState;
            onStateChange?.Invoke(currentState);

            if (currentState == ECreatureStates.Null)
                return;

            if (currentBehaviour != null)
                currentBehaviour.StopBehavior();

            if (currentState == ECreatureStates.Idle)
            {
                currentBehaviour = idleBehaviour;
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            if (currentState == ECreatureStates.Walking)
            {
                currentBehaviour = walkingBehaviour;
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            if (currentState == ECreatureStates.Running)
            {
                currentBehaviour = runningBehaviour;
                currentBehaviour.SetSource(targetTransfom);
                currentBehaviour.SetTarget(player.transform);
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            if (currentState == ECreatureStates.Catched)
            {
                currentBehaviour = catchBehaviour;
                currentBehaviour.SetSource(targetTransfom);
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            if (currentState == ECreatureStates.Jailed)
            {
                currentBehaviour = jailedBehaviour;
                currentBehaviour.SetSource(targetTransfom);
                currentBehaviour.ExecuteBehaviour();
                return;
            }
        }

        private void FinishedIdle()
        {
            ChangeState(ECreatureStates.Walking);
        }

        private void FinishedWalking()
        {
            ChangeState(ECreatureStates.Idle);
        }

        private void FinishedRunning()
        {
            ChangeState(ECreatureStates.Idle);
        }

        private void FinishedCatching()
        {
            
        }

        private void FinishedJail()
        {
            
        }
    }
}