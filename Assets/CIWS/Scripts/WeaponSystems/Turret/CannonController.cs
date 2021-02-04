using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    TurretController turretController;
    SearchRadar searchRadar;

    [SerializeField]
    private Transform projectile;

    private float barrelAccuracy = 0f;

    public float firerate = 1f / (60f / 1000f);
    public float lastShot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.turretController = transform.GetComponent<TurretController>();
        this.searchRadar = transform.GetComponent<SearchRadar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void Fire(FiringSolutionData firingSolutionData) 
    {
        Debug.Log("VibeCheck");
        Debug.Log(lastShot + firerate);
        Debug.Log(Time.time);
        if (lastShot + firerate <= Time.time) 
        {
            Debug.Log("Fire");
            Transform bullet = Instantiate(projectile, turretController.verticalRotator.position, turretController.verticalRotator.rotation);
            bullet.GetComponent<ProjectileBehaviour>().Setup((Quaternion.LookRotation(GetPointOnUnitSphereCap(turretController.verticalRotator.rotation, barrelAccuracy)) * Vector3.forward) * 100f, firingSolutionData.GetTimeToTarget()); ;
            lastShot = Time.time;
            
        }
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
