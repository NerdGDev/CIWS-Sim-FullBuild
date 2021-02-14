﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualMotionTurret : TurretMotionBase
{

    public Transform motionOneTranform;
    public float motionOneSpeed;
    public bool motionOneRotationX;
    public bool motionOneRotationY;
    public bool motionOneRotationZ;

    public Transform motionTwoTranform;
    public float motionTwoSpeed;
    public bool motionTwoRotationX;
    public bool motionTwoRotationY;
    public bool motionTwoRotationZ;

    override protected void MotionRotateTowards(Vector3 rotation)
    {
        if (rotation == Vector3.zero)
            return;
        float rotOneX = motionOneRotationX ? rotation.x : 0;
        float rotOneY = motionOneRotationY ? rotation.y : 0;
        float rotOneZ = motionOneRotationZ ? rotation.z : 0;
        Vector3 rotOne = new Vector3(rotOneX, rotOneY, rotOneZ);
        motionOneTranform.localRotation = Quaternion.RotateTowards(motionOneTranform.localRotation, Quaternion.Euler(rotOne), (Mathf.Deg2Rad * motionOneSpeed) * Time.fixedDeltaTime);

        float rotTwoX = motionTwoRotationX ? rotation.x : 0;
        float rotTwoY = motionTwoRotationY ? rotation.y : 0;
        float rotTwoZ = motionTwoRotationZ ? rotation.z : 0;
        Vector3 rotTwo = new Vector3(rotTwoX, rotTwoY, rotTwoZ);
        motionTwoTranform.localRotation = Quaternion.RotateTowards(motionTwoTranform.localRotation, Quaternion.Euler(rotTwo), (Mathf.Deg2Rad * motionTwoSpeed) * Time.fixedDeltaTime);
    }
}