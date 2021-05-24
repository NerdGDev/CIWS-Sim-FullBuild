using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motion))]
public class TrackingRadarController : MonoBehaviour
{
    [Header("Radar Beam")]
    [SerializeField] public GameObject radarCollider;
    public float optimalRange;
    public float width;
    public float height;

    public Motion motion;

    bool isTracking;
    int TargetSignature;
    Vector3 TargetCurrentPos;
    Queue<Vector3> TrackingData = new Queue<Vector3>();
    public bool DataReady;
    bool lastTickFound;
    bool countDown;
    public bool isDead;
    public int MaxReports;

    Visualise visualise;
    [SerializeField] public Vector3[] InspectorData;

    private void OnValidate()
    {
        radarCollider.transform.localScale = new Vector3(width, height, 1f * optimalRange * 1.5f);
    }

    private void Awake()
    {
        visualise = GetComponent<Visualise>();
        ResetTracking();
    }

    private void FixedUpdate()
    {
        InspectorData = GetTrackingData();
        visualise.AddDataField("Tracking", isTracking.ToString());
        visualise.AddDataField("Target Dead", isDead.ToString());
    }

    public Vector3[] GetTrackingData() { return TrackingData.ToArray(); }

    public void ReportHit(int signature, Vector3 position)
    {
        HandleHit(signature, position);
    }

    void HandleHit(int signature, Vector3 position) 
    {
        //Debug.Log("HIT");
        if (isTracking) 
        {
            //Debug.Log("IS TRACKING");
            if (signature == TargetSignature) 
            {
                //Debug.Log("IS VALID TARGET");
                lastTickFound = true;
                TargetCurrentPos = position;
                TrackingData.Enqueue(position);
                if (TrackingData.Count > MaxReports) 
                {
                    TrackingData.Dequeue();
                }
            }
        }
    }

    public void TrackTarget(int signature, Vector3 position) 
    {
        //Debug.LogWarning("TRACKING");
        ResetTracking();

        TargetSignature = signature;
        StartCoroutine(TrackingRoutine(signature, position));
    }

    IEnumerator TrackingRoutine(int signature, Vector3 position) 
    {
        motion.TargetToDirection(position);
        isTracking = true;
        Coroutine co = null;
        while (!isDead)
        {
            //Debug.Log("Tracking");
            //Debug.Log(TrackingData.Count);
            if (!lastTickFound && !countDown) 
            {
                visualise.AddDataField("Target Status", "CANNOT FIND");
                visualise.AddShortData("Action", "Lost Target");
                //Debug.LogWarning("DID NOT FIND");
                co = StartCoroutine(GiveUpRoutine());
            }
            if (lastTickFound && countDown) 
            {                
                if (co != null) 
                {
                    visualise.AddShortData("Action", "Reacquired Target");
                    //Debug.LogWarning("REACQUIRED");
                    //Debug.LogWarning(lastTickFound);
                    //Debug.LogWarning(countDown);
                    StopCoroutine(co);
                    countDown = false;
                }
            }
            if (lastTickFound) 
            {
                motion.TargetToDirection(TargetCurrentPos);
            }

            if (TrackingData.Count >= MaxReports) 
            {
                DataReady = true;
            }

            
            lastTickFound = false;
            yield return new WaitForFixedUpdate();
        }
        visualise.AddShortData("Action", "Target Dead");
        isTracking = false;
    }

    IEnumerator GiveUpRoutine() 
    {
        visualise.AddShortData("Action", "Waiting To Give Up");
        countDown = true;
        yield return new WaitForSeconds(1f);
        isDead = true;
        Debug.LogWarning("Dead");
    }

    void ResetTracking() 
    {
        TrackingData.Clear();
        countDown = false;
        isDead = false;
        TargetSignature = 0;
        DataReady = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(radarCollider.transform.position, radarCollider.transform.position + (radarCollider.transform.forward * 3000f));
        Gizmos.DrawWireSphere(TargetCurrentPos, 100f);
    }
}
