using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchRadarController : MonoBehaviour
{
    public Transform turretRing;

    public Transform radarBeamShape;
    public MeshCollider radarBeamCollider;

    public float rotationRate;

    public float optimalRange;
    public float width;
    public float height;
    public float elevation;

    public TrackingRadarController trackingRadarController;

    

    private void OnValidate()
    {
        radarBeamShape.localRotation = Quaternion.Euler(elevation, 0, 0);
        radarBeamShape.localScale = new Vector3(width, 1f * optimalRange * 1.5f, height);
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        turretRing.localRotation *= Quaternion.Euler(0, rotationRate * Time.fixedDeltaTime, 0);
        radarBeamCollider.GetComponent<Rigidbody>().MoveRotation(turretRing.localRotation * Quaternion.Euler(0,0,elevation));

    }

    public void SubmitDetection(Transform target) 
    {
        trackingRadarController.SubmitTarget(target);
    }

    

}
