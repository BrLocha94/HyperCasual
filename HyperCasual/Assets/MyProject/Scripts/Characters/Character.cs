namespace Project.Characters
{
    using Project.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Enums;

    public class Character : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private Transform rotationPivot;
        [SerializeField]
        private GameObject messageCanvas;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private JoystickUI joystick;
        [SerializeField]
        private CatchArea catchArea;
        [SerializeField]
        private ParticleSystem smokeEffect;
        [SerializeField]
        private Transform catchedList;
        [SerializeField]
        private float movimentSpeed = 5f;
        
        private float inputX = 0f;
        private float inputZ = 0f;

        private Vector3 moviment = Vector3.zero;
        private List<Creature> creatures = new List<Creature>();

        private void Awake()
        {
            catchArea.onCountUpdated += OnCatchListUpdated;
        }

        private void OnCatchListUpdated(int count)
        {
            animator.SetLayerWeight(1, count > 0 ? 1 : 0);
        }

        private void FixedUpdate()
        {
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

            float movimentScale = (moviment.x > 0.5f || moviment.z > 0.5f) ? 1f : 0.5f;

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
    }
}