namespace Project.Characters
{
    using Project.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Character : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private JoystickUI joystick;
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

        private void FixedUpdate()
        {
            if (inputX == 0f && inputZ == 0f)
            {
                smokeEffect.enableEmission = false;
                return;
            }

            smokeEffect.enableEmission = true;

            moviment = new Vector3(inputX * movimentSpeed * Time.fixedDeltaTime, 0f, inputZ * movimentSpeed * Time.fixedDeltaTime);

            transform.localRotation = Quaternion.LookRotation(moviment);

            characterController.Move(moviment);
        }

        private void Update()
        {
            inputX = joystick.InputHorizontal;
            inputZ = joystick.InputVertical;
        }

        public void CatchCreature(Creature creature)
        {
            creature.transform.SetParent(catchedList);
            creatures.Add(creature);
            creature.FinishCatching();
        }
    }
}