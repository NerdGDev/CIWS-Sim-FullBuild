using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rb;

    public GameObject Explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 60f);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnDestroy()
    {
        Destroy(Instantiate(Explosion, transform.position, transform.rotation), 5f);
    }
}
