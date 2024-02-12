namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Behaviours;
    using Project.Enums;
    using UnityEngine.Windows;

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
        private AIBehaviourBase currentBehaviour = null;
        
        private Coroutine coroutine = null;
        private Coroutine behaviourCoroutine = null;

        Vector3 moviment = Vector3.zero;

        private int life = 300;
        private int lifeRemoval = 1;

        private void Awake()
        {
            ChangeState(ECreatureStates.Idle);
        }

        private void FixedUpdate()
        {
            if (currentBehaviour == null)
                return;

            moviment = currentBehaviour.GetMoviment();

            if (moviment == Vector3.zero)
                return;

            moviment = new Vector3(moviment.x * Time.fixedDeltaTime, 0f, moviment.z * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.LookRotation(moviment);

            characterController.Move(moviment);
        }

        private void ChangeState(ECreatureStates nextState)
        {
            currentState = nextState;

            if(behaviourCoroutine != null)
            {
                StopCoroutine(behaviourCoroutine);
                behaviourCoroutine = null;
            }

            if (currentState == ECreatureStates.Null)
                return;

            if(currentBehaviour != null)
                currentBehaviour.StopBehavior();

            if (currentState == ECreatureStates.Idle)
            {
                currentBehaviour = idleBehaviour;
                behaviourCoroutine = StartCoroutine(currentBehaviour.ExecuteBehaviourRoutine(FinishedIdle));
            }

            else if (currentState == ECreatureStates.Walking)
            {
                currentBehaviour = walkingBehaviour;
                behaviourCoroutine = StartCoroutine(currentBehaviour.ExecuteBehaviourRoutine(FinishedWalking));
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("CatchArea"))
            {
                Debug.Log("Started catching");
                coroutine = StartCoroutine(CatchingRoutine());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("CatchArea"))
            {
                Debug.Log("Stoped catching");

                if(coroutine != null)
                {
                    StopCoroutine(CatchingRoutine());
                    coroutine = null;
                }
            }
        }

        IEnumerator CatchingRoutine()
        {
            while(life > 0)
            {
                life -= lifeRemoval;

                if(life < 0)
                    life = 0;

                Debug.Log(life);

                yield return null;
            }

            Debug.Log("Catched");
            Catched();
        }

        void Catched()
        {
            if (coroutine != null)
            {
                StopCoroutine(CatchingRoutine());
                coroutine = null;
            }

            ChangeState(ECreatureStates.Catched);
            gameObject.SetActive(false);
        }
    }
}