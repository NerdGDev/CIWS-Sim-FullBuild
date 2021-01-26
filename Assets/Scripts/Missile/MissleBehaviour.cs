using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleBehaviour : MonoBehaviour
{

    public float thrust = 1.0f;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * thrust;
        //rb.AddForce(transform.forward * thrust);
    }
}