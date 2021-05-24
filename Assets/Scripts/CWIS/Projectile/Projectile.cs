using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float fuse = 10f;

    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, fuse - 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Destroy(Instantiate(Explosion, transform.position, transform.rotation), 1f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 150f);
        foreach (Collider col in colliders) 
        {
            if (col.GetComponent<Missile>()) 
            {
                Destroy(col.gameObject, 0.1f);
            }
        }
    }
}
