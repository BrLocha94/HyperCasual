namespace Project.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SimpleMovimentTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.fixedDeltaTime * 6);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.fixedDeltaTime * 6);
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.fixedDeltaTime * 6);
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Time.fixedDeltaTime * 6);
            }
        }
    }
}