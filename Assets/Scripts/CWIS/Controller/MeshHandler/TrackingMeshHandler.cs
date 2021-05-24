using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMeshHandler : MonoBehaviour
{
    TrackingRadarController controller;
    private void Awake()
    {
        controller = GetComponentInParent<TrackingRadarController>();
    }

    private void OnTriggerStay(Collider other)
    {
        controller.ReportHit(other.gameObject.GetInstanceID(), other.gameObject.transform.position);
    }
}
