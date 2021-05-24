using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motion))]
public class TurretController : MonoBehaviour
{
    public Motion motion;
    public Transform muzzle;
    public Transform optic;
    [SerializeField] public Vector3 TurretOffset;

    ProjectileFireSystem projectileFireSystem;
    bool FireCalled;

    FireConfiguration fireConfiguration;

    public float burstCount = 8f;
    public float burst;

    Visualise visualise;

    private void Awake()
    {
        visualise = GetComponent<Visualise>();
        burst = burstCount;
        projectileFireSystem = GetComponent<ProjectileFireSystem>();        
        StartCoroutine(FireUpdater());
    }

    public float GetAlignmentAngle(Vector3 direction) 
    {
        return Quaternion.Angle(motion.CenterPivot.rotation, Quaternion.LookRotation(direction, Vector3.up));
    }

    public void ExecuteFireSolution(Vector3 direction, float timeToImpact) 
    {        
        motion.targetDirection = direction;
        if (GetAlignmentAngle(direction) <= 0.5f) 
        {
            Fire(direction, timeToImpact);
        }
    }

    void Fire(Vector3 direction, float fuse) 
    {
        fireConfiguration = new FireConfiguration(direction, fuse);
        FireCalled = true;
    }

    IEnumerator FireUpdater() 
    {
        while (true) 
        {
            if (FireCalled && burst > 0)
            {
                visualise.AddShortData("Action", "Fire");
                FireCalled = false;
                //Debug.LogWarning("Fire");
                //Debug.LogWarning(fireConfiguration.fuse);
                projectileFireSystem.Fire(muzzle, fireConfiguration.direction, fireConfiguration.fuse);
                burst--;
                yield return new WaitForSeconds(1 / 75);
                if (burst == 0) 
                {
                    visualise.AddShortData("Action", "Reloading");
                    StartCoroutine(ReloadBurst());
                }
            }
            else 
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator ReloadBurst() 
    {
        yield return new WaitForSeconds(2.5f);
        burst = burstCount;
    }
}

[System.Serializable]
public class FireConfiguration 
{
    public FireConfiguration(Vector3 d, float f) 
    {
        direction = d;
        fuse = f;
    }

    public Vector3 direction;
    public float fuse;
}

