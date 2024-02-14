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
        private Transform rotationPivot;
        [SerializeField]
        private Animator animator;
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
        private Coroutine catchCoroutine = null;

        Vector3 moviment = Vector3.zero;

        private int life = 300;
        private int lifeRemoval = 1;
        private bool canCatch = true;

        public ECreatureType GetCreatureType() => creatureType;
        public bool CanCatchCreature() => (
            currentState == ECreatureStates.Idle ||
            currentState == ECreatureStates.Walking ||
            currentState == ECreatureStates.Running
            );

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

        private void Start()
        {
            canCatch = CreatureController.Instance.CanCatchCreature(creatureType);
            CreatureController.Instance.onCreatureCountUpdated += CreatureCatched;
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
                animator.Play("Idle");
                return;
            }
            else if (currentState != ECreatureStates.Jailed)
            {
                animator.Play("Walk");
                smokeEffect.enableEmission = true;
                moviment = new Vector3(moviment.x * Time.fixedDeltaTime, 0f, moviment.z * Time.fixedDeltaTime);
                rotationPivot.localRotation = Quaternion.LookRotation(moviment);
                characterController.Move(moviment);
            }
            else
            {
                animator.Play("Idle_Variation");
                moviment = new Vector3(moviment.x * Time.fixedDeltaTime, 0f, moviment.z * Time.fixedDeltaTime);
                rotationPivot.localRotation = Quaternion.LookRotation(moviment);
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

        public void StartCatch()
        {
            ChangeState(ECreatureStates.Running);

            if (canCatch)
            {
                if (catchCoroutine != null)
                {
                    StopCoroutine(catchCoroutine);
                    catchCoroutine = null;
                }

                catchCoroutine = StartCoroutine(CatchingRoutine());
                //Change canvas message to catching
                return;
            }
        }

        public void StopCatch() 
        {
            if (catchCoroutine != null)
            {
                StopCoroutine(catchCoroutine);
                catchCoroutine = null;
            }

            //Deactivate all canvas
            Debug.Log("DEACTIVATING CANVAS");
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

            Catched();
        }

        void Catched()
        {
            Debug.Log("Catched");

            if (catchCoroutine != null)
            {
                StopCoroutine(catchCoroutine);
                catchCoroutine = null;
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

        private void CreatureCatched(ECreatureType type, int count) 
        {
            if (type != creatureType)
                return;

            canCatch = CreatureController.Instance.CanCatchCreature(creatureType);

            if (!canCatch)
            {
                if (catchCoroutine != null)
                {
                    StopCoroutine(catchCoroutine);
                    catchCoroutine = null;
                    
                    //CHANGE CANVAS MESSAGE
                    Debug.Log("FULL");
                    return;
                }

                //Deactivate all canvas
                Debug.Log("DEACTIVATING CANVAS");
            }
        }
    }
}