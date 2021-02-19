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
    public Transform muzzle;
    public float muzzleVelocity = 1100f;
    public Transform projectile;
    //public CommandController commandController;

    public float fireRate;
    public bool canShoot = true;

    //TargetAssignment targetAssignment;
    Coroutine burstCoroutine;
    public FiringState firingState;
    public int ammoCount;
    public int burstSize;

    public bool burstFireStart = false;

    

    public (Vector3 direction, float timeToImpact) fs = (Vector3.zero, 0f);

    public enum FiringState
    {
        IDLE,
        FIRING,
        CYCLING,
        RELOADING,
        EMPTY
    }



    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (burstFireStart) 
        {
            burstFireStart = false;
            StartCoroutine(Burst());
        }
        if (firingState == FiringState.EMPTY || ammoCount <= 0) 
        {
            StartCoroutine(Reload());
        }
        
    }

    IEnumerator Reload() 
    {
        firingState = FiringState.RELOADING;
        while (ammoCount < 600)
        {
            ammoCount++;
            yield return new WaitForSeconds(0.005f);
        }
        firingState = FiringState.IDLE;
    }

    

    private void FixedUpdate()
    {
        
    }

    public void StartBurst() 
    {
        //Debug.Log("Start Burst");
        burstFireStart = true;
        
    }

    IEnumerator Burst() 
    {
        //Debug.Log("Burst");
        //Debug.Log(burstSize);
        firingState = FiringState.FIRING;
        for (int i = 0; i < burstSize; i++) 
        {
            //Debug.Log("Fire : " + i);
            //StartCoroutine(BurstShot());
            Fire();
            yield return new WaitForSeconds(fireRate);
            //yield return StartCoroutine(Burst(burstSize));
        }
        firingState = FiringState.CYCLING;
        yield return new WaitForSeconds(1f + fs.timeToImpact);
        firingState = FiringState.IDLE;

    }

    IEnumerator BurstShot() 
    {
        Fire();
        yield return new WaitForSeconds(1 / fireRate);
    }

    public bool AimTo(Vector3 target) 
    {
        if (target == null) {
            return false;
        }
        tm.SetTargetDirection(target);
        //Debug.Log(Quaternion.Angle(tm.GetRotation(), Quaternion.LookRotation(target)));
        if (Quaternion.Angle(tm.GetRotation(), Quaternion.LookRotation(target)) <= 0.5f) 
        {
            
            //Debug.Log("Can Fire");
            return true;
        }


        return false;
    }

    public void UpdateFireSolution((Vector3 direction, float timeToImpact) fs) 
    {
        this.fs = fs;
    }

    public void Fire() 
    {
        //Debug.Log("Fire");
        if (ammoCount <= 0) 
        {
            //Debug.Log("Empty");
            firingState = FiringState.EMPTY;
            StopCoroutine(burstCoroutine);
            return;
        }   
        ammoCount--;
        Transform bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
        bullet.GetComponent<ProjectileBasic>().Setup((Quaternion.LookRotation(GetPointOnUnitSphereCap(muzzle.rotation, 0.00025f)) * Vector3.forward) * 100f, fs.timeToImpact);
        //Debug.Log("Fire");
        Debug.DrawLine(muzzle.position, muzzle.position + (fs.direction.normalized * muzzleVelocity * (fs.timeToImpact)), Color.magenta);
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
