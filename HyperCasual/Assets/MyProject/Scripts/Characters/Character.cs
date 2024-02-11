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

        private float inputX = 0f;
        private float inputZ = 0f;
        private float movimentSpeed = 5f;

        private Vector3 moviment = Vector3.zero;


        private void FixedUpdate()
        {
            if (inputX == 0f && inputZ == 0f)
                return;

            moviment = new Vector3(inputX * movimentSpeed * Time.fixedDeltaTime, 0f, inputZ * movimentSpeed * Time.fixedDeltaTime);

            transform.localRotation = Quaternion.LookRotation(moviment);

            characterController.Move(moviment);
        }

        private void Update()
        {
            inputX = joystick.InputHorizontal;
            inputZ = joystick.InputVertical;
        }
    }
}