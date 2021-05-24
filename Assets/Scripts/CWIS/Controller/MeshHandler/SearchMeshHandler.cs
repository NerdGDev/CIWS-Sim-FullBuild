using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMeshHandler : MonoBehaviour
{
    SearchRadarController controller;

    public GameObject SearchFXReference;

    private void Awake()
    {
        controller = GetComponentInParent<SearchRadarController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        controller.ReportHit(other.gameObject.GetInstanceID(), other.gameObject.transform.position);
        GameObject go = Instantiate(SearchFXReference, other.transform.position, new Quaternion());
        go.GetComponent<RadarDetectionFXHandle>().origin = other.transform;
        go.GetComponent<RadarDetectionFXHandle>().target = transform;
        Destroy(go, 2f);
    }

    private void OnTriggerStay(Collider other)
    {
        controller.ReportHit(other.gameObject.GetInstanceID(), other.gameObject.transform.position);
    }


}
