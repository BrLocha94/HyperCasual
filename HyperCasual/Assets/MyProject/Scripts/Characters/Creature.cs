namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Behaviours;
    using Project.Enums;
    using UnityEngine.Windows;
    using Project.UI;
    using System.Diagnostics;

    public class Creature : CharacterBase
    {
        [Header("Creature")]
        [Header("External references")]
        [SerializeField]
        private CreatureFSM creatureFSM;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private FillHolder fillHolder;

        [Header("Creature configurations")]
        [SerializeField]
        private ECreatureType creatureType;
        [SerializeField] 
        private int defaultLife = 300;
        [SerializeField]
        private int lifeRemoval = 1;
        [SerializeField]
        private float gravityScale = 1f;

        private ECreatureStates currentState = ECreatureStates.Null;

        private Character player = null;
        private Coroutine catchCoroutine = null;

        private Vector3 moviment = Vector3.zero;
        private Vector3 gravity = Vector3.zero;

        private int currentLife = 0;
        private bool canCatch = true;

        public ECreatureType GetCreatureType() => creatureType;
        public bool CanCatchCreature() => (
            currentState == ECreatureStates.Idle ||
            currentState == ECreatureStates.Walking ||
            currentState == ECreatureStates.Running
            );

        private void OnStateChange(ECreatureStates state)
        {
            currentState = state;
        }

        private void Awake()
        {
            creatureFSM.onStateChange += OnStateChange;

            currentLife = defaultLife;
            fillHolder.SetCurrentFill(currentLife, defaultLife);
            canvas.gameObject.SetActive(false);

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        }

        private void Start()
        {
            creatureFSM.Initialize(player, transform);
            canCatch = CreatureController.Instance.CanCatchCreature(creatureType);
            CreatureController.Instance.onCreatureCountUpdated += CreatureCatched;
        }

        private void FixedUpdate()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (characterController.isGrounded)
                gravity.y = 0;
            else
                gravity.y = -1 * gravityScale * Time.fixedDeltaTime;

            // Do not aplly gravity when is on specific situations
            if(CanCatchCreature())
                characterController.Move(gravity);

            moviment = creatureFSM.GetMoviment();

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

        public void StartCatch()
        {
            creatureFSM.ChangeState(ECreatureStates.Running);

            if (canCatch)
            {
                if (catchCoroutine != null)
                {
                    StopCoroutine(catchCoroutine);
                    catchCoroutine = null;
                }

                catchCoroutine = StartCoroutine(CatchingRoutine());
                canvas.gameObject.SetActive(true);
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

            canvas.gameObject.SetActive(false);
        }

        IEnumerator CatchingRoutine()
        {
            while(currentLife > 0)
            {
                currentLife -= lifeRemoval;

                if(currentLife < 0)
                    currentLife = 0;

                fillHolder.SetCurrentFill(currentLife, defaultLife);

                yield return null;
            }

            Catched();
        }

        void Catched()
        {
            StopCatch();
            player.CatchCreature(this);
        }

        public void FinishCatching()
        {
            creatureFSM.ChangeState(ECreatureStates.Catched);
        }

        public void SetOnJail()
        {
            gameObject.SetActive(true);
            creatureFSM.ChangeState(ECreatureStates.Jailed);
        }

        public void FinishedMerge()
        {
            Destroy(gameObject);
        }

        private void CreatureCatched(ECreatureType type, int count) 
        {
            if (type != creatureType)
                return;

            canCatch = CreatureController.Instance.CanCatchCreature(creatureType);

            if (!canCatch)
            {
                StopCatch();
            }
        }
    }
}