using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSearchMesh : MonoBehaviour
{

    public SearchRadarController parentSearchRadarController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter Mesh");
        Debug.DrawLine(transform.position, other.transform.position, Color.red, 0f, false);
        parentSearchRadarController.SubmitDetection(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger Stay Mesh");
        Debug.DrawLine(transform.position, other.transform.position, Color.red, 0f, false);
    }

}

    
