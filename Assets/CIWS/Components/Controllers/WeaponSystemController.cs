using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemController : MonoBehaviour
{
    public bool isBusy;
    public bool isAssigned;

    public FireControlSystem fcs;

    public TurretMotionBase tm;

    public Transform barrelComponent;
    public float muzzleVelocity = 1100f;
    public Transform projectile;
    //public CommandController commandController;

    //TargetAssignment targetAssignment;


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
        
    }

    public bool AimTo(Vector3 target) 
    {
        if (target == null) {
            return false;
        }
        tm.SetTargetDirection(target);
        Debug.Log(Quaternion.Angle(tm.GetRotation(), Quaternion.LookRotation(target)));
        if (Quaternion.Angle(tm.GetRotation(), Quaternion.LookRotation(target)) <= 0.1f) 
        {
            
            Debug.Log("Can Fire");
            return true;
        }


        return false;
    }

    public void Fire(Vector3 direction, float timeToImpact) 
    {
        Transform bullet = Instantiate(projectile, barrelComponent.position, barrelComponent.rotation);
        bullet.GetComponent<ProjectileBasic>().Setup((Quaternion.LookRotation(GetPointOnUnitSphereCap(barrelComponent.rotation, 0.00025f)) * Vector3.forward) * 100f, timeToImpact);
        Debug.Log("Fire");
        Debug.DrawLine(transform.position, transform.position + (direction.normalized * muzzleVelocity * (timeToImpact + 0.5f)), Color.magenta);
    }

    public static Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle)
    {
        var angleInRad = Random.Range(0.1f, angle) * Mathf.Deg2Rad;
        var PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        var V = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * V;
    }

    public static Vector3 GetPointOnUnitSphereCap(Vector3 targetDirection, float angle)
    {
        return GetPointOnUnitSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }
}
