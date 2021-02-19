using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionMeshHandler : MonoBehaviour
{
    public GameObject visualPrefab;

    public delegate void DetectionEnter(int signatureID, Vector3 position);
    public DetectionEnter detectionEnterDelegate;

    public delegate void DetectionStay(int signatureID, Vector3 position);
    public DetectionStay detectionStayDelegate;

    public delegate void DetectionExit(int signatureID, Vector3 position);
    public DetectionExit detectionExitDelegate;

    private void OnTriggerEnter(Collider other)
    {
        if (detectionEnterDelegate != null)
        {
            detectionEnterDelegate(other.gameObject.GetInstanceID(), other.transform.position);
        }
        GameObject visualiser = Instantiate(visualPrefab, transform.position, transform.rotation);
        visualiser.GetComponent<RadarBounceVisualiser>().origin = transform;
        visualiser.GetComponent<RadarBounceVisualiser>().target = other.transform;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (detectionStayDelegate != null) {
            detectionStayDelegate(other.gameObject.GetInstanceID(), other.transform.position);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (detectionExitDelegate != null)
        {
            detectionExitDelegate(other.gameObject.GetInstanceID(), other.transform.position);
        }
    }
}
