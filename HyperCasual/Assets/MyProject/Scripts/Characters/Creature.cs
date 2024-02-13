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
        private ECreatureType creatureType;
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private ParticleSystem smokeEffect;

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
        private Coroutine coroutine = null;

        Vector3 moviment = Vector3.zero;

        private int life = 300;
        private int lifeRemoval = 1;

        public ECreatureType GetCreatureType() => creatureType;

        private void Awake()
        {
            idleBehaviour.onFinishEvent += FinishedIdle;
            walkingBehaviour.onFinishEvent += FinishedWalking;
            runningBehaviour.onFinishEvent += FinishedRunning;
            catchBehaviour.onFinishEvent += FinishedCatching;
            jailedBehaviour.onFinishEvent += FinishedJail;

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

            ChangeState(ECreatureStates.Idle);
        }

        private void FixedUpdate()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (currentBehaviour == null)
                return;

            moviment = currentBehaviour.GetMoviment();

            if (moviment == Vector3.zero)
            {
                smokeEffect.enableEmission = false;
                return;
            }
            else if (currentState != ECreatureStates.Jailed)
            {
                smokeEffect.enableEmission = true;
                moviment = new Vector3(moviment.x * Time.fixedDeltaTime, 0f, moviment.z * Time.fixedDeltaTime);
                transform.localRotation = Quaternion.LookRotation(moviment);
                characterController.Move(moviment);
            }
            else
            {
                moviment = new Vector3(moviment.x * Time.fixedDeltaTime, 0f, moviment.z * Time.fixedDeltaTime);
                transform.localRotation = Quaternion.LookRotation(moviment);
            }
        }
        private void ChangeState(ECreatureStates nextState)
        {
            currentState = nextState;

            if (currentState == ECreatureStates.Null)
                return;

            if(currentBehaviour != null)
                currentBehaviour.StopBehavior();

            if (currentState == ECreatureStates.Idle)
            {
                currentBehaviour = idleBehaviour;
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            else if (currentState == ECreatureStates.Walking)
            {
                currentBehaviour = walkingBehaviour;
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            else if (currentState == ECreatureStates.Running)
            {
                currentBehaviour = runningBehaviour;
                currentBehaviour.SetSource(transform);
                currentBehaviour.SetTarget(player.transform);
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            else if (currentState == ECreatureStates.Catched)
            {
                currentBehaviour = catchBehaviour;
                currentBehaviour.SetSource(transform);
                currentBehaviour.ExecuteBehaviour();
                return;
            }

            else if (currentState == ECreatureStates.Jailed)
            {
                currentBehaviour = jailedBehaviour;
                currentBehaviour.SetSource(transform);
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
            gameObject.SetActive(false);
        }

        private void FinishedJail()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (currentState == ECreatureStates.Catched ||
                currentState == ECreatureStates.Jailed ||
                currentState == ECreatureStates.Merging)
                return;

            if (other.tag.Equals("CatchArea"))
            {
                ChangeState(ECreatureStates.Running);

                if (coroutine != null)
                {
                    StopCoroutine(CatchingRoutine());
                    coroutine = null;
                }

                coroutine = StartCoroutine(CatchingRoutine());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentState == ECreatureStates.Catched ||
                currentState == ECreatureStates.Jailed ||
                currentState == ECreatureStates.Merging)
                return;

            if (other.tag.Equals("CatchArea"))
            {
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

            player.CatchCreature(this);
        }

        public void FinishCatching()
        {
            ChangeState(ECreatureStates.Catched);
        }

        public void SetOnJail()
        {
            gameObject.SetActive(true);
            ChangeState(ECreatureStates.Jailed);
        }
    }
}