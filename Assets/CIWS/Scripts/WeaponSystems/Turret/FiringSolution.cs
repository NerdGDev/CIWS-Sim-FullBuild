using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringSolution : MonoBehaviour
{
    TurretController turretController;
    CannonController cannonController;
    SearchRadar searchRadar;

    public float range = 10000f;
    public float muzzleVelocity = 1100f;

    public float acceptableFiringAngle = 0.1f;

    public Vector3 targetPosition;
    public Vector3 targetDirection;
    public float targetSpeed;

    public bool targetActive = true;

    public FiringSolutionData fireSolution;

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

    public void SubmitTargetData(Vector3 targetPosition, Vector3 targetDirection, float targetSpeed) {
        this.targetPosition = targetPosition;
        this.targetDirection = targetDirection;
        this.targetSpeed = targetSpeed;
    }

    private void FixedUpdate()
    {
        if (!targetActive)
            return;

        if (targetPosition == null || targetDirection == null) {
            Debug.LogWarning("System does not have valid firing components");
        }

        fireSolution = GenerateFiringSolutionNew(); ;
        turretController.SubmitFiringSolution(fireSolution);

        if (fireSolution.GetTimeToTarget() * muzzleVelocity > range)
            return;
                
        if (turretController.angleToTarget < 0.5f)
        {
            cannonController.Fire(fireSolution);
        }


    }

    //Vector3 GenerateFiringSolution() {
    //    Vector3 turretCenter = transform.position + (turretController.verticalRotator.position - transform.position);
    //    Vector3 targetDirection = (targetPosition - turretCenter);
    //    Vector3 targetLead;
    //    float t;
    //    t = (targetDirection.magnitude / muzzleVelocity * Time.fixedDeltaTime);
    //    for (int i = 0; i < 100; i++) {
    //        targetLead = (((targetPosition + ((this.targetDirection * targetSpeed)) * t) - turretCenter));
    //        t = (targetLead.magnitude / muzzleVelocity * Time.fixedDeltaTime);
    //    }

    //    return (((targetPosition + ((this.targetDirection * targetSpeed)) * t) - turretCenter));

    //}

    FiringSolutionData GenerateFiringSolutionNew()
    {
        Vector3 turretCenter = transform.position + (turretController.verticalRotator.position - transform.position);

        Vector3 p0 = targetPosition;
        Vector3 v0 = targetDirection;
        float s0 = targetSpeed;

        Vector3 p1 = turretCenter;
        float s1 = muzzleVelocity;

        float a = ((v0.x * v0.x) + (v0.y * v0.y) + (v0.z * v0.z)) - (s1 * s1);
        float b = 2 * (((p0.x * v0.x) + (p0.y * v0.y) + (p0.z * v0.z)) - ((p1.x * v0.x) - (p1.y * v0.y) - (p1.z * v0.z)));
        float c = ((p0.x * p0.x) + (p0.y * p0.y) + (p0.z * p0.z)) + ((p1.x * p1.x) + (p1.y + p1.y) + (p1.z + p1.z)) - ((2 * p1.x * p0.x) - (2 * p1.y * p0.y) - (2 * p1.z * p0.z));

        float t1 = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t2 = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float t;

        if (t1 >= 0 && (t2 >= 0 ? t1 < t2 : true))
        {
            t = t1;
        }
        else if (t2 >= 0 && (t1 >= 0 ? t2 < t1 : true))
        {
            t = t2;
        }
        else 
        {
            return new FiringSolutionData();
        }

        t *= Time.fixedDeltaTime;
        t *= 1.5f;
        //Debug.Log(t);

        Vector3 v = (p0 - p1 + (t * s0 * v0)) / (t * s1);
        v.x = (p0.x - p1.x + (t * s0 * v0.x)) / (t * s1);
        v.y = (p0.y - p1.y + (t * s0 * v0.y)) / (t * s1);
        v.z = (p0.z - p1.z + (t * s0 * v0.z)) / (t * s1);

        return new FiringSolutionData(v, t);
    }

    public bool QuadSolver() 
    {

        return false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            Vector3 turretCenter = transform.position + turretController.verticalRotator.localPosition;
            Vector3 dir = targetPosition - turretCenter;
            Vector3 lead = ((targetPosition + ((targetSpeed * (targetDirection * (dir.magnitude / muzzleVelocity))) * Time.fixedDeltaTime)) - turretCenter);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(turretCenter, turretCenter + (fireSolution.GetRotation() * fireSolution.GetTimeToTarget()));
            Gizmos.DrawWireSphere(transform.position, range);
        }

    }
}

public class FiringSolutionData
{
    Vector3 rotationToTarget;
    float timeToTarget;
    bool valid = false;

    public FiringSolutionData(Vector3 rotationToTarget, float timeToTarget) 
    {
        this.valid = true;
        this.rotationToTarget = rotationToTarget;
        this.timeToTarget = timeToTarget;
    }

    public FiringSolutionData()
    {
        this.valid = false;
    }

    public Vector3 GetRotation() 
    {
        return rotationToTarget;
    }

    public float GetTimeToTarget() 
    {
        return timeToTarget;
    }

    public bool GetValid()
    {
        return valid;
    }
}
