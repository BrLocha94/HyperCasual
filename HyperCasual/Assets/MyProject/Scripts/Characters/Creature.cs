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

        private int life = 300;
        private int lifeRemoval = 1;

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

            //if (currentState == ECreatureStates.Idle)
            //    coroutine = StartCoroutine(idleBehaviour.ExecuteBehaviourRoutine(characterController, transform, FinishedIdle));

            //if (currentState == ECreatureStates.Walking)
            //    coroutine = StartCoroutine(walkingBehaviour.ExecuteBehaviourRoutine(characterController, transform, FinishedWalking));
        }

        private void FinishedIdle()
        {
            //ChangeState(ECreatureStates.Walking);
        }

        private void FinishedWalking()
        {
            //ChangeState(ECreatureStates.Idle);
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