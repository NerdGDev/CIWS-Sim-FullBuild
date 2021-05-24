using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMotion : SingleMotionTurret
{
    override protected void MotionRotateTowards(Vector3 rotation)
    {
        float rotX = motionRotationX ? (Mathf.Deg2Rad * motionSpeed) * Time.fixedDeltaTime : 0;
        float rotY = motionRotationY ? (Mathf.Deg2Rad * motionSpeed) * Time.fixedDeltaTime : 0;
        float rotZ = motionRotationZ ? (Mathf.Deg2Rad * motionSpeed) * Time.fixedDeltaTime : 0;
        Vector3 rot = new Vector3(rotX, rotY, rotZ);
        motionTranform.localRotation *= Quaternion.Euler(rot);
    }
}
