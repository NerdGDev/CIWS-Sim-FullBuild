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

    private void Start()
    {
        firingSolution = transform.GetComponent<FiringSolution>();
    }

    void FixedUpdate()
    {
        if (firingSolution.targetActive) {
            Quaternion lookRotation = Quaternion.LookRotation(weaponRotation);
            Vector3 rotation = lookRotation.eulerAngles;
            GunRotateTowards(rotation);
            this.angleToTarget = Quaternion.Angle(verticalRotator.rotation, lookRotation);
        }
    }

    private void GunRotateTowards(Vector3 rotation) {
        // Requires conversion to Local Rotation
        horizontalRotator.rotation = Quaternion.RotateTowards(horizontalRotator.rotation, Quaternion.Euler(new Vector3(0, rotation.y, 0)), rotationSpeed * Time.fixedDeltaTime);
        verticalRotator.rotation = Quaternion.RotateTowards(verticalRotator.rotation, Quaternion.Euler(new Vector3(rotation.x, rotation.y, rotation.z)), rotationSpeed / 1.5f * Time.fixedDeltaTime);
    }

    public void setWeaponRotation(Vector3 weaponDirection) {
        this.weaponRotation = weaponDirection;
    }

    private void OnDrawGizmos()
    {
        Vector3 turretCenter = transform.position + verticalRotator.localPosition;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(turretCenter, turretCenter + (weaponRotation.normalized * 100f));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(turretCenter, (verticalRotator.position + (verticalRotator.rotation * Vector3.forward) * 50f));
    }
}
