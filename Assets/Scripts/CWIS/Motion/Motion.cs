using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] public Transform CenterPivot;
    [SerializeField] public Vector3 targetDirection = Vector3.forward;
    [SerializeField] public bool isActive;

    [SerializeField] public bool inherit;
    [SerializeField] public Transform InheritRoot;
    [SerializeField] public Transform InheritLocation;

    [SerializeField] public RotationData[] rotations;


    [Header("Debug")]
#if UNITY_EDITOR
    public bool DebugAngles;
    public bool UseTransform;
    public Transform TransformTarget;
#endif
    private void Awake()
    {
        for (int x = 0; x < rotations.Length; x++)
        {
            rotations[x].offSet = rotations[x].transform.rotation;
        }
    }

    public float GetAlignment() 
    {
        return Quaternion.Angle(CenterPivot.rotation, Quaternion.LookRotation(targetDirection, Vector3.up));
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (UseTransform)
            TargetToDirection(TransformTarget.position);
#endif
        if (inherit) 
        {
            InheritRoot.position = InheritLocation.position;
        }

        if (isActive)
        {
            UpdateMotion();
        }
    }

    public void TargetToDirection(Vector3 target) 
    {
        targetDirection = target - CenterPivot.transform.position;
    }

    virtual public void UpdateMotion()
    {
        
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

            

        Vector3 rotation = lookRotation.eulerAngles;
        if (rotation == Vector3.zero)
            return;

        foreach (RotationData data in rotations) 
        {
            float x = data.x ? rotation.x : 0;
            float y = data.y ? rotation.y : 0;
            float z = data.z ? rotation.z : 0;
            Vector3 rot = new Vector3(x, y, z);
            data.transform.localRotation = Quaternion.RotateTowards(data.transform.localRotation, Quaternion.Euler(rot), data.rotationSpeed * Time.fixedDeltaTime);
        }
    }

    virtual public void ResetDirection()
    {
        targetDirection = transform.forward;
    }

    [System.Serializable]
    public struct RotationData 
    {
        public Transform transform;
        public float rotationSpeed;
        public Quaternion offSet;
        public bool x;
        public bool y;
        public bool z;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(CenterPivot.position, CenterPivot.position + (targetDirection.normalized * 5000f));
    }
}
