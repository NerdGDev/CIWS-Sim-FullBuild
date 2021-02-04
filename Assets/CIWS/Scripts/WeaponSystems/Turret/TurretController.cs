using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    FiringSolution firingSolution;

    public float rotationSpeed = 50f;
    public Transform horizontalRotator;
    public Transform verticalRotator;

    public Vector3 weaponRotation;

    public float angleToTarget;

    public FiringSolutionData firingSolutionData = new FiringSolutionData();

    private void Start()
    {
        firingSolution = transform.GetComponent<FiringSolution>();
    }

    void FixedUpdate()
    {
        if (firingSolutionData.GetValid() && firingSolutionData != null) {
            Quaternion lookRotation = Quaternion.LookRotation(firingSolutionData.GetRotation());
            Vector3 rotation = lookRotation.eulerAngles;
            GunRotateTowards(rotation);
            this.angleToTarget = Quaternion.Angle(verticalRotator.rotation, lookRotation);

        }
    }

    public void SubmitFiringSolution(FiringSolutionData firingSolutionData) 
    {
        this.firingSolutionData = firingSolutionData;
    }

    private void GunRotateTowards(Vector3 rotation) {
        // Requires conversion to Local Rotation
        if (rotation == Vector3.zero)
            return;
        horizontalRotator.localRotation = Quaternion.RotateTowards(horizontalRotator.localRotation, Quaternion.Euler(new Vector3(0, rotation.y, 0)), rotationSpeed * Time.fixedDeltaTime);
        verticalRotator.localRotation = Quaternion.RotateTowards(verticalRotator.localRotation, Quaternion.Euler(new Vector3(rotation.x, 0, rotation.z)), rotationSpeed / 1.5f * Time.fixedDeltaTime);
    }

    public void setWeaponRotation(Vector3 weaponDirection) {
        this.weaponRotation = weaponDirection;
    }

    private void OnDrawGizmos()
    {
        Vector3 turretCenter = transform.position + (verticalRotator.position - transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(turretCenter, turretCenter + (weaponRotation.normalized * 100f));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(turretCenter, (verticalRotator.position + (verticalRotator.rotation * Vector3.forward) * 50f));
    }
}
