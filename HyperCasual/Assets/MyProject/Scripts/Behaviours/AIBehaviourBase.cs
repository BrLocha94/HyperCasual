namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AIBehaviourBase : ScriptableObject
    {
        public abstract IEnumerator ExecuteBehaviourRoutine(CharacterController target);
    }
}