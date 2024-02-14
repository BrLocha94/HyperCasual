namespace Project.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterBase : MonoBehaviour
    {
        [Header("Character Base")]
        [SerializeField]
        protected CharacterController characterController;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Transform rotationPivot;
        [SerializeField]
        protected ParticleSystem smokeEffect;
    }
}