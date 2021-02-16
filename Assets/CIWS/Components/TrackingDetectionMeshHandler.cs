using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDetectionMeshHandler : MonoBehaviour
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
            detectionEnterDelegate(other.transform.GetInstanceID(), other.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (detectionStayDelegate != null)
        {
            detectionStayDelegate(other.transform.GetInstanceID(), other.transform.position);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (detectionExitDelegate != null)
        {
            detectionExitDelegate(other.transform.GetInstanceID(), other.transform.position);
        }
    }
}

