namespace Project.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraMoviment : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float distanceYFromTarget = 11f;
        [SerializeField]
        private float distanceZFromTarget = -11f;


        private void Update()
        {
            if (target == null)
                return;

            transform.position = new Vector3(
                target.position.x,
                target.position.y + distanceYFromTarget,
                target.position.z + distanceZFromTarget
                );
        }
    }
}