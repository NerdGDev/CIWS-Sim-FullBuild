using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBasic : MonoBehaviour
{
    public GameObject explosion;
    private Vector3 projectileDirection;

    private void Start()
    {
        transform.parent = GameObject.Find("ProjectileParent").transform;
    }

    public void Setup(Vector3 projectileDirection, float ttt)
    {
        Debug.Log(ttt);
        this.projectileDirection = projectileDirection;
        Destroy(gameObject, ttt);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += projectileDirection * Time.fixedDeltaTime * 1100;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (transform.position + (projectileDirection * 1.75f)));
    }

    private void OnDestroy()
    {
        //Destroy(Instantiate(explosion, transform.position, transform.rotation), 1f);
    }
}
