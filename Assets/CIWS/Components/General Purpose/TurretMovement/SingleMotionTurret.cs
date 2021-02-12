using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMotionTurret : TurretMotionBase
{

    public Transform motionTranform;
    public float motionSpeed;
    public bool motionRotationX;
    public bool motionRotationY;
    public bool motionRotationZ;

    override protected void MotionRotateTowards(Vector3 rotation)
    {
        if (rotation == Vector3.zero)
            return;
        float rotX = motionRotationX ? rotation.x : 0;
        float rotY = motionRotationY ? rotation.y : 0;
        float rotZ = motionRotationZ ? rotation.z : 0;
        Vector3 rot = new Vector3(rotX, rotY, rotZ);
        motionTranform.localRotation = Quaternion.RotateTowards(motionTranform.localRotation, Quaternion.Euler(rot), (Mathf.Deg2Rad * motionSpeed) * Time.fixedDeltaTime);
    }
}
