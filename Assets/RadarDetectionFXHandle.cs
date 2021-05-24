using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarDetectionFXHandle : MonoBehaviour
{
    public ParticleSystem ps;

    public Transform origin;
    public Transform target;


    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    private void Start()
    {
        ps.Play();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (origin != null) 
        {
            transform.position = origin.position;
        }        
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        RunFX();
    }

    public void RunFX() 
    {
        var main = GetComponent<ParticleSystem>().main;
        main.startLifetime = (target.position - transform.position).magnitude / main.startSpeed.constant;
    }
}
