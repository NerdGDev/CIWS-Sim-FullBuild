using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchRadar : MonoBehaviour
{
    FiringSolution firingSolution;

    public Transform target;

    public float searchRange;

    // Start is called before the first frame update
    void Start()
    {
        firingSolution = transform.GetComponent<FiringSolution>();
    }

    // Update is called once per frame
    void Update()
    {
        firingSolution.SubmitTargetData(target.position, target.GetComponent<Rigidbody>().velocity.normalized, target.GetComponent<Rigidbody>().velocity.magnitude);
    }
}
