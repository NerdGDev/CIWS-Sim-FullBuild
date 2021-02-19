using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBasic : MonoBehaviour
{ 
    public float thrust = 1.0f;
    public Rigidbody rb;
    public int signal;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        signal = gameObject.GetInstanceID();
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        Destroy(this.gameObject, 0.2f);
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit");
        Destroy(this.gameObject, 0.2f);
    }

    private void OnDestroy()
    {
        GameObject go = Instantiate(explosion,transform.position,transform.rotation);
        Destroy(go, 5f);
    }
}