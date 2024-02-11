namespace Project.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AIBehaviours/Catched")]
    public class CatchedBehaviour : AIBehaviourBase
    {
        public override IEnumerator ExecuteBehaviourRoutine(CharacterController target)
        {
            throw new System.NotImplementedException();
        }
    }
}