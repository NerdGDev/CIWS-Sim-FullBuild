using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringSolution : MonoBehaviour
{
    TurretController turretController;
    CannonController cannonController;
    SearchRadar searchRadar;

    public float range = 10000f;
    public float muzzleVelocity = 100f;

    public float acceptableFiringAngle = 0.1f;

    public Vector3 targetPosition;
    public Vector3 targetDirection;
    public float targetVelocity;

    public bool targetActive = true;

    // Start is called before the first frame update
    void Start()
    {
        this.turretController = transform.GetComponent<TurretController>();
        this.cannonController = transform.GetComponent<CannonController>();
        this.searchRadar = transform.GetComponent<SearchRadar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void submitTargetData(Vector3 targetPosition, Vector3 targetDirection, float targetVelocity) {
        this.targetPosition = targetPosition;
        this.targetDirection = targetDirection;
        this.targetVelocity = targetVelocity;
    }

    private void FixedUpdate()
    {
        if (!targetActive)
            return;

        if (targetPosition == null || targetDirection == null) {
            Debug.LogWarning("System does not have valid firing components");
        }

        Vector3 turretCenter = transform.position + turretController.verticalRotator.localPosition;
        Vector3 dir = targetPosition - turretCenter;
        Vector3 lead = ((targetPosition + ((targetVelocity * (targetDirection * (dir.magnitude / muzzleVelocity))) * Time.fixedDeltaTime)) - turretCenter);

        turretController.setWeaponRotation(lead);

        if (lead.magnitude > range)
            return;
                
        if (turretController.angleToTarget < 0.5f)
        {
            cannonController.Fire();
        }


    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            Vector3 turretCenter = transform.position + turretController.verticalRotator.localPosition;
            Vector3 dir = targetPosition - turretCenter;
            Vector3 lead = ((targetPosition + ((targetVelocity * (targetDirection * (dir.magnitude / muzzleVelocity))) * Time.fixedDeltaTime)) - turretCenter);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(turretCenter, turretCenter + lead);
            Gizmos.DrawWireSphere(transform.position, range);
        }

    }
}
