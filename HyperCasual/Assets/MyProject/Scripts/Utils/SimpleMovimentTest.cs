using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovimentTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.fixedDeltaTime * 6);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.fixedDeltaTime * 6);
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * 6);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Time.fixedDeltaTime * 6);
        }
    }
}
