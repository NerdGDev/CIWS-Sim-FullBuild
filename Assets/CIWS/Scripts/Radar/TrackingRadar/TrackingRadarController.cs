using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRadarController : MonoBehaviour
{

    public float rotationSpeed = 240f;
    public Transform horizontalRotator;
    public Transform verticalRotator;

    public float rotX;
    public float rotY;
    public float rotZ;

    public Transform target;

    public FiringSolution turretController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            return;

        Debug.DrawLine(transform.position, target.position);

        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        Vector3 rotation = lookRotation.eulerAngles;
        GunRotateTowards(rotation);

        Debug.Log("Sending Target");
        turretController.SubmitTargetData(target.position, target.GetComponent<Rigidbody>().velocity.normalized, target.GetComponent<Rigidbody>().velocity.magnitude);

    }

    private void GunRotateTowards(Vector3 rotation)
    {
        // Requires conversion to Local Rotation
        if (rotation == Vector3.zero)
            return;

        rotX = rotation.x;
        rotY = rotation.y;
        rotZ = rotation.z;

        horizontalRotator.rotation = Quaternion.RotateTowards(horizontalRotator.rotation, Quaternion.Euler(new Vector3(0, rotation.y, 0)), rotationSpeed * Time.fixedDeltaTime);
        verticalRotator.localRotation = Quaternion.RotateTowards(verticalRotator.localRotation, Quaternion.Euler(new Vector3(rotation.x, 0, 0)), rotationSpeed * Time.fixedDeltaTime);
    }

    public void SubmitTarget(Transform target) 
    {
        this.target = target;
    }
}
