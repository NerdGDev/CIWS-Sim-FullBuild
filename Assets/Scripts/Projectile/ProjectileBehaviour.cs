using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    private Vector3 projectileDirection;

    private void Start()
    {
        transform.parent = GameObject.Find("ProjectileParent").transform;
    }

    public void Setup(Vector3 projectileDirection, float ttt)
    {
        this.projectileDirection = projectileDirection;
        Destroy(gameObject, 0.7f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += projectileDirection * Time.fixedDeltaTime * 1100f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (transform.position + (projectileDirection * 1.75f)));
    }

    private void OnDestroy()
    {

    }
}
