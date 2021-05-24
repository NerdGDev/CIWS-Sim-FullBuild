using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMotionBase : MonoBehaviour
{
    protected Vector3 targetDirection = Vector3.forward;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        Vector3 rotation = lookRotation.eulerAngles;
        MotionRotateTowards(rotation);
    }

    public Vector3 GetTargetDirection()
    {
        return this.targetDirection;
    }

    public void SetTargetDirection(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void ResetDirection()
    {
        this.targetDirection = Vector3.forward;
    }

    virtual protected void MotionRotateTowards(Vector3 rotation)
    {

    }

    virtual public Quaternion GetRotation()    
    {
        return new Quaternion();
    }
}
