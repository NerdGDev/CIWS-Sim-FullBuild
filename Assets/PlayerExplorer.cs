using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplorer : MonoBehaviour
{
    public float panSpeed = 250f;



    KeyCode forwardKey = KeyCode.W;
    KeyCode backwardKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(forwardKey))
        {
            pos.z += panSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(backwardKey))
        {
            pos.z -= panSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(leftKey))
        {
            pos.x -= panSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(rightKey))
        {
            pos.x += panSpeed * Time.fixedDeltaTime;
        }

        pos.y += Input.mouseScrollDelta.y * panSpeed * Time.fixedDeltaTime; 


        transform.position = pos;
    }
}