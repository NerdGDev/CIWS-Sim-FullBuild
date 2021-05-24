using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireSystem : MonoBehaviour
{
    public GameObject projectileReference;
    public float muzzleVelocity = 300f;

    public void Fire(GameObject muzzle, Vector3 direction, float fuse) 
    {
        Fire(muzzle.transform, direction, fuse);
    }

    public void Fire(Transform muzzle, Vector3 direction, float fuse) 
    {
        GameObject bullet = Instantiate(projectileReference, muzzle.position, Quaternion.LookRotation(direction));
        bullet.GetComponent<Projectile>().fuse = fuse;
        bullet.GetComponent<Rigidbody>().velocity = direction.normalized * muzzleVelocity;
        //Debug.LogWarning((direction.normalized * muzzleVelocity).magnitude);
    }

    public void SetMuzzleVelocity(float mv) 
    {
        muzzleVelocity = mv;
    }
}
