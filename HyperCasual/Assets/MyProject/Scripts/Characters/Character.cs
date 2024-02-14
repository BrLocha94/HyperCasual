namespace Project.Characters
{
    using Project.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Enums;

    public class Character : CharacterBase
    {
        [Header("Character")]
        [Header("External references")]
        [SerializeField]
        private GameObject messageCanvas;
        [SerializeField]
        private GameObject vfxCatch;
        [SerializeField]
        private JoystickUI joystick;
        [SerializeField]
        private CatchArea catchArea;
        [SerializeField]
        private Transform catchedList;

        [Header("Character configurations")]
        [SerializeField]
        private float movimentSpeed = 5f;
        [SerializeField]
        private float gravityScale = 1f;
        
        private float inputX = 0f;
        private float inputZ = 0f;

        private Vector3 moviment = Vector3.zero;
        private Vector3 gravity = Vector3.zero;

        private List<Creature> creatures = new List<Creature>();

        private void Awake()
        {
            catchArea.onCountUpdated += OnCatchListUpdated;
            OnCatchListUpdated(0, false);
        }

        private void OnCatchListUpdated(int count, bool anyIsFull)
        {
            animator.SetLayerWeight(1, count > 0 ? 1 : 0);

            if (anyIsFull)
                messageCanvas.SetActive(true);
            else
                messageCanvas.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (characterController.isGrounded)
                gravity.y = 0;
            else
                gravity.y = -1 * gravityScale * Time.fixedDeltaTime;

            characterController.Move(gravity);

            if (inputX == 0f && inputZ == 0f)
            {
                smokeEffect.enableEmission = false;
                animator.SetFloat("Moviment", 0f);
                return;
            }

            smokeEffect.enableEmission = true;

            moviment = new Vector3(inputX * movimentSpeed * Time.fixedDeltaTime, 0f, inputZ * movimentSpeed * Time.fixedDeltaTime);

            rotationPivot.localRotation = Quaternion.LookRotation(moviment);

            characterController.Move(moviment);

            float movimentScale = (
                moviment.x > 0.08f || 
                moviment.x < -0.08f ||
                moviment.z > 0.08f ||
                moviment.z < -0.08f) ? 1f : 0.5f;

            animator.SetFloat("Moviment", movimentScale);
        }

        private void Update()
        {
            inputX = joystick.InputHorizontal;
            inputZ = joystick.InputVertical;
        }

        public void CatchCreature(Creature creature)
        {
            if (!CreatureController.Instance.CanCatchCreature(creature.GetCreatureType()))
                return;

            creature.transform.SetParent(catchedList);
            creatures.Add(creature);
            creature.FinishCatching();
            CreatureController.Instance.CatchCreature(creature.GetCreatureType());

            catchArea.CatchedCreature(creature);

            StartCoroutine(CatchVfxRoutine());
        }

        public List<Creature> GetCatchedCreaturesByType(List<ECreatureType> typeList)
        {
            if (creatures.Count == 0)
                return null;

            List<Creature> list = new List<Creature>();

            for(int i = 0; i < typeList.Count; i++) 
            {
                for(int j = 0; j < creatures.Count; j++)
                {
                    if (creatures[j].GetCreatureType() == typeList[i])
                    {
                        list.Add(creatures[j]);
                        creatures.RemoveAt(j);
                        break;
                    }
                }
            }

            return list;
        }

        IEnumerator CatchVfxRoutine()
        {
            yield return new WaitForSeconds(1.2f);
            
            vfxCatch.SetActive(false);

            yield return new WaitForEndOfFrame();

            vfxCatch.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            vfxCatch.SetActive(false);
        }
    }
}